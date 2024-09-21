using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Magic.BrowserAutomationNET;

namespace Magic.MarketplaceNET.Facebook
{
    public class SendOffer
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

            public static EventType TurningOffMessagePopup { get; } = new EventType(1, "Start Sending Offer");
            public static EventType StartSendingOffer { get; } = new EventType(1, "Start Sending Offer");
            public static EventType OfferMessageIsEmpty { get; } = new EventType(2, "Message untuk penawaran isinya kosong");
            public static EventType AccountCheckpoint { get; } = new EventType(3, "Akun checkpoint");
            public static EventType UnavailableProduct { get; } = new EventType(4, "Sudah membuka URL tawaran tapi tawaran tidak tersedia");
            public static EventType NoSendMessageButton { get; } = new EventType(5, "Tombol kirim pesan / kirim pesan lagi tidak ditemukan");
            public static EventType SendMessageButtonClickFailed { get; } = new EventType(6, "Gagal klik tombol kirim pesan");
            public static EventType PasteMessageFailed { get; } = new EventType(7, "Gagal paste text kirim pesan");
            public static EventType NoSendMessageAgainTextBox { get; } = new EventType(8, "Pesan text box tidak muncul untuk kirim pesan lagi.");
            public static EventType SendEnterButtonFailed { get; } = new EventType(9, "Gagal enter text kirim pesan");
            public static EventType SendOfferLimit { get; } = new EventType(10, "Akun boost mencapai limit untuk send offer");
            public static EventType SendOfferSuccess { get; } = new EventType(11, "Send first offer success");

        } // end of class

        public event Action<SendOfferEventArgs>? SendOfferEvent;

        public class SendOfferEventArgs
        {
            public EventType EventType { get; set; }
            public Chrome? Chrome { get; set; }
            public string ListingURL { get; set; }
            public long FacebookChatId { get; set; }

            public SendOfferEventArgs(EventType eventType, string listingUrl, Chrome chrome, long facebookChatId = 0)
            {

                this.EventType = eventType;
                this.ListingURL = listingUrl;
                this.Chrome = chrome;
                this.FacebookChatId = facebookChatId;

            } // end of constructor method

        } // end of class

        private string listingURL { get; set; }
        private Chrome Chrome { get; set; }
        private int Timeout { get; set; } = 10;
        public CancellationToken CancellationToken { get; set; }

        public SendOffer(string listingURL, Chrome chrome, int timeout, CancellationToken cancellationToken)
        {

            this.listingURL = listingURL;
            this.Chrome = chrome;
            this.Timeout = timeout;
            this.CancellationToken = cancellationToken;

        } // end of method

        public bool Start(string offerMessage)
        {

            if(String.IsNullOrEmpty(offerMessage))
            {
                SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.OfferMessageIsEmpty, listingURL, Chrome));
            }

            SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.TurningOffMessagePopup, listingURL, Chrome));

            WebElement messengerButton = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'messenger') and @role='button' and not(@aria-hidden='true')]", Timeout);
            messengerButton.SafeClick();

            WebElement optionButton = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='opsi' and @role='button']/ancestor::div[1]", Timeout);

            SafeClickResult optionButtonSafeClick = optionButton.SafeClick();

            if(optionButtonSafeClick.Status)
            {
                WebElement newMessagePopUpSwitch = Chrome.FindElementByXPath($"//input[contains({Chrome.ToLower("@aria-label")}, 'pop-up pesan') and @role='switch']");

                string newMessagePopUpSwitchSelected = newMessagePopUpSwitch.Item!.GetAttribute("checked");

                if (newMessagePopUpSwitchSelected == "true")
                {
                    newMessagePopUpSwitch.SafeClick();

                    for (int i = 0; i < Timeout; i++)
                    {
                        newMessagePopUpSwitch = Chrome.FindElementByXPath($"//input[contains({Chrome.ToLower("@aria-label")}, 'pop-up pesan') and @role='switch']", 1);
                        newMessagePopUpSwitchSelected = newMessagePopUpSwitch.Item!.GetAttribute("checked");

                        if (newMessagePopUpSwitchSelected == "false") break;
                    }
                }
            }

            SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.StartSendingOffer, listingURL, Chrome));

            Chrome.Navigate(listingURL);

            #region CHECK URL

            WebPage webPage = Chrome.GetCurrentUrl();

            if (webPage.Url.Contains("checkpoint"))
            {
                SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.AccountCheckpoint, listingURL, Chrome));
                return false;
            }
            else if (webPage.Url.Contains("unavailable_product=1"))
            {
                SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.UnavailableProduct, listingURL, Chrome));
                return false;
            }

            #endregion

            #region SEND OFFER MESSAGE

            WebElement sendMessageButtonChild = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='kirim pesan' and @role='button']|(//div[{Chrome.ToLower("@aria-label")}='kirim pesan lagi' and @role='button'])[2]", Timeout);

            //div[@aria-label='Kirim pesan' and @role='button']/ancestor::div[1]|(//div[@aria-label='Kirim Pesan Lagi' and @role='button'])[2]/ancestor::div[1]

            if (!sendMessageButtonChild.State)
            {
                SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.NoSendMessageButton, listingURL, Chrome));
                return false;
            }

            WebElement chatSettings = new WebElement();
            SafeClickResult safeClickResult;

            string sendMessageButtonText = sendMessageButtonChild.Item!.GetAttribute("aria-label").ToLower();

            bool firstTimeOffer = sendMessageButtonText == "kirim pesan" ? true : false;

            WebElement sendMessageButton = sendMessageButtonChild.FindElementByXPath("ancestor::div[1]");

            Magic.HelperNET.PutContentToClipboard(offerMessage);

            if (firstTimeOffer)
            {
                safeClickResult = sendMessageButton.SafeClick();

                if (!safeClickResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendMessageButtonClickFailed, listingURL, Chrome));
                    return false;
                }

                // baca popup dialog kirim pesan
                WebElement textAreaInDialog = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'kirim pesan')]//textarea", Timeout);

                SafeSendKeysResult safeSendKeysResult = new SafeSendKeysResult();

                safeSendKeysResult = textAreaInDialog.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");

                if (!safeSendKeysResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.PasteMessageFailed, listingURL, Chrome));
                    return false;
                }

                WebElement kirimPesanButton = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'kirim pesan') and @role='dialog']//div[{Chrome.ToLower("@aria-label")}='kirim pesan' and @role='button']", Timeout);

                safeClickResult = kirimPesanButton.SafeClick();

                if (!safeClickResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendMessageButtonClickFailed, listingURL, Chrome));
                    return false;
                }

                chatSettings = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pengaturan obrolan' and @role='button']", Timeout);

                if (!chatSettings.State)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendOfferLimit, listingURL, Chrome));
                    return false;
                }
            }
            else
            {
                // jika sudah pernah nanya perdana "Apa ini masih ada". Jatuhnya "kirim pesan lagi."

                // sleep ini diperlukan karena adanya persiapan elemen meskipun sudah selesai loading, ada flickr gitu
                Thread.Sleep(6000);

                safeClickResult = sendMessageButton.SafeClick();

                if (!safeClickResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendMessageButtonClickFailed, listingURL, Chrome));
                    return false;
                }

                WebElement pesanTextBox = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pengaturan obrolan' and @role='button']/ancestor::div[@data-visualcompletion='ignore'][1]//div[{Chrome.ToLower("@aria-label")}='kirim pesan' and @role='textbox']", Timeout / 2);
                
                if (!pesanTextBox.State)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.NoSendMessageAgainTextBox, listingURL, Chrome));
                    return false;
                }

                SafeSendKeysResult safeSendKeysResult = pesanTextBox.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");

                if (!safeSendKeysResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.PasteMessageFailed, listingURL, Chrome));
                    return false;
                }

                safeSendKeysResult = pesanTextBox.SafeSendKeys(OpenQA.Selenium.Keys.Enter);

                if (!safeSendKeysResult.Status)
                {
                    SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendEnterButtonFailed, listingURL, Chrome));
                    return false;
                }

                Thread.Sleep(3000);

                chatSettings = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pengaturan obrolan' and @role='button']", Timeout);
            }

            #endregion

            #region WRAP UP

            safeClickResult = chatSettings.SafeClick();

            WebElement openInMessengerButton = Chrome.FindElementByXPath($"//a[@role='menuitem'][.//span[contains({Chrome.ToLower("text()")}, 'buka di messenger')]]", Timeout);

            string chatUrl = openInMessengerButton.Item!.GetAttribute("href");
            string pattern = @"/messages/t/(\d+)/";

            Match match = Regex.Match(chatUrl, pattern);
            long facebookChatId = Convert.ToInt64(match.Groups[1].Value);

            Debug.WriteLine("ID chat adalah : " + facebookChatId);


            SendOfferEvent?.Invoke(new SendOfferEventArgs(EventType.SendOfferSuccess, listingURL, Chrome, facebookChatId));

            #endregion

            return true;

        } // end of method

    } // end of class
} // end of namespace
