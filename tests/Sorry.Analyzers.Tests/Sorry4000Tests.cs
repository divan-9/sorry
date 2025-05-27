namespace Sorry.Analyzers.Tests
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Xunit;
    using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
       Sorry.Analyzers.Sorry4000DenyPrimaryConstructorAnalyzer>;

    public class Sorry4000Tests
    {
        [Fact]
        public async Task T1()
        {
            var code = """
                                namespace A;
                                public class B()
                                {
                                }
                                """;

            var expected = Verifier.Diagnostic()
                .WithSpan(startLine: 2, startColumn: 15, endLine: 2, endColumn: 17)
                .WithMessage("Detected primary constructor usage");

            var sut = new CSharpAnalyzerTest<Sorry4000DenyPrimaryConstructorAnalyzer, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Add(expected);

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task T2()
        {
            var code = """
                                namespace A;
                                public class B
                                {
                                }
                                """;

            var sut = new CSharpAnalyzerTest<Sorry4000DenyPrimaryConstructorAnalyzer, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            await sut.RunAsync().ConfigureAwait(false);
        }
    }
}
