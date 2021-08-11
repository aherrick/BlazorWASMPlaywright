using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace BlazorWASMPlaywright.Tests
{
    // https://www.meziantou.net/automated-ui-tests-an-asp-net-core-application-with-playwright-and-xunit.htm

    public class BlazorWASMPlaywrightTests : IClassFixture<BlazorWebAssemblyWebHostFixture<Program>>
    {
        private readonly BlazorWebAssemblyWebHostFixture<Program> _server;

        public BlazorWASMPlaywrightTests(BlazorWebAssemblyWebHostFixture<Program> server) => _server = server;

        [Fact]
        public async Task DisplayHomePage()
        {
            var browser = await GetBrowser();
            var page = await browser.NewPageAsync();

            await page.GotoAsync(_server.RootUri.AbsoluteUri);

            var header = await page.WaitForSelectorAsync("h1");
            Assert.Equal("Hello, world!", await header.InnerTextAsync());

            await browser.CloseAsync();
        }

        [Fact]
        public async Task Counter()
        {
            var browser = await GetBrowser();
            var page = await browser.NewPageAsync();

            await page.GotoAsync(_server.RootUri + "counter", new PageGotoOptions() { WaitUntil = WaitUntilState.NetworkIdle });
            await page.ClickAsync("#IncrementBtn");

            // Selectors are not only CSS selectors. You can use xpath, css, or text selectors
            // By default there is a timeout of 30s. If the selector isn't found after the timeout, an exception is thrown.
            // More about selectors: https://playwright.dev/#version=v1.4.2&path=docs%2Fselectors.md
            await page.WaitForSelectorAsync("text=Current count: 1");

            await browser.CloseAsync();
        }

        public static async Task<IBrowser> GetBrowser()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false,
                SlowMo = 5000
            });

            return browser;
        }
    }
}