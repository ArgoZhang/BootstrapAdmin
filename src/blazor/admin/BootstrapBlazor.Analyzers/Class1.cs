// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace BootstrapBlazor.Analyzers;

/// <summary>
/// 
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BlazorAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// 
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(new DiagnosticDescriptor("Test", "Test", "{0} Test", "Naming", DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "Test")); } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(syntaxNodeContext =>
        {

        }, SyntaxKind.IdentifierName);
    }
}
