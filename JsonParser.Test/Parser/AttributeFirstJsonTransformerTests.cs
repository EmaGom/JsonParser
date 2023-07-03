using System.IO;
using System.Text;
using JsonParser.Parser;
using NUnit.Framework;

namespace Tests
{
    public class AttributeFirstJsonTransformerTests
    {
        [Test]
        public void WhenEmptyObjectThenEmptyObject()
        {
            // Arrange
            var input = new MemoryStream(Encoding.ASCII.GetBytes("{}"));

            // Act
            var output = AttributeFirstJsonTransformer.Transform(input);
            var actual = new StreamReader(output).ReadToEnd();

            // Assert
            Assert.AreEqual("{}", actual);
        }
    }
}