using System.Collections.Generic;
using FluentAssertions;
using Moon.Testing;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OffsetClauseTests : TestSetup
    {
        ODataOptions<Model> options;
        string result;

        [Fact]
        public void BuildingClauseWhenOrderByIsNotDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build an OFFSET clause"
                .x(() => result = OffsetClause.Build(options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Fact]
        public void BuildingClauseWhenSkipIsNotDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build an OFFSET clause"
                .x(() => result = OffsetClause.Build(options));

            "Then it should return empty string"
                .x(() =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Fact]
        public void BuildingClauseWhenOnlySkipIsDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$orderby"] = "Id desc",
                    ["$skip"] = "20"
                }));

            "When I build an OFFSET clause"
                .x(() => result = OffsetClause.Build(options));

            "Then it should return OFFSET clause"
                .x(() =>
                {
                    result.Should().Be("OFFSET 20 ROWS");
                });
        }

        [Fact]
        public void BuildingClauseWhenSkipAndTopAreDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$orderby"] = "Id desc",
                    ["$skip"] = "40",
                    ["$top"] = "20"
                }));

            "When I build an OFFSET clause"
                .x(() => result = OffsetClause.Build(options));

            "Then it should return OFFSET clause"
                .x(() =>
                {
                    result.Should().Be("OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY");
                });
        }
    }
}