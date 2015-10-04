using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class SelectClauseTests
    {
        [Fact]
        public void Build_WhenSelectIsNotDefined_RetrunsSelectClauseWithWildcard()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.Equal("SELECT * FROM", SelectClause.Build(options));
        }

        [Fact]
        public void Build_WhenSelectContainsWildcard_RetrunsSelectClauseWithWildcard()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$select"] = "*,Name"
            });

            Assert.Equal("SELECT * FROM", SelectClause.Build(options));
        }

        [Fact]
        public void Build_WhenTopIsDefined_RetrunsSelectTopClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$top"] = "20"
            });

            Assert.Equal("SELECT TOP(20) * FROM", SelectClause.Build(options));
        }

        [Fact]
        public void Build_WhenSelectIsDefined_RetrunsSelectClauseWithColumns()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$select"] = "Id,Name"
            });

            Assert.Equal("SELECT [Id], [Name] FROM", SelectClause.Build(options));
        }

        [Fact]
        public void Build_WithCommandTextNotSpecifyingColumns_RetrunsSelectClauseWithColumns()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$select"] = "Id,Name"
            });

            Assert.Equal("SELECT [Id], [Name] FROM Table", SelectClause.Build("SELECT FROM Table", options));
        }

        [Fact]
        public void Build_WithCommandTextSpecifyingTop_WillNotChangeTopClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$top"] = "20"
            });

            Assert.Equal("SELECT TOP(40) * FROM", SelectClause.Build("SELECT TOP(40) FROM", options));
        }

        [Fact]
        public void Build_WithCommandTextSpecifyingColumns_WillNotChangeColumns()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$select"] = "Id"
            });

            Assert.Equal("SELECT Name FROM Table", SelectClause.Build("SELECT Name FROM Table", options));
        }
    }
}