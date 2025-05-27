namespace Sorry.Analyzers.Tests
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Xunit;
    using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Sorry.Analyzers.Sorry2000NamespaceMustMatchDirectoryStructure>;

    public class Sorry2000Tests
    {
        [Fact]
        public async Task WarnsWrongBlockNamespace()
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

            var expected = Verifier.Diagnostic()
                .WithSpan(startLine: 2, startColumn: 13, endLine: 10, endColumn: 14)
                .WithMessage($"Namespace X must match directory structure. Expected 0.");

            var sut = new CSharpAnalyzerTest<Sorry2000NamespaceMustMatchDirectoryStructure, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Add(expected);

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task WarnsWrongFileNamespace()
        {
            const string code = @"
            namespace X;

            class A
            {
                void Method(string arg1)
                {
                }
            }
            ";

            var expected = Verifier.Diagnostic()
                .WithLocation("/Y/TestDocument", 2, 13)
                .WithMessage($"Namespace X must match directory structure. Expected Y.");

            var sut = new CSharpAnalyzerTest<Sorry2000NamespaceMustMatchDirectoryStructure, XUnitVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.None,
                TestState =
                {
                    Sources =
                    {
                        ("/Y/TestDocument", code),
                    },
                    ExpectedDiagnostics = { expected, },
                },
            };

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task DoesNotWarnCorrectBlockNamespace()
        {
            const string code = @"
            namespace X;
            {
                class A
                {
                    void Method(string arg1)
                    {
                    }
                }
            }
            ";

            var sut = new CSharpAnalyzerTest<Sorry2000NamespaceMustMatchDirectoryStructure, XUnitVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.None,
                TestState =
                {
                    Sources =
                    {
                        ("/X/TestDocument", code),
                    },
                    ExpectedDiagnostics = { },
                },
            };

            await sut.RunAsync().ConfigureAwait(false);
        }
    }
}
