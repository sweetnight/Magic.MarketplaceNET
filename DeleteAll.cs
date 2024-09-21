using Magic.BrowserAutomationNET;
using OpenQA.Selenium;
using System.Threading;

namespace Magic.MarketplaceNET.Facebook
{
    public class DeleteAll
    {

        public class EventType
        {

            public int Code { get; }
            public string Message { get; }

            private EventType(int code, string message)
            {

                Code = code;
                Message = message;
            }

            public static EventType StartDeleting { get; } = new EventType(1, "Delete post dimulai");
            public static EventType AccountCheckpoint { get; } = new EventType(2, "Akun checkpoint");
            public static EventType AListingDeleted { get; } = new EventType(3, "Sebuah listing telah dihapus");

        } // end of method

        public event Action<DeleteAllEventArgs>? DeleteAllEvent;

        public class DeleteAllEventArgs
        {

            public EventType EventType { get; set; }
            public int DeletedNumber { get; set; }

            public DeleteAllEventArgs(EventType eventType, int deletedNumber)
            {

                EventType = eventType;
                this.DeletedNumber = deletedNumber;

            } // end of constructor method

        } // end of method

        public Chrome Chrome { get; set; }
        public int Timeout { get; set; }

        public DeleteAll(Chrome chrome, int timeout)
        {

            Chrome = chrome;
            Timeout = timeout;

        } // end of method

        public int Start()
        {

            DeleteAllEvent?.Invoke(new DeleteAllEventArgs(EventType.StartDeleting, 0));

            Chrome.Navigate("https://www.facebook.com/marketplace/you/selling");

            BrowserAutomationNET.WebPage checkpointWebPage = Chrome.GetCurrentUrl();

            if (checkpointWebPage.Url.Contains("checkpoint"))
            {
                DeleteAllEvent?.Invoke(new DeleteAllEventArgs(EventType.AccountCheckpoint, 0));
                return 0;
            }

            // ini diperlukan jika listing pertama tidak bisa didelete, jadi pilih listing kedua.
            // jadi listing kedua disebut first listing (indexnya diincrement jadi 2).
            int firstListingIndex = 1;
            int listingDeleted = 0;

            while (true)
            {
                Magic.BrowserAutomationNET.WebElement firstThreeDotSymbol = Chrome.FindElementByXPath($@"(//div[contains({Chrome.ToLower("@aria-label")}, 'opsi lainnya untuk') and @role='button'])[{firstListingIndex}]");

                if(!firstThreeDotSymbol.State)
                {
                    break;
                }

                SafeClickResult safeClickResult = firstThreeDotSymbol.SafeClick(5);

                if (!safeClickResult.Status)
                {
                    firstListingIndex++;
                    continue;
                }

                Thread.Sleep(1000);

                Magic.BrowserAutomationNET.WebElement hapusTawaranButton = Chrome.FindElementByXPath($@"//span[contains({Chrome.ToLower("text()")}, 'hapus tawaran')]");
                safeClickResult = hapusTawaranButton.SafeClick(5);

                Thread.Sleep(2000);

                //Magic.BrowserAutomation.WebElement hapusButton = chrome.FindElementByXPath("(//div[@aria-label='Hapus Tawaran']//div[@aria-label='Hapus']//span[text()='Hapus'])[2]");

                // konfirmasi dialog
                Magic.BrowserAutomationNET.WebElement hapusButton = Chrome.FindElementByXPath($@"(//div[{Chrome.ToLower("@aria-label")}='hapus tawaran']//div[{Chrome.ToLower("@aria-label")}='hapus']//span[{Chrome.ToLower("text()")}='hapus'])[2]|//div[{Chrome.ToLower("@aria-label")}='hapus']//span[{Chrome.ToLower("text()")}='hapus']");

                Magic.BrowserAutomationNET.WebElement hapusButtonConfirmed = new Magic.BrowserAutomationNET.WebElement();

                if (hapusButton.State)
                {
                    Magic.BrowserAutomationNET.WebElement hapusButtonConfirmed1 = Chrome.FindElementByXPath($@"(//div[{Chrome.ToLower("@aria-label")}='hapus tawaran']//div[{Chrome.ToLower("@aria-label")}='hapus']//span[{Chrome.ToLower("text()")}='hapus'])[2]", 1);

                    if (hapusButtonConfirmed1.State)
                    {
                        hapusButtonConfirmed = hapusButtonConfirmed1;
                    }
                    else
                    {
                        hapusButtonConfirmed = Chrome.FindElementByXPath($@"//div[{Chrome.ToLower("@aria-label")}='hapus']//span[{Chrome.ToLower("text()")}='hapus']", 1);
                    }
                }

                safeClickResult = hapusButtonConfirmed.SafeClick(5);

                // pertanyaan survey kenapa dihapus
                Magic.BrowserAutomationNET.WebElement hapusTawaranDialogCloseButton = Chrome.FindElementByXPath($@"//div[{Chrome.ToLower("@aria-label")}='hapus tawaran']//div[{Chrome.ToLower("@aria-label")}='tutup']/i");

                if (hapusTawaranDialogCloseButton.State)
                {
                    hapusTawaranDialogCloseButton.SafeClick(5);
                    Thread.Sleep(2000);
                }

                bool cantDelete = false;
                Magic.BrowserAutomationNET.WebElement tidakBisaMenghapusTawaranTutup;

                // wait element disappear
                for (int i = 0; i < Timeout; i++)
                {
                    try
                    {
                        bool _ = firstThreeDotSymbol.Item!.Enabled;

                        tidakBisaMenghapusTawaranTutup = Chrome.FindElementByXPath($@"//span[{Chrome.ToLower("text()")}='tidak bisa menghapus postingan.']/../../../../..//span[{Chrome.ToLower("text()")}='tutup']", 1);

                        if(tidakBisaMenghapusTawaranTutup.State)
                        {
                            Thread.Sleep(1000);

                            tidakBisaMenghapusTawaranTutup.SafeClick(5);
                            cantDelete = true;
                            firstListingIndex++;

                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }

                    // tidak perlu sleep untuk iterasi, karena sudah menunggu timeout 1 detik untuk elemen tidakBisaMenghapusTawaranTutup
                    //Thread.Sleep(1000);
                }

                if (cantDelete)
                {
                    continue;
                }

                listingDeleted++;
                DeleteAllEvent?.Invoke(new DeleteAllEventArgs(EventType.AListingDeleted, listingDeleted));

                Thread.Sleep(1000);

            } // end of while

            //listingDeleted = listingDeleted - 1;

            return listingDeleted;

        } // end of method

    } // end of class
} // end of namespace
