using System.IO;

namespace JsonParser.Parser
{
    public static class AttributeFirstJsonTransformer
    {
        public static Stream Transform(Stream source)
        {
            return source;
        }
    }
}