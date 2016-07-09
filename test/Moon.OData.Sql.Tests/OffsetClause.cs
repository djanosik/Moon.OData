using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OffsetClauseTests
    {
        [Fact]
        public void BuildingClauseWhenOrderByOrSkipIsNotDefined()
        {
            var data = new Dictionary<string, string>();
            var result = OffsetClause.Build(new ODataOptions<Model>(data));

            result.Should().BeEmpty();
        }

        [Fact]
        public void BuildingClauseWhenSkipAndTopAreDefined()
        {
            var data = new Dictionary<string, string> {
                ["$orderby"] = "Id desc",
                ["$skip"] = "40",
                ["$top"] = "20"
            };

            var result = OffsetClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY");
        }

        [Fact]
        public void BuildingClauseWhenSkipIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$orderby"] = "Id desc",
                ["$skip"] = "20"
            };

            var result = OffsetClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("OFFSET 20 ROWS");
        }
    }
}