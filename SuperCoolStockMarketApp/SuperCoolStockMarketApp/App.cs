using System;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SuperCoolStockMarketApp
{
    public class GoogleStockData
    {
        public double l_cur { get; set; }
    }

    public class App : Application
    {
        public App()
        {
            var priceLabel = new Label { HorizontalOptions = 
                LayoutOptions.CenterAndExpand };
            var stockEntry = new Entry
            {
                Placeholder = "GOOG, AAPL, etc",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var submitButton = new Button
            {
                Text = "Check now",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            submitButton.Clicked += async (sender, args) =>
            {
                HttpClient client = new HttpClient(new NativeMessageHandler());
                var httpRequest = new HttpRequestMessage(new HttpMethod("GET"),
                    string.Format("{0}{1}",
                        "http://www.google.com/finance/info?q=NASDAQ%3a",
                        stockEntry.Text));

                client.Timeout = TimeSpan.FromSeconds(30);

                var response = await client.SendAsync(httpRequest);
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GoogleStockData>(
                    jsonString.Replace("\n// ", "")
                        .Replace("[", "").Replace("]", ""));

                priceLabel.Text = string.Format("Current price for {0} is US$ {1}",
                    stockEntry.Text,
                    result.l_cur);
            };

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
					    new Label {
						    XAlign = TextAlignment.Center,
						    Text = "Check stock prices for:"
					    },
					    stockEntry,
					    submitButton,
					    priceLabel
				    }
                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
