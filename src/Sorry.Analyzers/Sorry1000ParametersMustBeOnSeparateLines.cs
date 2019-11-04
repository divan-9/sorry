namespace Sorry.Analyzers
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class Sorry1000ParametersMustBeOnSeparateLines : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: DiagnosticIds.Sorry1000,
            title: "Parameters must be on separate lines",
            messageFormat: "Parameters must be on separate lines",
            category: "Readability Rules",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Parameters must be on separate lines",
            helpLinkUri: "TODO:");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
            = ImmutableArray.Create(Descriptor);

        public override void Initialize(
            AnalysisContext context)
        {
            var methodDeclarationKinds = ImmutableArray.Create(
                SyntaxKind.ConstructorDeclaration,
                SyntaxKind.MethodDeclaration);

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(BaseMethodDeclarationAction, methodDeclarationKinds);
        }

        private static void BaseMethodDeclarationAction(
            SyntaxNodeAnalysisContext context)
        {
            var declaration = (BaseMethodDeclarationSyntax)context.Node;

            HandleParameterListSyntax(
                context: context,
                parameterList: declaration.ParameterList);
        }

        private static void HandleParameterListSyntax(
            SyntaxNodeAnalysisContext context,
            ParameterListSyntax parameterList)
        {
            if (parameterList == null)
            {
                return;
            }

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;
            Analyze(
                context: context,
                parameters: parameters,
                openParenLine: OpenParenLine(parameterList));
        }

        private static void Analyze(
            SyntaxNodeAnalysisContext context,
            SeparatedSyntaxList<ParameterSyntax> parameters,
            int openParenLine)
        {
            if (ParameterLine(parameters[0]) == openParenLine)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(Descriptor, parameters[0].GetLocation()));
            }

            for (var index = 1; index < parameters.Count; index++)
            {
                var prev = parameters[index - 1];
                var current = parameters[index];
                if (ParameterLine(prev) == ParameterLine(current))
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(Descriptor, current.GetLocation()));
                }
            }
        }

        private static int OpenParenLine(
            ParameterListSyntax parameterList)
        {
            return parameterList.OpenParenToken.GetLocation().GetLineSpan().EndLinePosition.Line;
        }

        private static int ParameterLine(
            ParameterSyntax parameterSyntax)
        {
            return parameterSyntax.GetLocation().GetLineSpan().EndLinePosition.Line;
        }
    }
}