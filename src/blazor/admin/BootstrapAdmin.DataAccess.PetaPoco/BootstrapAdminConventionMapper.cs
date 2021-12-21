using BootstrapAdmin.DataAccess.PetaPoco.Coverters;
using PetaPoco;
using System.Reflection;

namespace BootstrapAdmin.DataAccess.PetaPoco
{
    class BootstrapAdminConventionMapper : ConventionMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoType"></param>
        /// <returns></returns>
        public override TableInfo GetTableInfo(Type pocoType)
        {
            var ti = base.GetTableInfo(pocoType);
            ti.AutoIncrement = true;

            // 支持 Oracle 数据库
            ti.SequenceName = $"SEQ_{ti.TableName.ToUpperInvariant()}_ID";

            ti.TableName = pocoType.Name switch
            {
                _ => $"{pocoType.Name}s"
            };
            return ti;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoProperty"></param>
        /// <returns></returns>
        public override ColumnInfo GetColumnInfo(PropertyInfo pocoProperty) => pocoProperty.DeclaringType?.Name switch
        {
            nameof(Models.User) => GetUserColumnInfo(pocoProperty),
            nameof(Models.Navigation) => GetNavigationColumnInfo(pocoProperty),
            _ => base.GetColumnInfo(pocoProperty)
        };

        private ColumnInfo GetUserColumnInfo(PropertyInfo pocoProperty)
        {
            var ci = base.GetColumnInfo(pocoProperty);
            var resultColumns = new List<string>
            {
                nameof(Models.User.Checked),
                nameof(Models.User.UserStatus),
                nameof(Models.User.Period),
                nameof(Models.User.NewPassword),
                nameof(Models.User.IsReset)
            };
            ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
            return ci;
        }

        private ColumnInfo GetNavigationColumnInfo(PropertyInfo pocoProperty)
        {
            var ci = base.GetColumnInfo(pocoProperty);
            var resultColumns = new List<string>
            {
                nameof(Models.Navigation.HasChildren)
            };
            ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
            return ci;
        }

        public override Func<object?, object?> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            if (targetProperty.PropertyType.IsEnum && sourceType == typeof(string))
            {
                return new StringToEnumConverter(targetProperty.PropertyType).ConvertFromDb;
            }
            return base.GetFromDbConverter(targetProperty, sourceType);
        }
    }
}
