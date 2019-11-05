namespace Sorry.Analyzers.Tests
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using RoslynTestKit;
    using Xunit;

    public class Sorry2000Tests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;

        [Fact]
        public void WarnsWrongNamespace()
        {
            const string code = @"
            namespace X
            {
                class A
                {
                    void Method(string arg1)
                    {
                    }
                }
            }
            ";

            var document = GetDocumentFromCode(code, "/Y/TestDocument");
            var locator = new TextSpan(23, 1);
            this.HasDiagnostic(document, DiagnosticIds.Sorry2000, locator);
        }

        [Fact]
        public void DoesNotWarnCorrectNamespace()
        {
            const string code = @"
            namespace X
            {
                class A
                {
                    void Method(string arg1)
                    {
                    }
                }
            }
            ";

            var document = GetDocumentFromCode(code, "/X/TestDocument");
            this.NoDiagnostic(document, DiagnosticIds.Sorry2000);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new Sorry2000NamespaceMustMatchDirectoryStructure();
        }

        private static Document GetDocumentFromCode(
            string code,
            string fileName)
        {
            var immutableReferencesBuilder = ImmutableArray.CreateBuilder<MetadataReference>();

            return new AdhocWorkspace()
                .AddProject("TestProject", LanguageNames.CSharp)
                .AddMetadataReferences(immutableReferencesBuilder.ToImmutable())
                .AddDocument(fileName, code);
        }
    }
}
