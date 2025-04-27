using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace Wikisplorer.Models
{
    public class WWScraper
    {
        private HashSet<Article> articlesSet;

        public HashSet<Article> ArticlesSet
        {
            get { return articlesSet; }
        }
        public WWScraper()
        {
            Console.WriteLine("WWScraper created");
            // list for storing scraped articles
            articlesSet = new HashSet<Article>();
        }

        // Add article to WWScraper object
        public bool AddArticle(string link)
        {
            Article article = new Article(link);
            bool isAdded = articlesSet.Add(article);
            return isAdded;
        }

        // Print out string representation of article list
        public override string ToString()
        {
            int num = 0;
            StringBuilder returnStr = new StringBuilder();
            foreach (var article in articlesSet)
            {
                if (num == 0)
                {
                    returnStr.Append("\n");
                }

                num += 1;
                returnStr.Append($"[{num}] {article}\n");
            }

            return returnStr.ToString();
        }
    }
}
