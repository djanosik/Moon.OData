using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Sql.Tests
{
    public class WhereClauseTests
    {
        readonly IList<object> arguments = new List<object>();

        [Fact]
        public void Build_WhenFilterIsNotDefined_RetrunsEmptyString()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string> { });
            Assert.True(WhereClause.Build("WHERE", arguments, options).Length == 0);
        }

        [Fact]
        public void Build_WhenFilterIsDefined_RetrunsWhereClause()
        {
            var options = new ODataOptions<Model>(new Dictionary<string, string>
            {
                ["$filter"] = "Id eq 1 and (Name eq 'text' or tolower(Name) eq 'other') and Name ne null"
            });

            var result = WhereClause.Build("WHERE", arguments, options);

            Assert.Equal("WHERE ((([Id] = @p0) AND (([Name] = @p1) OR (LOWER([Name]) = @p2))) AND ([Name] IS NOT NULL))", result);
            Assert.Equal(1L, arguments[0]);
            Assert.Equal("text", arguments[1]);
            Assert.Equal("other", arguments[2]);
        }
    }
}