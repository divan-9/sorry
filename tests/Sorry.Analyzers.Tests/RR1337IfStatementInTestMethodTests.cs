namespace Sorry.Analyzers.Tests
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using RoslynTestKit;
    using Xunit;

    public class RR1337IfStatementInTestMethodTests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;

        [Fact]
        public void XunitFactAttributeTest()
        {
            const string text = @"
namespace Tests;

using Xunit;

public class Foo
{
    [Fact]
    public void MyTestMethod()
    {
        [|if (true)|]
        {
        }
    }
}
";

            this.HasDiagnostic(
                markupCode: text,
                diagnosticId: DiagnosticIds.Sorry1337);
        }

        [Fact]
        public void XunitTheoryAttributeTest()
        {
            const string text = @"
namespace Tests;

using Xunit;

public class Foo
{
    [Theory]
    public void MyTestMethod()
    {
        [|if (true)|]
        {
        }
    }
}
";

            this.HasDiagnostic(
                markupCode: text,
                diagnosticId: DiagnosticIds.Sorry1337);
        }

        [Fact]
        public void NunitTestAttributeTest()
        {
            const string text = @"
namespace Tests;

using NUnit.Framework;

public class Foo
{
    [Test]
    public void MyTestMethod()
    {
        [|if (true)|]
        {
        }
    }
}
";

            this.HasDiagnostic(
                markupCode: text,
                diagnosticId: DiagnosticIds.Sorry1337);
        }

        [Fact]
        public void NunitTestCaseAttributeTest()
        {
            const string text = @"
namespace Tests;

using NUnit.Framework;

public class Foo
{
    [TestCase(123)]
    public void MyTestMethod(int foo)
    {
        [|if (true)|]
        {
        }
    }
}
";

            this.HasDiagnostic(
                markupCode: text,
                diagnosticId: DiagnosticIds.Sorry1337);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new Sorry1337IfStatementInTestMethod();
        }
    }
}