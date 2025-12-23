using Magic.BrowserAutomationNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magic.MarketplaceNET.Facebook.SendOffer;

namespace Magic.MarketplaceNET.Facebook
{

    public class Chat
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

            public static EventType StartChat { get; } = new EventType(1, "Start Chat");
            public static EventType ChatMessageIsEmpty { get; } = new EventType(2, "Message kosong");
            public static EventType NoMessageTextBox { get; } = new EventType(3, "Textbox untuk isi pesan tidak tersedia");
            public static EventType PasteMessageFailed { get; } = new EventType(4, "Gagal paste text kirim pesan");
            public static EventType SendEnterButtonFailed { get; } = new EventType(5, "Gagal enter text kirim pesan");
            public static EventType ChatSuccess { get; } = new EventType(6, "Chat success");
            public static EventType ReviewSuccess { get; } = new EventType(7, "Review success");

        } // end of class

        public event Action<ChatEventArgs>? ChatEvent;

        public class ChatEventArgs
        {
            public EventType EventType { get; set; }
            public long FacebookChatId { get; set; }
            public Chrome? Chrome { get; set; }

            public ChatEventArgs(EventType eventType, Chrome chrome)
            {

                this.EventType = eventType;
                this.Chrome = chrome;

            } // end of constructor method

            public ChatEventArgs(EventType eventType, long facebookChatId, Chrome chrome)
            {

                this.EventType = eventType;
                this.FacebookChatId = facebookChatId;
                this.Chrome = chrome;

            } // end of constructor method

        } // end of class

        private long FacebookChatId { get; set; }
        private Chrome Chrome { get; set; }
        private int Timeout { get; set; } = 10;
        public CancellationToken CancellationToken { get; set; }

        public Chat(Chrome chrome, int timeout)
        {

            Chrome = chrome;
            Timeout = timeout;

        } // end of method

        public Chat(long facebookChatId, Chrome chrome, int timeout, CancellationToken cancellationToken)
        {

            this.FacebookChatId = facebookChatId;
            this.Chrome = chrome;
            this.Timeout = timeout;
            this.CancellationToken = cancellationToken;

        } // end of method

        public bool Start(string chatMessage, bool newOpenChat = true)
        {

            ChatEvent?.Invoke(new ChatEventArgs(EventType.StartChat, FacebookChatId, Chrome));

            if(string.IsNullOrEmpty(chatMessage))
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.ChatMessageIsEmpty, FacebookChatId, Chrome));
            }

            if(newOpenChat)
            {
                Chrome.Navigate($"https://www.facebook.com/messages/t/{this.FacebookChatId}/");
            }

            WebElement messageTextBox = new WebElement();
            
            for(int i = 0; i < 2; i++)
            {
                //messageTextBox = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pesan' and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']", Timeout);
                messageTextBox = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'tulis ke') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[contains({Chrome.ToLower("@aria-label")}, 'pesan') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[{Chrome.ToLower("@aria-label")}='terima' and @role='button'][.//span[{Chrome.ToLower("normalize-space(.)")}='terima']]", Timeout);

                if (!messageTextBox.State)
                {
                    Chrome.Refresh();
                    continue;
                }
            }

            if (!messageTextBox.State)
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.NoMessageTextBox, FacebookChatId, Chrome));
                return false;
            }

            // digunakan untuk menunggu flickr loading halaman
            if(newOpenChat)
            {
                // disini sudah loading sepenuhnya
                // mesti cek jendela pin/kode sinkronisasi, kasih waktu 5 detik saja

                WebElement pinRequestDialogCloseButton = Chrome.FindElementByXPath($"//div[@role='dialog' and .//span[contains({Chrome.ToLower("text()")}, 'masukkan pin anda')]]//div[{Chrome.ToLower("@aria-label")}='tutup' and @role='button']", 5);

                if (pinRequestDialogCloseButton.State)
                {
                    pinRequestDialogCloseButton.SafeClick();

                    WebElement confirmationDialog = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='jangan memulihkan pesan' and @role='button' and .//span[{Chrome.ToLower("text()")}='jangan memulihkan pesan'] and not(ancestor::div[@aria-hidden='true'])]", 5);
                    confirmationDialog.SafeClick();
                }

                string messageTextBoxAriaLabel = messageTextBox.SafeGetAttribute("aria-label")!;
                
                if(messageTextBoxAriaLabel.ToLower() == "terima")
                {
                    messageTextBox.SafeClick();

                    messageTextBox = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'tulis ke') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[contains({Chrome.ToLower("@aria-label")}, 'pesan') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[{Chrome.ToLower("@aria-label")}='terima' and @role='button'][.//span[{Chrome.ToLower("normalize-space(.)")}='terima']]", Timeout);
    
                    Thread.Sleep(3000);
                }

                messageTextBox = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'tulis ke') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[contains({Chrome.ToLower("@aria-label")}, 'pesan') and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']|//div[{Chrome.ToLower("@aria-label")}='terima' and @role='button'][.//span[{Chrome.ToLower("normalize-space(.)")}='terima']]", Timeout);
            }

            Magic.HelperNET.PutContentToClipboard(chatMessage);

            SafeSendKeysResult safeSendKeysResult = messageTextBox.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");

            if (!safeSendKeysResult.Status)
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.PasteMessageFailed, FacebookChatId, Chrome));
                return false;
            }

            safeSendKeysResult = messageTextBox.SafeSendKeys(OpenQA.Selenium.Keys.Enter);

            if (!safeSendKeysResult.Status)
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.SendEnterButtonFailed, FacebookChatId, Chrome));
                return false;
            }

            // sebelum chrome ditutup (versi magic < 8.5.0) supaya terlihat dulu hasil chat nya
            //Thread.Sleep(1000);

            ChatEvent?.Invoke(new ChatEventArgs(EventType.ChatSuccess, FacebookChatId, Chrome));

            return true;

        } // end of method

        public void Review(string PostedItemListingURL)
        {

            // url utk review, angka tersebut adalah ID jualannya
            // https://www.facebook.com/marketplace/you/rate/3964658137115944/

            Post post = new Post();
            long listingID = post.ExtractListingIDFromHref(PostedItemListingURL);
            string listingLink = "https://www.facebook.com/marketplace/you/rate/" + listingID + "/";

            Chrome.Navigate(listingLink);

            // ini bintang 5. jangan lupa pakai Chrome.ToLower()
            //div[contains(@aria-label, 'pilih peringkat') and @role = 'radiogroup']//input[@type = 'radio' and contains(@aria-label, '5 dari 5 peringkat')]

            WebElement fifthStar = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower("@aria-label")}, 'pilih peringkat') and @role = 'radiogroup']//input[@type = 'radio' and contains({Chrome.ToLower("@aria-label")}, '5 dari 5 peringkat')]", Timeout);

            SafeClickResult safeClickResult = fifthStar.SafeClick();

            // ini tombol kirim
            //div[@role='dialog' and contains(., 'Beri Peringkat')]//div[@role='button' and @aria-label='Kirim']

            WebElement sendButton = Chrome.FindElementByXPath($"//div[@role='dialog' and contains({Chrome.ToLower(".")}, 'beri peringkat')]//div[@role='button' and {Chrome.ToLower("@aria-label")}='kirim']", Timeout);

            safeClickResult = sendButton.SafeClick();

            // ini setelah bintang dikirim, jangan lupa pakai Chrome.ToLower()
            //div[contains(@aria-label, 'pesan dalam percakapan')]//span[not(.//span) and contains(., 'penilaian anda dikirim')]

            //WebElement reviewSent = Chrome.FindElementByXPath($"//div[contains({Chrome.ToLower(".")}, 'pesan dalam percakapan')]//span[not(.//span) and contains({Chrome.ToLower(".")}, 'penilaian anda dikirim')]", Timeout);

            WebElement pesanTextBox = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pengaturan obrolan' and @role='button']/ancestor::div[@data-visualcompletion='ignore'][1]//div[{Chrome.ToLower("@aria-label")}='kirim pesan' and @role='textbox']|//div[{Chrome.ToLower("@aria-label")}='pengaturan obrolan' and @role='button']/ancestor::div[@data-visualcompletion='ignore'][1]//div[{Chrome.ToLower("@aria-label")}='pesan' and @role='textbox']", Timeout / 2);

            if(pesanTextBox.State)
            {
                Debug.WriteLine("Chat ======================== : berhasil kirim bintang");
            }
            else
            {
                Debug.WriteLine("Chat ======================== : berhasil kirim bintang dengan catatan, pesanTextBox tidak berhasil dipilih.");
            }

        } // end of method

    } // end of class

} // end of namespace
