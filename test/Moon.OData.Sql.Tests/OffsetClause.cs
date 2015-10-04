using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OffsetClauseTests
    {
        [Fact]
        public void Build_WhenOrderByIsNotDefined_RetrunsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.True(OffsetClause.Build(options).Length == 0);
        }
        [Fact]
        public void Build_WhenSkipIsNotDefined_RetrunsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.True(OffsetClause.Build(options).Length == 0);
        }

        [Fact]
        public void Build_WhenSkipIsDefined_RetrunsOffsetClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$orderby"] = "Id desc",
                ["$skip"] = "20"
            });

            Assert.Equal("OFFSET 20 ROWS", OffsetClause.Build(options));
        }

        [Fact]
        public void Build_WhenSkipAndTopIsDefined_RetrunsOffsetFetchClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$orderby"] = "Id desc",
                ["$skip"] = "40",
                ["$top"] = "20"
            });

            Assert.Equal("OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY", OffsetClause.Build(options));
        }
    }
}