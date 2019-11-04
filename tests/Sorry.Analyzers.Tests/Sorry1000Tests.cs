namespace Sorry.Analyzers.Tests
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using RoslynTestKit;
    using Xunit;

    public class Sorry1000Tests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;

        [Fact]
        public void WarnsSingleArgument()
        {
            const string code = @"
                class A
                {
                    void Method([|string arg1|])
                    {
                    }
                }
            ";

            this.HasDiagnostic(code, DiagnosticIds.Sorry1000);
        }

        [Fact]
        public void WarnsArgumentsOnSameLine()
        {
            const string code = @"
                class A
                {
                    void Method(
                        string arg0, [|string arg1|])
                    {
                    }
                }
            ";

            this.HasDiagnostic(code, DiagnosticIds.Sorry1000);
        }

        [Fact]
        public void DoesNotWarnCorrectlyFormattedArguments()
        {
            const string code = @"
                class A
                {
                    void Method(
                        string arg0,
                        string arg1)
                    {
                    }
                }
            ";

            this.NoDiagnostic(code, DiagnosticIds.Sorry1000);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new Sorry1000ParametersMustBeOnSeparateLines();
        }
    }
}
