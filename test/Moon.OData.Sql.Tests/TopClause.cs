using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class TopClauseTests
    {
        [Fact]
        public void BuildClauseWhenOnlyTopIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$top"] = "20"
            };

            var result = TopClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("TOP(20)");
        }

        [Fact]
        public void BuildingClauseWhenTopAndSkipAreDefined()
        {
            var data = new Dictionary<string, string> {
                ["$top"] = "20",
                ["$skip"] = "5"
            };

            var result = TopClause.Build(new ODataOptions<Model>(data));
            result.Should().BeEmpty();
        }

        [Fact]
        public void BuildingClauseWhenTopIsNotDefined()
        {
            var data = new Dictionary<string, string>();
            var result = TopClause.Build(new ODataOptions<Model>(data));

            result.Should().BeEmpty();
        }
    }
}