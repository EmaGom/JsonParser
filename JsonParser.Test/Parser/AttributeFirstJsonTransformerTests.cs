using System.IO;
using System.Text;
using JsonParser.Parser;
using JsonParser.Utils;
using Newtonsoft.Json;
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

        [Test]
        [TestCase("{\r\n\"FirstName\": \"Arthur\",\r\n\"LastName\": \"Bertrand\",\r\n\"Adrress\": {\r\n\"StreetName\": \"Gedempte Zalmhaven\",\r\n\"Number\": \"4K\",\r\n\"City\": {\r\n\"Name\": \"Rotterdam\",\r\n\"Country\": \"Netherlands\"\r\n},\r\n\"ZipCode\": \"3011 BT\"\r\n},\r\n\"Age\": 35,\r\n\"Hobbies\": [\"Fishing\", \"Rowing\"]\r\n}",
           "{\r\n  \"Age\": 35,\r\n  \"LastName\": \"Bertrand\",\r\n  \"FirstName\": \"Arthur\",\r\n  \"Adrress\": {\r\n    \"ZipCode\": \"3011 BT\",\r\n    \"Number\": \"4K\",\r\n    \"StreetName\": \"Gedempte Zalmhaven\",\r\n    \"City\": {\r\n      \"Country\": \"Netherlands\",\r\n      \"Name\": \"Rotterdam\"\r\n    }\r\n  },\r\n  \"Hobbies\": [\r\n    \"Rowing\",\r\n    \"Fishing\"\r\n  ]\r\n}")]
        [TestCase("{\r\n\"FirstName\": \"Arthur\",\r\n\"Hobbies\": [\r\n\t{\r\n\t\t\"Fishing\": \"Yes\"\r\n\t}, \r\n\t\"Rowing\"\r\n],\r\n\"LastName\": \"Bertrand\",\r\n\"Adrress\": {\r\n\t\"StreetName\": \"Gedempte Zalmhaven\",\r\n\t\"Number\": \"4K\",\r\n\t\"City\": {\r\n\t\"Name\": \"Rotterdam\",\r\n\t\"Country\": \"Netherlands\"\r\n\t},\r\n\t\"ZipCode\": \"3011 BT\"\r\n},\r\n\"Age\": 35\r\n}",
            "{\r\n  \"Age\": 35,\r\n  \"LastName\": \"Bertrand\",\r\n  \"FirstName\": \"Arthur\",\r\n  \"Hobbies\": [\r\n    \"Rowing\",\r\n    {\r\n      \"Fishing\": \"Yes\"\r\n    }\r\n  ],\r\n  \"Adrress\": {\r\n    \"ZipCode\": \"3011 BT\",\r\n    \"Number\": \"4K\",\r\n    \"StreetName\": \"Gedempte Zalmhaven\",\r\n    \"City\": {\r\n      \"Country\": \"Netherlands\",\r\n      \"Name\": \"Rotterdam\"\r\n    }\r\n  }\r\n}")]
        public void WhenRightObjectThenOrderedObject(string pInput, string pExpectedOutput)
        {
            // Arrange
            var input = new MemoryStream(Encoding.ASCII.GetBytes(pInput));
            var expectedOutputJson = new MemoryStream(Encoding.ASCII.GetBytes(pExpectedOutput));
            var expectedOutput = new StreamReader(expectedOutputJson).ReadToEnd();

            // Act
            var output = AttributeFirstJsonTransformer.Transform(input);
            var actual = new StreamReader(output).ReadToEnd();

            // Assert
            Assert.AreEqual(actual, expectedOutput);
        }

        [Test]
        [TestCase("{\"Name\": \"Jhon\",:[Test}")]
        [TestCase("\"Name\": \"Jhon\",:[Test}")]
        [TestCase("{\"Name\": \"Jhon\",[Test]")]
        [TestCase("{\r\n\t\"Age\": 35,\r\n\t\"LastName\": \"Bertrand\",\r\n\t\"FirstName\": \"Arthur\",\r\n\t\"Adrress\": {\r\n\t\t\"ZipCode\": \"3011 BT\"\r\n\t\t\"Number\": \"4K\",\r\n\t\t\"StreetName\": \"Gedempte Zalmhaven\",\r\n\t\t\"City\": {\r\n\t\t\t\"Country\": \"Netherlands\",\r\n\t\t\t\"Name\": \"Rotterdam\"\r\n\t\t}\r\n\t},\r\n\t\"Hobbies\": [\"Fishing\", \"Rowing\"]\r\n}")]
        public void WhenWrongObjectThenJsonException(string pInput)
        {
            // Arrange
            var input = new MemoryStream(Encoding.ASCII.GetBytes(pInput));
            var expectedResult = ErrorMessages.WrongInput;

            // Act - Assert
            var result = Assert.Throws<JsonException>(() => AttributeFirstJsonTransformer.Transform(input));

            // Assert
            Assert.That(result.Message, Is.EqualTo(expectedResult));
        }
    }
}
