using Magic.BrowserAutomationNET;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace Magic.MarketplaceNET
{
    public class Shopee
    {

        public enum PaymentSource
        {

            Balance,
            ShopeePay

        } // end of enum

        public enum LoggedInStatus
        {

            LoggedIn,
            NotLoggedIn

        } // end of enum

        public enum IncreaseBudgetResult
        {

            Success,
            NoPaymentSourceSelected,
            BalanceNotEnough,
            SuccessUsingShopeePay,
            SuccessUsingBalance,
            WrongPinShopeePay,
            WrongPinSeller,
            ErrorSomehow

        } // end of enum


        public Chrome Chrome { get; }
        public string PIN { get; set; }
        private CancellationToken CancellationToken { get; }
        private int Timeout { get; }

        List<PaymentSource> PaymentSourcesSelected { get; set; } = new List<PaymentSource>();
        List<PaymentSource> PaymentSourcesAvailable { get; set; } = new List<PaymentSource>();

        private static readonly Random _rand = new Random();

        public Shopee(Chrome chrome, bool useBalance = true, bool useShopeePay = true, string pin = "0", int timeout = 10, CancellationToken cancellationToken = default)
        {

            Chrome = chrome;
            PIN = pin;
            Timeout = timeout;
            CancellationToken = cancellationToken;

            if(useBalance)
            {
                PaymentSourcesSelected.Add(PaymentSource.Balance);
            }

            if(useShopeePay)
            {
                PaymentSourcesSelected.Add(PaymentSource.ShopeePay);
            }

        } // end of method

        public LoggedInStatus CheckLoggedIn()
        {

            WebPage webPage = Chrome.GetCurrentUrl();

            if (!webPage.Url.Contains("https://seller.shopee.co.id/portal/marketing/pas/top-up"))
            {
                Chrome.Navigate("https://seller.shopee.co.id/portal/marketing/pas/top-up");
            }

            BrowserAutomationNET.WebElement isiSaldoAtauLogin = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@class")},'content-box')]//div[contains({Chrome.ToLower("@class")},'breadcrumb')]//a[{Chrome.ToLower("normalize-space(.)")}='isi saldo' and not(ancestor::div[contains({Chrome.ToLower("@class")},'phantom')])]|//form//button[{Chrome.ToLower("normalize-space(.)")}='log in']", Timeout);

            if (isiSaldoAtauLogin.Item == null) return LoggedInStatus.NotLoggedIn;

            string tag = isiSaldoAtauLogin.Item.TagName.ToLower();

            if (tag == "a")
            {
                return LoggedInStatus.LoggedIn;
            }
            else
            {
                return LoggedInStatus.NotLoggedIn;
            }

        } // end of method

        public IncreaseBudgetResult IncreaseBudget(int budget)
        {

            if(PaymentSourcesSelected.Count == 0)
            {
                return IncreaseBudgetResult.NoPaymentSourceSelected;
            }

            WebPage webPage = Chrome.GetCurrentUrl();

            if (!webPage.Url.Contains("https://seller.shopee.co.id/portal/marketing/pas/top-up"))
            {
                Chrome.Navigate("https://seller.shopee.co.id/portal/marketing/pas/top-up");
            }

            // HALAMAN INPUT NILAI

            BrowserAutomationNET.WebElement masukkanJumlahSaldoLainnyaButton = Chrome.FindElementByXPath($"//button[.//span[contains({Chrome.ToLower("normalize-space(.)")},'masukkan jumlah isi ulang saldo lainnya')]]", Timeout);
            
            CancellationToken.ThrowIfCancellationRequested();
            SafeClickResult safeClickResult = masukkanJumlahSaldoLainnyaButton.SafeClick();

            BrowserAutomationNET.WebElement masukkanJumlahSaldoLainnyaInput = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("normalize-space(.)")}='atau masukkan jumlah isi ulang saldo lainnya']/ancestor::div[contains(@class,'input-wrapper')][1]//input", Timeout);
            
            CancellationToken.ThrowIfCancellationRequested();
            safeClickResult = masukkanJumlahSaldoLainnyaInput.SafeClick();

            masukkanJumlahSaldoLainnyaInput.SafeSendKeys(OpenQA.Selenium.Keys.Control + "a");
            masukkanJumlahSaldoLainnyaInput.SafeSendKeys(OpenQA.Selenium.Keys.Delete);

            SafeSendKeysResult safeSendKeysResult = masukkanJumlahSaldoLainnyaInput.SafeCopyAndPaste(budget.ToString());

            WebDriverWait wait = new WebDriverWait(Chrome.Driver, TimeSpan.FromSeconds(Timeout));

            wait.Until(d =>
            {
                string price = d.FindElement(By.XPath("//div[@data-testid='topup-product-price']")).Text.Trim();
                return price == $"Rp{budget.ToString()}";
            });
            
            BrowserAutomationNET.WebElement checkoutButton = Chrome.FindElementByXPath($"//button[@data-testid='topup-checkout' and .//span[{Chrome.ToLower("normalize-space(.)")}='checkout']]", Timeout);
            
            CancellationToken.ThrowIfCancellationRequested();
            safeClickResult = checkoutButton.SafeClick();

            // HALAMAN PILIH PAYMENT SOURCE

            BrowserAutomationNET.WebElement saldoPenjualOption = Chrome.FindElementByXPath($"//div[contains(@class,'payment-channel-list-item') and contains({Chrome.ToLower("normalize-space(.)")},'saldo penjual')]", Timeout);

            if(saldoPenjualOption.Item != null && !saldoPenjualOption.Item.GetAttribute("class").Contains("disabled"))
            {
                PaymentSourcesAvailable.Add(PaymentSource.Balance);
            }

            BrowserAutomationNET.WebElement shopeePayOption = Chrome.FindElementByXPath($"//div[contains(@class,'payment-channel-list-item') and contains({Chrome.ToLower("normalize-space(.)")},'saldo shopeepay')]", Timeout);

            if (shopeePayOption.Item != null && !shopeePayOption.Item.GetAttribute("class").Contains("disabled"))
            {
                PaymentSourcesAvailable.Add(PaymentSource.ShopeePay);
            }

            PaymentSource? paymentSource = PickPaymentSource();

            if(paymentSource == null)
            {
                return IncreaseBudgetResult.BalanceNotEnough;
            }

            CancellationToken.ThrowIfCancellationRequested();

            // HALAMAN KONFIRMASI

            switch(paymentSource)
            {
                case PaymentSource.Balance:
                    safeClickResult = saldoPenjualOption.SafeClick();
                    break;
                case PaymentSource.ShopeePay:
                    safeClickResult = shopeePayOption.SafeClick();
                    break;
            }

            BrowserAutomationNET.WebElement konfirmasiButton = Chrome.FindElementByXPath($"//button[contains(@class,'footer-button') and .//span[{Chrome.ToLower("normalize-space(.)")}='konfirmasi']]", Timeout);

            CancellationToken.ThrowIfCancellationRequested();
            safeClickResult = konfirmasiButton.SafeClick();

            IncreaseBudgetResult increaseBudgetResult = IncreaseBudgetResult.Success;

            if (paymentSource == PaymentSource.Balance)
            {
                BrowserAutomationNET.WebElement payButton = Chrome.FindElementByXPath($"//div[contains(@class,'payment-safe-page')]//button[contains({Chrome.ToLower("normalize-space(.)")},'bayar')]", Timeout * 4);

                CancellationToken.ThrowIfCancellationRequested();
                safeClickResult = payButton.SafeClick();

                BrowserAutomationNET.WebElement pinInput = Chrome.FindElementByXPath($"//div[contains(@class,'shopee-modal')]//input[@type='number' and contains(@style,'opacity: 0')]", Timeout);

                CancellationToken.ThrowIfCancellationRequested();
                safeSendKeysResult = pinInput.SafeCopyAndPaste(PIN);

                BrowserAutomationNET.WebElement pinKonfirmasiButton = Chrome.FindElementByXPath($"//div[contains(@class,'shopee-modal')]//div[@class='txtNewline' and .//span[contains(@class,'orangeText') and {Chrome.ToLower("normalize-space(.)")}='konfirmasi']]", Timeout);

                CancellationToken.ThrowIfCancellationRequested();
                safeClickResult = pinKonfirmasiButton.SafeClick();

                BrowserAutomationNET.WebElement pembayaranBerhasilTitle = Chrome.FindElementByXPath($"//div[contains(@class,'txtNewline') and {Chrome.ToLower("normalize-space(.)")}='pembayaran berhasil']|//div[contains(@class, 'txtNewline') and contains({Chrome.ToLower("normalize-space(.)")}, 'pin saldo penjual salah')]", Timeout);

                if (pembayaranBerhasilTitle.Item != null && pembayaranBerhasilTitle.Item.Text.ToLower().Contains("berhasil"))
                {
                    increaseBudgetResult = IncreaseBudgetResult.SuccessUsingBalance;
                }
                else
                {
                    increaseBudgetResult = IncreaseBudgetResult.WrongPinSeller;
                }

                Chrome.Navigate("https://seller.shopee.co.id/portal/marketing/pas/top-up");
            }
            else if(paymentSource == PaymentSource.ShopeePay)
            {
                BrowserAutomationNET.WebElement pinInput = Chrome.FindElementByXPath($"//div[contains(@class,'title') and {Chrome.ToLower("normalize-space(.)")}='masukkan pin shopeepay']/ancestor::div[contains(@class,'container')][1]//div[contains(@class,'pinContainer')]//input[@inputmode='numeric' and contains(@class,'inputElement')][1]", Timeout * 2);

                CancellationToken.ThrowIfCancellationRequested();
                safeSendKeysResult = pinInput.SafeCopyAndPaste(PIN);

                BrowserAutomationNET.WebElement pinConfirmationButton = Chrome.FindElementByXPath($"//div[contains(@class,'title') and {Chrome.ToLower("normalize-space(.)")}='masukkan pin shopeepay']/ancestor::div[contains(@class,'container')][1]//div[contains(@class,'confirmButton') and {Chrome.ToLower("normalize-space(.)")}='verifikasi']", Timeout);

                CancellationToken.ThrowIfCancellationRequested();
                safeClickResult = pinConfirmationButton.SafeClick();

                BrowserAutomationNET.WebElement successTitle = Chrome.FindElementByXPath($"//h2[contains({Chrome.ToLower("@class")},'title') and {Chrome.ToLower("normalize-space(.)")}='pembayaran berhasil']|//div[contains(@class,'pinContainer')]//div[contains(@class,'errorPinMsg') and {Chrome.ToLower("normalize-space(.)")}='pin tidak valid, tersisa 8 percobaan']", Timeout * 2);


                if (successTitle.Item != null && successTitle.Item.TagName == "h2")
                {
                    increaseBudgetResult = IncreaseBudgetResult.SuccessUsingShopeePay;
                }
                else
                {
                    increaseBudgetResult = IncreaseBudgetResult.WrongPinShopeePay;
                }

                Chrome.Navigate("https://seller.shopee.co.id/portal/marketing/pas/top-up");
            }

            return increaseBudgetResult;

        } // end of method

        public PaymentSource? PickPaymentSource()
        {

            // ambil irisan
            List<PaymentSource> candidates = PaymentSourcesSelected
                .Intersect(PaymentSourcesAvailable)
                .ToList();

            // kalau tidak ada yang valid
            if (candidates.Count == 0)
                return null;

            int index = _rand.Next(candidates.Count);
            
            return candidates[index];

        } // end of method

    } // end of class

} // end of namespace
