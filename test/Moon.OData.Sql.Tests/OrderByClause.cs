using System.Collections.Generic;
using FluentAssertions;
using Moon.Testing;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OrderByClauseTests : TestSetup
    {
        ODataOptions<Model> options;
        string result;

        [Fact]
        public void BuildClauseWhenOrderByIsNotDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build an ORDER BY clause"
                .x(() => result = OrderByClause.Build(options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Fact]
        public void BuildingClauseWhenOrderByIsDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$orderby"] = "Id, Name desc",
                    ["$skip"] = "20"
                }));

            "When I build an ORDER BY clause"
                .x(() => result = OrderByClause.Build(options));

            "Then it should return ORDER BY clause"
                .x(() =>
                {
                    result.Should().Be("ORDER BY [Id], [Name] DESC");
                });
        }
    }
}