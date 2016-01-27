using System.Collections.Generic;
using FluentAssertions;
using Moon.Testing;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class CountClauseTests : TestSetup
    {
        ODataOptions<Model> options;
        string result;

        [Fact]
        public void BuldingClause()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build a COUNT clause"
                .x(() => result = CountClause.Build(options));

            "Then it should return COUNT query"
                .x(() =>
                {
                    result.Should().Be("SELECT COUNT([Id]) FROM");
                });
        }
    }
}