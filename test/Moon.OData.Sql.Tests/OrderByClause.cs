using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class OrderByClauseTests
    {
        [Fact]
        public void Build_WhenOrderByIsNotDefined_RetrunsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.True(OrderByClause.Build(options).Length == 0);
        }

        [Fact]
        public void Build_WhenOrderByIsDefined_RetrunsOrderByClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$orderby"] = "Id, Name desc"
            });

            Assert.Equal("ORDER BY [Id], [Name] DESC", OrderByClause.Build(options));
        }
    }
}