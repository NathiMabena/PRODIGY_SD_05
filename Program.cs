using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace WebScraper
{
    class Program
    {
        public static void Main(string[] args)
        {
            string url = "https://www.amazon.com/BENGOO-G9000-Controller-Cancelling-Headphones/dp/B01H6GUCCQ/ref=sr_1_2?_encoding=UTF8&content-id=amzn1.sym.12129333-2117-4490-9c17-6d31baf0582a&dib=eyJ2IjoiMSJ9.R4qU6QQfQJhd_X1kItf0hJTRsi9JFB2_cKbTfuAAuOJAenQ5HJykCKNBvLZPjyZtGqaoLI9om5xbQCwcC8TNRkGoA_QcxN_9bZUGKwGsLL5ps7n0RAgzfMkwrEWG1PWU0IRHIR0ipKsgLGglrZ886kZW1F_xroHkOxXeG6GSy9_3AlKLQKKHaE_0koI9C8hBZ0-_RohP6wxlcfW_Vxi4a0iEkSVMu9VX-E6Uoxa9uUo.-CrZlZ1W0BKulZK-JZMV5hnR6klthg2RTum_NwqDJRs&dib_tag=se&keywords=gaming+headsets&pd_rd_r=e84a76d3-c58d-41c1-beab-2a167c45f9c8&pd_rd_w=dLavc&pd_rd_wg=03aMn&pf_rd_p=12129333-2117-4490-9c17-6d31baf0582a&pf_rd_r=JBJRAFM14E74Y3YGZQR9&qid=1727829627&sr=8-2";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            List<Product> products = new List<Product>();

            
            foreach (var productNode in doc.DocumentNode.SelectNodes("//div[@id='centerCol']"))
            {
                var name = productNode.SelectSingleNode(".//span[@id='productTitle']")?.InnerText.Trim() ?? "No Name";
                var price = productNode.SelectSingleNode(".//span[@class='a-price-whole']")?.InnerText.Trim() ?? "No Price";
                var ratingNode = productNode.SelectSingleNode(".//i[contains(@class, 'a-icon-star')]");
                var rating = ratingNode != null ? ratingNode.InnerText.Trim() : "No Rating";

                // Add each product to the list
                products.Add(new Product
                {
                    Name = name,
                    Price = price,
                    Rating = rating
                });
            }

            //  CSV 
            using (var writer = new StreamWriter("products.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(products);
            }

            Console.WriteLine("Scraping is complete. Data saved to products.csv");
        }
    }

    //  Product class
    public class Product
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Rating { get; set; }
    }
}