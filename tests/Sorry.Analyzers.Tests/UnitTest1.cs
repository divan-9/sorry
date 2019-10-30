namespace Sorry.Analyzers.Tests
{
    using FluentAssertions;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string.Empty.Should().BeEmpty();
        }
    }
}
