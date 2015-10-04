using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class TopClauseTests
    {
        [Fact]
        public void Build_WhenTopIsNotDefined_RetrunsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.True(TopClause.Build(options).Length == 0);
        }

        [Fact]
        public void Build_WhenOnlyTopIsDefined_RetrunsTopClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$top"] = "20"
            });

            Assert.Equal("TOP(20)", TopClause.Build(options));
        }

        [Fact]
        public void Build_WhenTopAndSkipAreBothDefined_ReturnsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$top"] = "20",
                ["$skip"] = "5"
            });

            Assert.True(TopClause.Build(options).Length == 0);
        }
    }
}