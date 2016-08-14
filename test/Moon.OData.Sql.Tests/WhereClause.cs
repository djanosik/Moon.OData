using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class WhereClauseTests
    {
        private readonly IList<object> arguments = new List<object>();

        [Fact]
        public void BuildingClauseWhenFilterIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$filter"] = "Id eq 1 and (Name eq 'text' or tolower(Name) eq 'other') and Name ne null"
            };

            var result = WhereClause.Build("WHERE", arguments, new ODataOptions<Model>(data));
            result.Should().Be("WHERE ((([Id] = @p0) AND (([Name] = @p1) OR (LOWER([Name]) = @p2))) AND ([Name] IS NOT NULL))");

            arguments[0].Should().Be(1L);
            arguments[1].Should().Be("text");
            arguments[2].Should().Be("other");
        }

        [Fact]
        public void BuildingClauseWhenFilterIsNotDefined()
        {
            var data = new Dictionary<string, string>();
            var result = WhereClause.Build("WHERE", arguments, new ODataOptions<Model>(data));

            result.Should().BeEmpty();
        }
    }
}