namespace Sorry.Analyzers.Tests
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using RoslynTestKit;
    using Xunit;

    public class Sorry3000Tests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;

        public static IEnumerable<object[]> MarkupCodeSamplesForTestsWithIfStatement()
        {
            yield return new object[]
            {
                @"
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
",
            };
            yield return new object[]
            {
                @"
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
",
            };
            yield return new object[]
            {
                @"
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
",
            };
            yield return new object[]
            {
                @"
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
",
            };
        }

        public static IEnumerable<object[]> CodeSamplesForTestsWithoutIfStatement()
        {
            yield return new object[]
            {
                @"
namespace Tests;

using Xunit;

public class Foo
{
    [Fact]
    public void MyTestMethod()
    {
        while (true)
        {
        }
    }
}
",
            };
            yield return new object[]
            {
                @"
namespace Tests;

using Xunit;

public class Foo
{
    [Theory]
    public void MyTestMethod()
    {
        while (true)
        {
        }
    }
}
",
            };
            yield return new object[]
            {
                @"
namespace Tests;

using NUnit.Framework;

public class Foo
{
    [Test]
    public void MyTestMethod()
    {
        while (true)
        {
        }
    }
}
",
            };
            yield return new object[]
            {
                @"
namespace Tests;

using NUnit.Framework;

public class Foo
{
    [TestCase(123)]
    public void MyTestMethod(int foo)
    {
        while (true)
        {
        }
    }
}
",
            };
        }

        [Theory]
        [MemberData(nameof(MarkupCodeSamplesForTestsWithIfStatement))]
        public void WarnsTestContainsIfStatement(
            string markupCode)
        {
            this.HasDiagnostic(
                markupCode: markupCode,
                diagnosticId: DiagnosticIds.Sorry3000);
        }

        [Theory]
        [MemberData(nameof(CodeSamplesForTestsWithoutIfStatement))]
        public void DoesNotWarnTestWithoutIfStatement(
            string code)
        {
            this.NoDiagnostic(
                code: code,
                diagnosticId: DiagnosticIds.Sorry3000);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new Sorry3000TestShouldNotContainIfStatement();
        }
    }
}