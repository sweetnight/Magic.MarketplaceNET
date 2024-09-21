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
            public static EventType ChatSuccess { get; } = new EventType(11, "Chat success");

        } // end of class

        public event Action<ChatEventArgs>? ChatEvent;

        public class ChatEventArgs
        {
            public EventType EventType { get; set; }
            public long FacebookChatId { get; set; }
            public Chrome? Chrome { get; set; }

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

        public Chat(long facebookChatId, Chrome chrome, int timeout, CancellationToken cancellationToken)
        {

            this.FacebookChatId = facebookChatId;
            this.Chrome = chrome;
            this.Timeout = timeout;
            this.CancellationToken = cancellationToken;

        } // end of method

        public bool Start(string chatMessage)
        {

            ChatEvent?.Invoke(new ChatEventArgs(EventType.StartChat, FacebookChatId, Chrome));

            if(string.IsNullOrEmpty(chatMessage))
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.ChatMessageIsEmpty, FacebookChatId, Chrome));
            }

            Chrome.Navigate($"https://www.facebook.com/messages/t/{this.FacebookChatId}/");

            WebElement messageTextBox = new WebElement();
            
            for(int i = 0; i < 2; i++)
            {
                messageTextBox = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='pesan' and {Chrome.ToLower("@aria-placeholder")}='aa' and @role='textbox']", Timeout);

                if(!messageTextBox.State)
                {
                    Chrome.Refresh();
                    continue;
                }
            }

            // disini sudah loading sepenuhnya
            // mesti cek jendela pin, kasih waktu 3 detik saja

            WebElement pinRequestDialogCloseButton = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'menyinkronkan')]/ancestor::div[@role='dialog']//div[{Chrome.ToLower("@aria-label")}='tutup' and @role='button']", 3);

            if(pinRequestDialogCloseButton.State)
            {
                pinRequestDialogCloseButton.SafeClick();

                WebElement confirmationDialog = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'jangan sinkronkan')]/ancestor::div[{Chrome.ToLower("@aria-label")}='jangan sinkronkan' and @role='button' and not(@aria-disabled='true')]", Timeout);
                confirmationDialog.SafeClick();
            }

            if (!messageTextBox.State)
            {
                ChatEvent?.Invoke(new ChatEventArgs(EventType.NoMessageTextBox, FacebookChatId, Chrome));
                return false;
            }

            // digunakan untuk menunggu flickr loading halaman
            Thread.Sleep(3000);

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

            Thread.Sleep(3000);

            ChatEvent?.Invoke(new ChatEventArgs(EventType.ChatSuccess, FacebookChatId, Chrome));

            return true;

        } // end of method

    } // end of class

} // end of namespace
