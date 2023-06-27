using KW1C.WebScraping.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

// Maak een nieuwe instantie van de ChromeDriver aan en open de Google reviews van het KW1C.
var driver = new ChromeDriver();
driver.Navigate().GoToUrl("https://www.google.com/search?q=kw1c#lrd=0x47c6ee81fd0d7977:0x982c62ce9a48d49,1,,,,&rlimm=685327989514472777");

// Accepteer alle cookies.
var acceptAllButton = driver.FindElement(By.Id("L2AGLb"));
acceptAllButton.Click();

// Wacht tot de reviews zijn geladen.
Thread.Sleep(7500);

// Scroll 10x naar benden (om meer reviews te laden).
var js = (IJavaScriptExecutor)driver;

for (var i = 0; i < 10; i++)
{
    // Scroll 1000px naar beneden.
    js.ExecuteScript($"document.querySelector('.review-dialog-list').scrollTo(0, {i * 1000});");
    
    Thread.Sleep(750);
}

// Haal alle reviews op.
var reviews = new List<Review>();

foreach (var reviewElement in driver.FindElements(By.CssSelector(".gws-localreviews__google-review")).ToArray())
{
    reviews.Add(new Review()
    {
        Name = reviewElement.FindElement(By.ClassName("TSUbDb")).Text,
        Text = reviewElement.FindElement(By.ClassName("Jtu6Td")).Text
    });
}

// Sla de reviews op in een JSON bestand.
File.WriteAllText("reviews.json", JsonConvert.SerializeObject(reviews, Formatting.Indented));

// Sluit de browser.
driver.Close();
