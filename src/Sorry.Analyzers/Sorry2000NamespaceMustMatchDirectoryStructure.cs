namespace Sorry.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class Sorry2000NamespaceMustMatchDirectoryStructure : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: DiagnosticIds.Sorry2000,
            title: "Namespace must match directory structure",
            messageFormat: "Namespace {0} must match directory structure. Expected {1}",
            category: "Namespace Rules",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "It's a good practice to keep namespace and file structure in sync",
            helpLinkUri: "TODO:");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Descriptor);

        public override void Initialize(
            AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(NamespaceNodeAction, SyntaxKind.NamespaceDeclaration);
        }

        private static void NamespaceNodeAction(
            SyntaxNodeAnalysisContext context)
        {
            var namespaceDeclaration = (NamespaceDeclarationSyntax)context.Node;
            var namespaceName = namespaceDeclaration.Name.GetText().ToString().Trim();

            var expectedNamespaceName = GetExpectedNamespace(
                fileDirectory: Path.GetDirectoryName(namespaceDeclaration.SyntaxTree.FilePath),
                namespaceSegmentsNumber: namespaceName.Split('.').Length);

            if (!expectedNamespaceName.Equals(namespaceName, StringComparison.Ordinal))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        Descriptor,
                        namespaceDeclaration.GetLocation(),
                        namespaceName,
                        expectedNamespaceName));
            }
        }

        private static string GetExpectedNamespace(
            string fileDirectory,
            int namespaceSegmentsNumber)
        {
            var expectedSegments = fileDirectory
                .Replace(Path.DirectorySeparatorChar, '.')
                .Split('.');

            var valuableSegments = expectedSegments
                .Skip(expectedSegments.Length - namespaceSegmentsNumber);

            return string.Join(".", valuableSegments);
        }
    }
}