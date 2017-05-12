using HtmlAgilityPack;

namespace STSCommon.Extensions
{
    public static class HtmlDocumentExtensions
    {
        public static HtmlDocument LoadFromPath(this HtmlDocument htmlDocument, string path)
        {
            htmlDocument.Load(path);
            return htmlDocument;
        }
    }
}