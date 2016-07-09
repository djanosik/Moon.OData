using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OrderByClauseTests
    {
        [Fact]
        public void BuildClauseWhenOrderByIsNotDefined()
        {
            var data = new Dictionary<string, string>();
            var result = OrderByClause.Build(new ODataOptions<Model>(data));

            result.Should().BeEmpty();
        }

        [Fact]
        public void BuildingClauseWhenOrderByIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$orderby"] = "Id, Name desc",
                ["$skip"] = "20"
            };

            var result = OrderByClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("ORDER BY [Id], [Name] DESC");
        }
    }
}