using System.Collections.Generic;
using FluentAssertions;
using Xbehave;

namespace Moon.OData.Sql.Tests
{
    public class WhereClauseTests
    {
        ODataOptions<Model> options;
        IList<object> arguments;
        string result;

        public WhereClauseTests()
        {
            "Given the arguments"
                .x(() => arguments = new List<object>());
        }

        [Scenario]
        public void BuildingClauseWhenFilterIsNotDefined()
        {
            "And the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build a WHERE clause"
                .x(() => result = WhereClause.Build("WHERE", arguments, options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Scenario]
        public void BuildingClauseWhenFilterIsDefined()
        {
            "And the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$filter"] = "Id eq 1 and (Name eq 'text' or tolower(Name) eq 'other') and Name ne null"
                }));

            "When I build a WHERE clause"
                .x(() => result = WhereClause.Build("WHERE", arguments, options));

            "Then it should return WHERE clause"
                .x(() =>
                {
                    result.Should().Be("WHERE ((([Id] = @p0) AND (([Name] = @p1) OR (LOWER([Name]) = @p2))) AND ([Name] IS NOT NULL))");
                });

            "And it should populate arguments"
                .x(() =>
                {
                    arguments[0].Should().Be(1L);
                    arguments[1].Should().Be("text");
                    arguments[2].Should().Be("other");
                });
        }
    }
}