// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using PetaPoco;
using System.Linq.Expressions;
using System.Reflection;

namespace BootstrapAdmin.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static Task<List<TModel>> FetchAsync<TModel>(this IDatabase db, QueryPageOptions options)
        {
            var sql = new Sql();

            // 处理模糊查询
            if (options.Searchs.Any())
            {
                var searchTextSql = new Sql();
                AnalysisExpression(options.Searchs.GetFilterLambda<TModel>(FilterLogic.Or), db, searchTextSql);
                sql.Append(searchTextSql.ToString().Replace("\nAND", "\nOR"), searchTextSql.Arguments);
            }

            // 处理高级搜索与过滤
            var filters = options.Filters.Concat(options.CustomerSearchs);
            if (filters.Any())
            {
                AnalysisExpression(filters.GetFilterLambda<TModel>(), db, sql);
            }

            var sortName = options.SortName;
            var sortOrder = options.SortOrder;
            if (!string.IsNullOrEmpty(sortName) && sortOrder != SortOrder.Unset)
            {
                sql.OrderBy(sortOrder == SortOrder.Asc ? sortName : $"{sortName} desc");
            }
            return db.FetchAsync<TModel>(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static Task<Page<TModel>> PageAsync<TModel>(this IDatabase db, QueryPageOptions options)
        {
            var sql = new Sql();

            // 处理模糊查询
            if (options.Searchs.Any())
            {
                var searchTextSql = new Sql();
                AnalysisExpression(options.Searchs.GetFilterLambda<TModel>(FilterLogic.Or), db, searchTextSql);
                sql.Append(searchTextSql.ToString().Replace("\nAND", "OR"), searchTextSql.Arguments);
            }

            // 处理高级搜索与过滤
            var filters = options.Filters.Concat(options.CustomerSearchs);
            if (filters.Any())
            {
                AnalysisExpression(filters.GetFilterLambda<TModel>(), db, sql);
            }

            var sortName = options.SortName;
            var sortOrder = options.SortOrder;

            if (!string.IsNullOrEmpty(sortName) && sortOrder != SortOrder.Unset)
            {
                sql.OrderBy(sortOrder == SortOrder.Asc ? sortName : $"{sortName} desc");
            }
            return db.PageAsync<TModel>(options.PageIndex, options.PageItems, sql);
        }

        private static void AnalysisExpression(Expression expression, IDatabase db, Sql sql)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    if (expression is LambdaExpression exp)
                    {
                        AnalysisExpression(exp.Body, db, sql);
                    }
                    break;
                case ExpressionType.AndAlso:
                    if (expression is BinaryExpression andExp)
                    {
                        AnalysisExpression(andExp.Left, db, sql);
                        AnalysisExpression(andExp.Right, db, sql);
                    }
                    break;
                case ExpressionType.OrElse:
                    if (expression is BinaryExpression orExp)
                    {
                        AnalysisExpression(orExp.Left, db, sql);
                        AnalysisExpression(orExp.Right, db, sql);
                    }
                    break;
                case ExpressionType.Call:
                    if (expression is MethodCallExpression callExp)
                    {
                        if (callExp.Method.Name == "Contains")
                        {
                            if (callExp.Object is MemberExpression callLeft)
                            {
                                var callColName = GetColumnName(callLeft.Member) ?? callLeft.Member.Name;
                                var p = (callExp.Arguments[0] as ConstantExpression)?.Value;
                                if (p != null)
                                {
                                    sql.Where($"{db.Provider.EscapeSqlIdentifier(callColName)} like @0", $"%{p}%");
                                }
                            }
                        }
                    }
                    break;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    if (expression is BinaryExpression binaryExp)
                    {
                        MemberExpression? left = null;
                        if (binaryExp.Left is MethodCallExpression methodLeft && methodLeft.Method.Name == "ToString" && methodLeft.Object is MemberExpression p && p.Type.IsEnum)
                        {
                            // 枚举转字符串
                            left = p;
                        }
                        else if (binaryExp.Left is MemberExpression m)
                        {
                            left = m;
                        }

                        if (left != null)
                        {
                            // 查找 PetaPoco.Column 标签
                            var columnName = GetColumnName(left.Member) ?? left.Member.Name;

                            // 查找操作符右侧
                            if (binaryExp.Right is ConstantExpression right)
                            {
                                var v = right.Value;

                                if (v != null)
                                {
                                    var val = v.GetType().IsEnum ? v.ToString() : v;
                                    var operatorExp = GetOperatorExpression(expression);
                                    sql.Where($"{db.Provider.EscapeSqlIdentifier(columnName)} {operatorExp} @0", val);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private static string? GetColumnName(MemberInfo member) => member.CustomAttributes
            .FirstOrDefault(i => i.AttributeType == typeof(ColumnAttribute))?.NamedArguments
            .FirstOrDefault(i => i.MemberName == "Name").TypedValue.Value?.ToString();

        private static string GetOperatorExpression(Expression expression) => expression.NodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            _ => ""
        };
    }
}
