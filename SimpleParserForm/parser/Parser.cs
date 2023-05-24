using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SimpleParserForm.parser
{
    public class Parser
    {
        private readonly string _url;

        public Parser(string url)
        {
            _url = url;
        }

        public IEnumerable<string> ParseText()
        {
            var html = GetHtml();
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);

            var body = htmlSnippet.DocumentNode.SelectSingleNode("//body");
            if (body == null)
            {
                return new List<string> { "Сайт отправил HTML документ с ошибкой, в нем нет <body>" };
            }

            var stack = new Stack<HtmlNode>(body.ChildNodes.Reverse());
            var result = new HashSet<string>();

            while (stack.Count != 0)
            {
                var node = stack.Pop();
                if (node.Name == "script" || node.Name == "noindex") continue;

                var text = node.InnerText.Trim();
                
                text = Regex.Replace(text, @"\s+", " "); 
                text = Regex.Replace(text, @"\r+", "\r");
                text = Regex.Replace(text, @"\n+", "\n");

                if (text != "") result.Add(text);

                var nodes = node.ChildNodes.Reverse();

                foreach (var childNode in nodes)
                {
                    stack.Push(childNode);
                }
            }

            return result;
        }

        private string GetHtml()
        {
            string html;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            using(var client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                html = client.DownloadString(_url);
            }
            return html;
        }
    }
}