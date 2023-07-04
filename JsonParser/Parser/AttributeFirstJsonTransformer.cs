using JsonParser.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsonParser.Parser
{
    public static class AttributeFirstJsonTransformer
    {
        public static Stream Transform(Stream source)
        {
            var output = new MemoryStream();
            try
            {
                using (var reader = new StreamReader(source))
                using (var writer = new StreamWriter(output, reader.CurrentEncoding, 1024, true))
                using (var jsonReader = new JsonTextReader(new StreamReader(source)))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    if (!IsJsonValid(reader.ReadToEnd()))
                        throw new JsonException();

                    source.Seek(0, SeekOrigin.Begin);   

                    var jToken = JToken.Load(jsonReader);
                    ReorderObjectFields(jToken);

                    jsonWriter.Formatting = Formatting.Indented;
                    jToken.WriteTo(jsonWriter);
                }
                output.Seek(0, SeekOrigin.Begin);
            }
            catch (JsonException)
            {
                throw new JsonException(ErrorMessages.WrongInput);
            }
            return output;
        }

        private static void ReorderObjectFields(JToken parentNode)
        {
            foreach (var node in parentNode.Children<JProperty>())
            {
                if (IsObject(node.Value.Type))
                    ReorderObjectFields((JObject)node.Value);
                else if (IsArray(node.Value.Type))
                    ReorderObjectFields((JArray)node.Value);
            }

            if (IsObject(parentNode.Type))
            {
                List<JProperty> nodes = parentNode.Children<JProperty>()
                                      .Where(p => p.Value.Type != JTokenType.Object && p.Value.Type != JTokenType.Array)
                                      .ToList();
                AddNodes(nodes);
            }
            else if (IsArray(parentNode.Type))
            {
                List<JToken> nodes = parentNode.Children()
                            .Where(t => t.Type != JTokenType.Object && t.Type != JTokenType.Array)
                            .ToList();

                AddNodes(nodes);
            }

            void AddNodes(dynamic nodes)
            {
                foreach (var node in nodes)
                {
                    node.Remove(); // Removes this token from its parent.
                    ((JContainer)parentNode).AddFirst(node);
                }
            }
        }
        private static bool IsJsonValid(string inputJson) => (inputJson.StartsWith('{') || inputJson.StartsWith('[')) && (inputJson.EndsWith(']') || inputJson.EndsWith('}'));
        private static bool IsObject(JTokenType jTokenType) => jTokenType == JTokenType.Object;
        private static bool IsArray(JTokenType jTokenType) => jTokenType == JTokenType.Array;
    }
}
