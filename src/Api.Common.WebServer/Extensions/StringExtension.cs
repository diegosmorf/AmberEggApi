using Newtonsoft.Json.Linq;

namespace Api.Common.WebServer.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();

            if (text.StartsWith("{") && text.EndsWith("}") || //For object
                text.StartsWith("[") && text.EndsWith("]")) //For array
                try
                {
                    JToken.Parse(text);
                    return true;
                }
                catch
                {
                    return false;
                }

            return false;
        }
    }
}