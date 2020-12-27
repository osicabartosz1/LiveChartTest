using System.IO;
using System.Text.RegularExpressions;

namespace LiveChartTest
{
    class Import
    {
        public string Path;
        public string text;

        public Import(string path)
        {
            Path = path;
            text = File.ReadAllText(Path);
        }

        private void ReplaceTag(string name, string replateText)
        {
            var patern = new Regex("<[^>]*" + name + "[^>]*>[^>]*" + "</[^>]*" + name + "[^>]*>");
            text = patern.Replace(text, replateText);
        }
        private void ReplaceTagName(string name, string replateText)
        {
            var patern = new Regex("</?[^>]*" + name + "[^>]*>");
            text = patern.Replace(text, replateText);
        }

        private void ReplaceEndTagName(string name, string replateText)
        {
            var patern = new Regex("</[^>]*" + name + "[^>]*>");
            text = patern.Replace(text, replateText);
        }
        private void ReplaceBeginTagName(string name, string replateText)
        {
            var patern = new Regex("<" + name + "[^>]*>");
            text = patern.Replace(text, replateText);
        }
        private void Replace(string patern, string replateText)
        {
            var Patern = new Regex(patern);
            text = Patern.Replace(text, replateText);
        }

        public void PrepareText()
        {
            ReplaceTag("style", "");
            ReplaceEndTagName("th", "@");
            ReplaceEndTagName("tr", ";");
            ReplaceEndTagName("td", "@");
            ReplaceBeginTagName("tr", "");
            ReplaceBeginTagName("th", "");
            ReplaceBeginTagName("br", "");
            ReplaceBeginTagName("td", "");
            ReplaceTagName("table", "");
            Replace("@;", ";");
        }

    }
}
