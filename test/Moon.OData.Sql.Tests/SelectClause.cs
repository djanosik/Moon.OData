using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class SelectClauseTests
    {
        [Fact]
        public void BuildingClauseWhenCommandDoesNotSpecifyColumns()
        {
            var data = new Dictionary<string, string> {
                ["$select"] = "Id,Name"
            };

            var result = SelectClause.Build("SELECT FROM Table", new ODataOptions<Model>(data));
            result.Should().Be("SELECT [Id], [Name] FROM Table");
        }

        [Fact]
        public void BuildingClauseWhenCommandSpecifiesColumns()
        {
            var data = new Dictionary<string, string> {
                ["$select"] = "Id"
            };

            var result = SelectClause.Build("SELECT Name FROM Table", new ODataOptions<Model>(data));
            result.Should().Be("SELECT Name FROM Table");
        }

        [Fact]
        public void BuildingClauseWhenCommandSpecifiesTop()
        {
            var data = new Dictionary<string, string> {
                ["$top"] = "20"
            };

            var result = SelectClause.Build("SELECT TOP(40) FROM", new ODataOptions<Model>(data));
            result.Should().Be("SELECT TOP(40) * FROM");
        }

        [Fact]
        public void BuildingClauseWhenSelectContainsWildcard()
        {
            var data = new Dictionary<string, string> {
                ["$select"] = "*,Name"
            };

            var result = SelectClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("SELECT * FROM");
        }

        [Fact]
        public void BuildingClauseWhenSelectIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$select"] = "Id,Name"
            };

            var result = SelectClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("SELECT [Id], [Name] FROM");
        }

        [Fact]
        public void BuildingClauseWhenSelectIsNotDefined()
        {
            var data = new Dictionary<string, string>();
            var result = SelectClause.Build(new ODataOptions<Model>(data));

            result.Should().Be("SELECT * FROM");
        }

        [Fact]
        public void BuildingClauseWhenTopIsDefined()
        {
            var data = new Dictionary<string, string> {
                ["$top"] = "20"
            };

            var result = SelectClause.Build(new ODataOptions<Model>(data));
            result.Should().Be("SELECT TOP(20) * FROM");
        }
    }
}