using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace Wikisplorer
{
    public class WWScraper
    {
        private List<Article> articlesList;

        public List<Article> ArticlesList
        {
            get { return articlesList; }
        }
        public WWScraper()
        {
            Console.WriteLine("WWScraper created");
            // list for storing scraped articles
            articlesList = new List<Article>();
        }

        // Add article to WWScraper object
        public bool AddArticle(String link)
        {
            try
            {
                // Create new article object and add it to list
                Article article = new Article(link);
                articlesList.Add(article);

                return true;
            }
            catch (Exception )
            {
                // article does not exist
                return false;
            }
        }

        // Print out string representation of article list
        public override string ToString()
        {
            int num = 0;
            StringBuilder returnStr = new StringBuilder();
            foreach (var ar in articlesList)
            {
                if (num == 0)
                {
                    returnStr.Append("\n");
                }

                num += 1;
                returnStr.Append($"[{num}] {ar}\n");
            }

            return returnStr.ToString();
        }
    }
}
