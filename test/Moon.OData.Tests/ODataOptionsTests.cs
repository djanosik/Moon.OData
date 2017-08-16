using System;
using System.Collections.Generic;
using Xunit;

namespace Moon.OData.Tests
{
    public class ODataOptionsTests
    {
        [Fact]
        public void GivenDictionaryWithCount_WhenCreating_ReturnsCountOption()
        {
            // Arrange
            var dic = new Dictionary<string, string>() {["$count"]="true" };

            // Act
            var options = new ODataOptions<Model>(dic);

            //Assert
            Assert.True(options.Count);
        }

        [Fact]
        public void GivenDictionaryWithExpand_WhenCreating_ReturnsCountOption()
        {
            // Arrange
            var dic = new Dictionary<string, string>() { ["$expand"] = "Model" };

            // Act
            var options = new ODataOptions<ModelItem>(dic);

            //Assert
            Assert.NotNull(options.SelectAndExpand);
            Assert.True(options.SelectAndExpand.AllSelected);
        }
    }
}
