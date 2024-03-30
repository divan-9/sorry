namespace Sorry.Analyzers
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(firstLanguage: LanguageNames.CSharp)]
    public class Sorry1337IfStatementInTestMethod : DiagnosticAnalyzer
    {
        private static readonly HashSet<string> TestAttributeNames =
            new HashSet<string>
            {
                "Test",
                "Theory",
                "Fact",
                "TestCase",
            };

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticIds.Sorry1337,
            title: "Tests should not contain `if` statements",
            messageFormat: "Test '{0}' contains `if` statement",
            category: "Testing",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Test contains `if` statement.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(item: Rule);

        public override void Initialize(
            AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(analysisMode: GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(action: AnalyzeSyntax, syntaxKinds: SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeSyntax(
            SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not MethodDeclarationSyntax methodDeclarationSyntax)
            {
                return;
            }

            var hasTestAttribute = methodDeclarationSyntax
                .AttributeLists
                .Any(predicate: attributeListSyntax =>
                    attributeListSyntax.Attributes
                        .Any(predicate: attributeSyntax =>
                            TestAttributeNames
                                .Contains(
                                    item: attributeSyntax.Name.ToString())));
            if (!hasTestAttribute)
            {
                return;
            }

            foreach (var ifStatement in methodDeclarationSyntax.DescendantNodes().OfType<IfStatementSyntax>())
            {
                context.ReportDiagnostic(
                    diagnostic: Diagnostic.Create(
                        descriptor: Rule,
                        location: ifStatement.GetLocation(),
                        messageArgs: methodDeclarationSyntax.Identifier.Text));
            }
        }
    }
}