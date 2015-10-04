using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class CountClauseTests
    {
        [Fact]
        public void Build_RetrunsCountClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.Equal("SELECT COUNT([Id]) FROM", CountClause.Build(options));
        }
    }
}