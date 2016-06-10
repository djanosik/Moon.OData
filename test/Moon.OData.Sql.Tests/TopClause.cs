using System.Collections.Generic;
using FluentAssertions;
using Xbehave;

namespace Moon.OData.Sql.Tests
{
    public class TopClauseTests
    {
        ODataOptions<Model> options;
        string result;

        [Scenario]
        public void BuildingClauseWhenTopIsNotDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build a TOP clause"
                .x(() => result = TopClause.Build(options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Scenario]
        public void BuildClauseWhenOnlyTopIsDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$top"] = "20"
                }));

            "When I build a TOP clause"
                .x(() => result = TopClause.Build(options));

            "Then it should return TOP clause"
                .x(() =>
                {
                    result.Should().Be("TOP(20)");
                });
        }

        [Scenario]
        public void BuildingClauseWhenTopAndSkipAreDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$top"] = "20",
                    ["$skip"] = "5"
                }));

            "When I build a TOP clause"
                .x(() => result = TopClause.Build(options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }
    }
}