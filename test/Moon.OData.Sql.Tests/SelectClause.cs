using System.Collections.Generic;
using FluentAssertions;
using Moon.Testing;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class SelectClauseTests : TestSetup
    {
        ODataOptions<Model> options;
        string command, result;

        [Fact]
        public void BuildingClauseWhenSelectIsNotDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string> { }));

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(options));

            "Then it should return SELECT query"
                .x(() =>
                {
                    result.Should().Be("SELECT * FROM");
                });
        }

        [Fact]
        public void BuildingClauseWhenSelectContainsWildcard()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$select"] = "*,Name"
                }));

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(options));

            "Then it should return SELECT query"
                .x(() =>
                {
                    result.Should().Be("SELECT * FROM");
                });
        }

        [Fact]
        public void BuildingClauseWhenTopIsDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$top"] = "20"
                }));

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(options));

            "Then it should return SELECT query"
                .x(() =>
                {
                    result.Should().Be("SELECT TOP(20) * FROM");
                });
        }

        [Fact]
        public void BuildingClauseWhenSelectIsDefined()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$select"] = "Id,Name"
                }));

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(options));

            "Then it should return SELECT query"
                .x(() =>
                {
                    result.Should().Be("SELECT [Id], [Name] FROM");
                });
        }

        [Fact]
        public void BuildingClauseWhenCommandDoesNotSpecifyColumns()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$select"] = "Id,Name"
                }));

            "And the SQL command without columns specified"
                .x(() => command = "SELECT FROM Table");

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(command, options));

            "Then it should return SELECT query"
                .x(() =>
                {
                    result.Should().Be("SELECT [Id], [Name] FROM Table");
                });
        }

        [Fact]
        public void BuildingClauseWhenCommandSpecifiesTop()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$top"] = "20"
                }));

            "And the SQL command specifying TOP clause"
                .x(() => command = "SELECT TOP(40) FROM");

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(command, options));

            "Then it should ignore the $top option"
                .x(() =>
                {
                    result.Should().Be("SELECT TOP(40) * FROM");
                });
        }

        [Fact]
        public void BuildingClauseWhenCommandSpecifiesColumns()
        {
            "Given the options"
                .x(() => options = new ODataOptions<Model>(new Dictionary<string, string>
                {
                    ["$select"] = "Id"
                }));

            "And the SQL command with columns specified"
                .x(() => command = "SELECT Name FROM Table");

            "When I build a SELECT clause"
                .x(() => result = SelectClause.Build(command, options));

            "Then it should ignore the $select option"
                .x(() =>
                {
                    result.Should().Be("SELECT Name FROM Table");
                });
        }
    }
}