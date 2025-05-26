namespace Sorry.Analyzers.Tests
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Xunit;

    public class Sorry1000Tests
    {
        [Fact]
        public async Task WarnsSingleArgument()
        {
            const string code = @"
                class A
                {
                    void Method([|string arg1|])
                    {
                    }
                }
            ";

            var sut = new CSharpAnalyzerTest<Sorry1000ParametersMustBeOnSeparateLines, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task WarnsArgumentsOnSameLine()
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

            var sut = new CSharpAnalyzerTest<Sorry1000ParametersMustBeOnSeparateLines, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task DoesNotWarnCorrectlyFormattedArguments()
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

            var sut = new CSharpAnalyzerTest<Sorry1000ParametersMustBeOnSeparateLines, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Clear();

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task DoesNotWarnEmptyArgumentList()
        {
            const string code = @"
                class A
                {
                    void Method()
                    {
                    }
                }
            ";

            var sut = new CSharpAnalyzerTest<Sorry1000ParametersMustBeOnSeparateLines, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Clear();

            await sut.RunAsync().ConfigureAwait(false);
        }
    }
}
