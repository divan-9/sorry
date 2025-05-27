namespace Sorry.Analyzers.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Testing;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Testing.Verifiers;
    using Xunit;
    using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Sorry.Analyzers.Sorry3000TestShouldNotContainIfStatement>;

    public class Sorry3000Tests
    {
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
            yield return new object[]
            {
                @"
namespace Tests;

using NUnit.Framework;

public class Foo
{
    public void MyTestMethod(int foo)
    {
        if (true)
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
    [AnAttribute]
    public void MyTestMethod(int foo)
    {
        if (true)
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
        public async Task WarnsTestContainsIfStatementAsync(
            string markupCode)
        {
            var code = markupCode.Replace("[|", string.Empty).Replace("|]", string.Empty);

            var expected = Verifier.Diagnostic()
                .WithSpan(startLine: 11, startColumn: 9, endLine: 13, endColumn: 10)
                .WithMessage("Test 'MyTestMethod' contains `if` statement");

            var sut = new CSharpAnalyzerTest<Sorry3000TestShouldNotContainIfStatement, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Add(expected);

            await sut.RunAsync().ConfigureAwait(false);
        }

        [Theory]
        [MemberData(nameof(CodeSamplesForTestsWithoutIfStatement))]
        public async Task DoesNotWarnTestWithoutIfStatementAsync(
            string code)
        {
            var sut = new CSharpAnalyzerTest<Sorry3000TestShouldNotContainIfStatement, XUnitVerifier>
            {
                TestCode = code,
                CompilerDiagnostics = CompilerDiagnostics.None,
            };

            sut.ExpectedDiagnostics.Clear();

            await sut.RunAsync().ConfigureAwait(false);
        }
    }
}