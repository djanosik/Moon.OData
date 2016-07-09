using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class CountClauseTests
    {
        [Fact]
        public void BuildingClause()
        {
            var data = new Dictionary<string, string>();
            var result = CountClause.Build(new ODataOptions<Model>(data));

            result.Should().Be("SELECT COUNT([Id]) FROM");
        }
    }
}