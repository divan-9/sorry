namespace Sorry.Analyzers
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Sorry4000DenyPrimaryConstructorAnalyzer :
        DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new (
            id: DiagnosticIds.Sorry4000,
            title: "Code contains primary constructor usage",
            messageFormat: "Detected primary constructor usage",
            category: "Readability Rules",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Code shouldn't contain any primary constructor usage.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Rule);

        public override void Initialize(
            AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(this.AnalyzePrimaryConstructor, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzePrimaryConstructor(
            SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (classDeclaration.ParameterList != null)
            {
                var diagnostic = Diagnostic.Create(
                    descriptor: Rule,
                    location: classDeclaration.ParameterList.GetLocation());

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}