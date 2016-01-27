using System.Collections.Generic;
using FluentAssertions;
using Moon.Testing;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class TopClauseTests : TestSetup
    {
        ODataOptions<Model> options;
        string result;

        [Fact]
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

        [Fact]
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

        [Fact]
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