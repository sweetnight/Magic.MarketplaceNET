using Magic.BrowserAutomationNET;
using System.Diagnostics;
using System.Text;

namespace Magic.MarketplaceNET.Facebook
{

    public class Post
    {

        #region EVENTS

        public class EventType
        {

            public int Code { get; }
            public string Message { get; }

            private EventType(int code, string message)
            {

                Code = code;
                Message = message;
            }

            public static EventType StartPosting { get; } = new EventType(1, "Start Posting");
            public static EventType NotLoggedIn { get; } = new EventType(2, "Akun belum login ke Facebook / Marketplace");
            public static EventType AccountCheckpoint { get; } = new EventType(3, "Akun checkpoint");
            public static EventType MPBanned { get; } = new EventType(3, "Marketplace FB dibanned");
            public static EventType PostLimitChecking { get; } = new EventType(4, "Memulai cek limit posting");
            public static EventType PostLimited { get; } = new EventType(5, "Limit posting oleh Facebook");
            public static EventType InputtingPhotos { get; } = new EventType(6, "Memulai input foto");
            public static EventType UploadPhotosFailed { get; } = new EventType(7, "Somehow gagal upload foto");
            public static EventType StopAfterPhotosUploaded { get; } = new EventType(8, "Order stop setelah upload foto");
            public static EventType InputtingTitle { get; } = new EventType(9, "Memulai input title");
            public static EventType InputTitleFailed { get; } = new EventType(10, "Somehow gagal input judul");
            public static EventType StopAfterTitleInputted { get; } = new EventType(11, "Order stop setelah input judul");
            public static EventType InputtingPrice { get; } = new EventType(12, "Memulai input harga");
            public static EventType InputPriceFailed { get; } = new EventType(13, "Somehow gagal input harga");
            public static EventType StopAfterPriceInputted { get; } = new EventType(14, "Order stop setelah input harga");
            public static EventType SelectingCategory { get; } = new EventType(15, "Memulai pilih category");
            public static EventType ClickCategoryFailed { get; } = new EventType(16, "Somehow gagal klik pilihan kategori");
            public static EventType SelectCategoryFailed { get; } = new EventType(17, "Gagal memilih salah satu kategori");
            public static EventType StopAfterCategorySelected { get; } = new EventType(18, "Order stop setelah memilih kategori");
            public static EventType SelectingLocation { get; } = new EventType(19, "Memulai memilih lokasi");
            public static EventType ClickLocationFailed { get; } = new EventType(20, "Somehow gagal klik inputan lokasi");
            public static EventType LocationPasteFailed { get; } = new EventType(21, "Gagal paste lokasi");
            public static EventType SelectLocationFailed { get; } = new EventType(22, "Gagal klik memilih salah satu lokasi");
            public static EventType StopAfterLocationSelected { get; } = new EventType(23, "Order stop setelah memilih lokasi");
            public static EventType InputtingDescription { get; } = new EventType(24, "Memulai input keterangan");
            public static EventType DescriptionPasteFailed { get; } = new EventType(25, "Gagal paste keterangan");
            public static EventType StopAfterDescriptionInputted { get; } = new EventType(26, "Order stop setelah input keterangan");
            public static EventType SelectingCondition { get; } = new EventType(27, "Memulai memilih kondisi");
            public static EventType ClickConditionFailed { get; } = new EventType(28, "Gagal klik pilihan kondisi");
            public static EventType SelectConditionFailed { get; } = new EventType(29, "Gagal memilih salah satu kondisi");
            public static EventType StopAfterConditionSelected { get; } = new EventType(30, "Order stop setelah memilih kondisi");
            public static EventType SelectingAvailability { get; } = new EventType(31, "Memulai memilih ketersediaan");
            public static EventType ClickAvailabilityFailed { get; } = new EventType(32, "Gagal klik pilihan keterseiaan");
            public static EventType SelectAvailabilityFailed { get; } = new EventType(33, "Gagal klik memilih salah satu ketersediaan");
            public static EventType StopAfterAvailabilitySelected { get; } = new EventType(34, "Order stop setelah memilih ketersediaan");
            public static EventType InputtingLabel { get; } = new EventType(35, "Akan memasukkan label");
            public static EventType InputLabelFailed { get; } = new EventType(36, "Gagal input label");
            public static EventType InputtingSKU { get; } = new EventType(37, "Memulai input SKU");
            public static EventType InputSKUFailed { get; } = new EventType(38, "Gagal input SKU");
            public static EventType StopAfterSKUInputted { get; } = new EventType(39, "Order stop setelah input SKU");
            public static EventType SelectingPromotion { get; } = new EventType(40, "Akan memilih promosi atau tidak");
            public static EventType ClickPromotionFailed { get; } = new EventType(41, "Gagal klik promosikan tawaran");
            public static EventType StopAfterPromotionSelected { get; } = new EventType(42, "Order stop setelah memilih promosi atau tidak");
            public static EventType SelectingHideFromFriends { get; } = new EventType(43, "Akan memilih hide from friends atau tidak");
            public static EventType ClickHideFromFriendsFailed { get; } = new EventType(44, "Gagal klik sembunyikan dari teman");
            public static EventType StopAfterHideFromFriendsSelected { get; } = new EventType(45, "Order stop memilih hide from friends atau tidak");
            public static EventType StartCheckUploadedPhotos { get; } = new EventType(46, "Memulai cek foto terupload");
            public static EventType AllPhotosUploaded { get; } = new EventType(47, "Semua foto berhasil terupload");
            public static EventType NotAllPhotosUploaded { get; } = new EventType(48, "Foto tidak terupload semua, internet lambat");
            public static EventType SavingDraftStart { get; } = new EventType(49, "Akan memulai simpan draf untuk identical");
            public static EventType NoSaveDraftButton { get; } = new EventType(50, "Tombol simpan draf tidak ada/tidak aktif");
            public static EventType SaveDraftButtonClickFailed { get; } = new EventType(51, "Gagal klik tombol Simpan draf (tombol Simpan draf eksis)");
            public static EventType PostedItemListingNotShowedUp { get; } = new EventType(52, "Listing tidak muncul-muncul");
            public static EventType ItemListingNotUploaded { get; } = new EventType(53, "Listing tidak terupload aja nih");
            public static EventType ClickingContinueDraftButton { get; } = new EventType(54, "Bersiap tekan tombol lanjut setelah dari draf");
            public static EventType ContinueDraftButtonClickFailed { get; } = new EventType(55, "Gagal klik tombol Lanjutkan untuk mengedit draf (tombol Lanjutkan eksis)");
            public static EventType ReinputtingTitle { get; } = new EventType(56, "Input kembali judul dari listing draf");
            public static EventType DraftAndEditSuccess { get; } = new EventType(57, "Simpan draf dan edit listing berhasil, kemudian tekan tombol selanjutnya / terbitkan.");
            public static EventType ClickingContinueButton { get; } = new EventType(58, "Memulai tekan tombol selanjutnya / terbitkan");
            public static EventType NoContinueButton { get; } = new EventType(59, "Tombol selanjutnya ATAU publikasikan tidak ada");
            public static EventType NoPublishButton { get; } = new EventType(60, "Tombol publikasikan tidak ada");
            public static EventType PublishButtonClickFailed { get; } = new EventType(61, "Gagal klik tombol publikasikan (tombol publikasikan eksis)");
            public static EventType ContinueButtonClickFailed { get; } = new EventType(62, "Gagal klik tombol selanjutnya (tombol selanjutnya eksis)");
            public static EventType PublishButtonClicked { get; } = new EventType(63, "Tekan tombol publikasikan berhasil");
            public static EventType GoingToPostedItemListingsPage { get; } = new EventType(64, "Mencoba menghilangkan 'Promosikan tawaran Anda', dengan cara navigate ke halaman listing");
            public static EventType GettingPostedItemListingLink { get; } = new EventType(65, "Akan mencatat link tawaran aktif yang baru saja dibuat");
            public static EventType PostedItemListingLinkRetrieved { get; } = new EventType(66, "Link tawaran aktif yang baru saja dibuat berhasil diambil");
            public static EventType EditingForLabel { get; } = new EventType(67, "Akan edit listing untuk memasukkan label");
            public static EventType EditingForStrikethroughPrice { get; } = new EventType(68, "Akan edit listing untuk mengubah harga");
            public static EventType TitleEditFailed { get; } = new EventType(69, "Gagal edit judul");
            public static EventType PriceEditFailed { get; } = new EventType(70, "Gagal edit harga");
            public static EventType RenewButtonClickFailed { get; } = new EventType(71, "Gagal klik tombol perbarui (tombol perbarui eksis)");
            public static EventType EditForLabelSuccess { get; } = new EventType(72, "Berhasil edit listing untuk input label");
            public static EventType PostSuccessWithoutImages { get; } = new EventType(73, "Post produk berhasil tapi gambar tidak muncul");
            public static EventType PostSuccess { get; } = new EventType(74, "Post produk berhasil dengan baik");

        }
        
        public event Action<PostEventEventArgs>? PostEvent;

        public class PostEventEventArgs
        {
            public EventType EventType { get; set; }
            public Chrome? Chrome { get; set; }
            public ListingInputs? ListingInputs { get; set; }
            public string? ListingLink { get; set; } = "";

            public PostEventEventArgs(EventType eventType, ListingInputs listingInputs, Chrome chrome, string? listingLink = null)
            {

                EventType = eventType;
                this.ListingInputs = listingInputs;
                this.Chrome = chrome;
                this.ListingLink = listingLink;

            } // end of constructor method

        } // end of class

        #endregion

        public class ListingInputs
        {

            public List<string>? Photos { get; set; }
            public string? Title { get; set; }
            public int Price { get; set; }
            public int StrikethroughPrice { get; set; }
            public string? Category { get; set; }
            public string? Condition { get; set; }
            public string? Description { get; set; }
            public string? Availability { get; set; }
            public string? Tags { get; set; }
            public string? SKU { get; set; }
            public string? Location { get; set; }
            public int LocationPosition { get; set; } = 0;      // 0. Custom Location
            public bool Hide { get; set; }

        } // end of class

        #region INTERNALS

        private ListingInputs listingInputs { get; set; }
        private bool Identical { get; set; } = false;
        private Chrome Chrome { get; set; }
        private int Timeout { get; set; } = 10;

        bool AfterPublishResult { get; set; } = false;

        public string? TitleDraft { get; set; }
        public long? ListingID { get; set; } = 0;

        bool EditPrice { get; set; }

        public CancellationToken CancellationToken { get; set; }

        #endregion

        public Post(ListingInputs listingInputs, bool identical, Chrome chrome, int timeout, CancellationToken cancellationToken)
        {

            this.listingInputs = listingInputs;
            this.Identical = identical;
            this.Chrome = chrome;
            this.Timeout = timeout;
            CancellationToken = cancellationToken;

        } // end of method

        public void Start()
        {
            PostEvent?.Invoke(new PostEventEventArgs(EventType.StartPosting, listingInputs!, Chrome));

            Chrome.Navigate("https://www.facebook.com/marketplace/create/item");

            #region CHECK ACTIVE ACCOUNT

            WebPage checkpointWebPage = Chrome.GetCurrentUrl();

            if (checkpointWebPage.Url.Contains("checkpoint"))
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.AccountCheckpoint, listingInputs!, Chrome));
                return;
            }

            #endregion

            #region CHECK LOGGED IN

            WebElement navigationDiv = Chrome.FindElementByXPath($"//span[{Chrome.ToLower("text()")} = 'marketplace']");

            if (!navigationDiv.State)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.NotLoggedIn, listingInputs!, Chrome));
                return; // belum login
            }

            #endregion

            // ini dicek dulu apakah elementny sudah muncul? Tapi nanti digunakan di tahap INPUT PHOTOS. Disini digunakan untuk cek limit.
            WebElement insertImageElement = Chrome.FindElementByXPath($"//input[@type='file' and contains(@accept, 'image')]|//span[contains({Chrome.ToLower("text()")}, 'anda tidak bisa jual beli item')]|//span[contains({Chrome.ToLower("text()")}, 'kami sedang meninjau permintaan anda')]", Timeout);

            if(insertImageElement.Item!.TagName == "span")
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.MPBanned, listingInputs!, Chrome));
                return;
            }

            #region CHECK LIMIT HARIAN

            PostEvent?.Invoke(new PostEventEventArgs(EventType.PostLimitChecking, listingInputs!, Chrome));

            WebElement batasTercapai = Chrome.FindElementByXPath($"//span[{Chrome.ToLower("text()")} = 'batas tercapai']", 1);

            if (batasTercapai.State)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.PostLimited, listingInputs!, Chrome));
                return; // limit akun untuk posting
            }

            #endregion

            #region INPUT PHOTOS

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingPhotos, listingInputs!, Chrome));

            string photoFilenames = String.Join("\n", listingInputs!.Photos!.ToArray());

            SafeSendKeysResult safeSendKeysResult = insertImageElement.SafeSendKeys(photoFilenames);

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.UploadPhotosFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterPhotosUploaded, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT TITLE

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingTitle, listingInputs, Chrome));

            WebElement inputJudulElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")} = 'judul']//input[@type='text']", Timeout);

            if (Identical)
            {
                TitleDraft = listingInputs.Title + "-" + Magic.HelperNET.CreateRandomString(5);
                //Magic.Helper.PutContentToClipboard(titleDraft);
                safeSendKeysResult = inputJudulElement.SafeCopyAndPaste(TitleDraft);
            }
            else
            {
                //Magic.Helper.PutContentToClipboard(listingInputs.Title);
                safeSendKeysResult = inputJudulElement.SafeCopyAndPaste(listingInputs.Title!);
            }

            //safeSendKeysResult = inputJudulElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.InputTitleFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterTitleInputted, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT HARGA

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingPrice, listingInputs, Chrome));

            WebElement inputHargaElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='harga']//input[@type='text']", Timeout);

            int price = listingInputs.StrikethroughPrice > listingInputs.Price ? listingInputs.StrikethroughPrice : listingInputs.Price;

            //Magic.Helper.PutContentToClipboard(listingInputs.price.ToString());
            //safeSendKeysResult = inputHargaElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            safeSendKeysResult = inputHargaElement.SafeCopyAndPaste(price.ToString());

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.InputPriceFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterPriceInputted, listingInputs, Chrome));
                return;
            }

            #endregion

            #region PILIH CATEGORY

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingCategory, listingInputs, Chrome));

            WebElement pilihanKategoriElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='kategori']/../../..", Timeout);
            SafeClickResult safeClickResult = pilihanKategoriElement.SafeClick();
            //pilihanKategoriElement.ClickJs();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickCategoryFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            WebElement pilihanKategoriItem = Chrome.FindElementByXPath($"(//span[{Chrome.ToLower("text()")}='{listingInputs.Category!.ToLower()}'])[last()]", Timeout);

            //WebElement pilihanKategoriItem = pilihanKategoriItems.items[pilihanKategoriItems.count - 1];

            safeClickResult = pilihanKategoriItem.SafeClick();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectCategoryFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterCategorySelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region PILIH KONDISI

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingCondition, listingInputs, Chrome));

            WebElement pilihanKondisiElement = new WebElement();
            WebElement pilihanKondisiItem = new WebElement();

            bool conditionPicked = false;

            for (int i = 0; i < Timeout; i++)
            {
                pilihanKondisiElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='kondisi']/../../..", Timeout);
                safeClickResult = pilihanKondisiElement.SafeClick();

                if (!safeClickResult.Status)
                {
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickConditionFailed, listingInputs, Chrome));
                    return;
                }

                pilihanKondisiItem = Chrome.FindElementByXPath($"(//span[contains({Chrome.ToLower("text()")}, 'seperti baru')]/../../../../..//span[contains({Chrome.ToLower("text()")}, '{listingInputs.Condition!.ToLower()}')])[1]", 1);
                safeClickResult = pilihanKondisiItem.SafeClick();

                if (!safeClickResult.Status)
                {
                    continue;
                }
                else
                {
                    conditionPicked = true;
                    break;
                }
            }

            if (!conditionPicked)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectConditionFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterConditionSelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT KETERANGAN

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingDescription, listingInputs, Chrome));

            WebElement inputKeteranganElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='keterangan']//textarea", Timeout);
            //Magic.Helper.PutContentToClipboard(listingInputs.description);
            //safeSendKeysResult = inputKeteranganElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            safeSendKeysResult = inputKeteranganElement.SafeCopyAndPaste(listingInputs.Description!);

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.DescriptionPasteFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterDescriptionInputted, listingInputs, Chrome));
                return;
            }

            #endregion

            #region PILIH KETERSEDIAAN

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingAvailability, listingInputs, Chrome));

            WebElement pilihanKetersediaanElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='ketersediaan']/../../..", Timeout);
            safeClickResult = pilihanKetersediaanElement.SafeClick();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickAvailabilityFailed, listingInputs, Chrome));
                return;
            }

            WebElement pilihanKetersediaanItem = Chrome.FindElementByXPath($"(//span[contains({Chrome.ToLower("text()")}, '{listingInputs.Availability!.ToLower()}')])[1]", Timeout);
            safeClickResult = pilihanKetersediaanItem.SafeClick();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectAvailabilityFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterAvailabilitySelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT LABEL

            if (!Identical && listingInputs.Tags != "")
            {
                // jika identical, label akan hilang ketika lanjutkan edit draf, jadi tidak perlu diisi di tahap ini

                // false = gagal input label, jadi exit. true = berhasil input label, lanjut ke tahap berikutnya
                if (!InputLabel()) return;

            }

            #endregion

            #region INPUT SKU

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingSKU, listingInputs, Chrome));

            WebElement inputSKUElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='sku']//input[@type='text']", Timeout);
            //Magic.Helper.PutContentToClipboard(listingInputs.sku);
            //safeSendKeysResult = inputSKUElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            safeSendKeysResult = inputSKUElement.SafeCopyAndPaste(listingInputs.SKU!);

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.InputSKUFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterSKUInputted, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT LOKASI

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingLocation, listingInputs, Chrome));

            WebElement inputLokasiElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='lokasi']//input[@type='text']", Timeout);
            safeClickResult = inputLokasiElement.SafeClick();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickLocationFailed, listingInputs, Chrome));
                return;
            }

            //Magic.Helper.PutContentToClipboard(listingInputs.location);
            inputLokasiElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "a");
            inputLokasiElement.SafeSendKeys(OpenQA.Selenium.Keys.Delete);
            //safeSendKeysResult = inputLokasiElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            safeSendKeysResult = inputLokasiElement.SafeCopyAndPaste(listingInputs.Location!);

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.LocationPasteFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            WebElement inputLokasiItem =  new WebElement();

            if (listingInputs.LocationPosition != 0)
            {
                inputLokasiItem = Chrome.FindElementByXPath($"(//ul[contains({Chrome.ToLower("@aria-label")},'pencarian yang')]/li)[{listingInputs.LocationPosition}]", 5);
            }
            else
            {
                inputLokasiItem = Chrome.FindElementByXPath($"//ul[contains({Chrome.ToLower("@aria-label")}, 'pencarian yang') and @role='listbox']//li[@role='option'][1]", 5);
            }

            safeClickResult = inputLokasiItem.SafeClick();

            if (!safeClickResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectLocationFailed, listingInputs, Chrome));
                return;
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterLocationSelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT PROMOSI

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingPromotion, listingInputs, Chrome));

            WebElement promosikanTawaranCheck = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'promosikan tawaran')]/../../../../..//input[{Chrome.ToLower("@aria-label")}='diaktifkan']", Timeout);

            if (promosikanTawaranCheck.State)
            {
                string promosikanTawaranPosition = promosikanTawaranCheck.Item!.GetAttribute("aria-checked");

                if (promosikanTawaranPosition == "true")
                {
                    safeClickResult = promosikanTawaranCheck.SafeClick();

                    if (!safeClickResult.Status)
                    {
                        PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickPromotionFailed, listingInputs, Chrome));
                        return;
                    }

                }
            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterPromotionSelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region INPUT HIDE FROM FRIENDS

            PostEvent?.Invoke(new PostEventEventArgs(EventType.SelectingHideFromFriends, listingInputs, Chrome));

            WebElement sembunyikanCheck = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'sembunyikan dari teman')]/../../../../..//input[{Chrome.ToLower("@aria-label")}='diaktifkan']", Timeout);
            WebElement sembunyikanCheckForClick;

            if (sembunyikanCheck.State)
            {
                string sembunyikanPosition = sembunyikanCheck.Item!.GetAttribute("aria-checked");
                sembunyikanCheckForClick = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'sembunyikan dari teman')]/../../../../..//input[{Chrome.ToLower("@aria-label")}='diaktifkan']/../../../..", Timeout);

                // bisa saja pakai 1 if, lalu pake OR. Tapi lebih mudah terbaca seperti dibawah ini
                if ((sembunyikanPosition == "false" && listingInputs.Hide) ||
                    (sembunyikanPosition == "true" && !listingInputs.Hide))
                {
                    safeClickResult = sembunyikanCheckForClick.SafeClick();

                    if (!safeClickResult.Status)
                    {
                        PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickHideFromFriendsFailed, listingInputs, Chrome));
                        return;
                    }

                }

            }

            Thread.Sleep(2000);

            if (CancellationToken.IsCancellationRequested)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.StopAfterHideFromFriendsSelected, listingInputs, Chrome));
                return;
            }

            #endregion

            #region CHECK GAMBAR MINI, sudah keupload semua belum gambarnya

            /*
            int photosCount = this.listingInputs.photos.Count;

            if (photosCount > 1)
            {
                WebElements gambarMini = new WebElements();

                for (int i = 0; i < timeout * 2; i++)
                {
                    gambarMini = Chrome.FindElementsByXPath("//div[contains(@aria-label, 'Gambar mini')]", Timeout);

                    if (gambarMini.count >= photosCount || gambarMini.count == 10)
                    {
                        break;
                    }

                    Thread.Sleep(1000);
                }

                if (gambarMini.count < photosCount)
                {
                    postStatus = false;
                    postMessage = "Gambar tidak terupload semua, internet lambat.";
                    stopType = 2;
                    return;
                }
            }
            */

            #endregion

            #region CHECK FOTO SUDAH TERUPLOAD SEMUA

            PostEvent?.Invoke(new PostEventEventArgs(EventType.StartCheckUploadedPhotos, listingInputs, Chrome));

            bool photosUploaded = false;

            WebElement memuat = new WebElement();

            for (int i = 0; i < Timeout * 2; i++)
            {
                memuat = Chrome.FindElementByXPath($"//span[contains({Chrome.ToLower("text()")}, 'anda bisa menambahkan hingga')]/../../../..//div[{Chrome.ToLower("@aria-label")}='memuat...']", 1);

                if (memuat.State)
                {
                    Thread.Sleep(1000);

                    // dicoba terus selama (timeout * 2) detik
                    if (i >= Timeout * 2)
                    {
                        photosUploaded = false;
                        break;
                    }

                    continue;
                }
                else
                {
                    // selama timeout tidak ditemukan element Memuat...
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.AllPhotosUploaded, listingInputs, Chrome));
                    photosUploaded = true;
                    break;
                }
            }

            if (!photosUploaded)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.NotAllPhotosUploaded, listingInputs, Chrome));
                return;
            }

            #endregion

            #region TOMBOL SIMPAN DRAF (KHUSUS IDENTIK) & TOMBOL LANJUT

            if (Identical)
            {
                if (!SimpanDrafClick()) return;

            }

            #endregion

            #region TOMBOL SELANJUTNYA ATAU PUBLISH

            bool SelanjutnyaORTerbitkanClickResult = SelanjutnyaORTerbitkanClick();

            #endregion

            #region KLIK LISTING / KLIK TITIK TIGA DAN CATAT URL NYA (JIKA NON-IDENTIK)

            PostEvent?.Invoke(new PostEventEventArgs(EventType.GettingPostedItemListingLink, listingInputs, Chrome));

            /*
            WebElement listingImage = Chrome.FindElementByXPath($"(//div[@aria-label='{listingInputs.Title}'])[1]", Timeout);

            if (identical)
            {
                Thread.Sleep(1000);
                Chrome.Refresh();
            }

            listingImage = Chrome.FindElementByXPath($"(//div[@aria-label='{listingInputs.Title}'])[1]", Timeout);
            listingImage.SafeClick();

            WebElement listingLinkElement = Chrome.FindElementByXPath("(//div[@aria-label='Penawaran Anda']//a)[1]", Timeout);
            string listingLink = listingLinkElement.Item.GetAttribute("href");
            */

            //WebElement listingImage = Chrome.FindElementByXPath($"(//div[@aria-label='{listingInputs.Title}'])[1]", Timeout);

            /*
            if (identical)
            {
                Thread.Sleep(1000);
                Chrome.Refresh();
            }
            */

            string listingLink;

            if (Identical && ListingID != 0)
            {
                listingLink = "https://www.facebook.com/marketplace/item/" + ListingID + "/";
            }
            else
            {
                //WebElement firstThreeDotSymbol = Chrome.FindElementByXPath($"((((//div[{Chrome.ToLower("@aria-label")}='{listingInputs.Title!.ToLower()}'])[2]/../../../../../../div/div/div/div)[2]/div/div)[2]/div/div)[last()]/div/div");
                WebElement firstThreeDotSymbol = Chrome.FindElementByXPath($"(//div[@role='button' and not(.//div[contains(@class, '__fb-light-mode')]) and @aria-label='Opsi lainnya untuk {listingInputs.Title}' and .//i[@data-visualcompletion='css-img' and contains(@style, 'background-image: url(') and contains(@style, 'width: 16px; height: 16px;')]])[1]");

                //((((//div[@aria-label='Lampu Led Motor Bebek Super Terang'])[2]/../../../../../../div/div/div/div)[2]/div/div)[2]/div/div)[last()]/div/div

                safeClickResult = firstThreeDotSymbol.SafeClick(5);

                //Thread.Sleep(2000);

                WebElement listingLinkElement = new WebElement();

                bool titikTigaClicked = false;

                // untuk internet yang lambat, setelah klik titik tiga akan tidak muncul elemen lain. Jadi harus direfresh
                for (int i = 0; i < 5; i++)
                {
                    listingLinkElement = Chrome.FindElementByXPath($"(//span[contains({Chrome.ToLower("text()")}, 'lihat tawaran')]/ancestor::a)[1]", Timeout);

                    if (listingLinkElement.State)
                    {
                        titikTigaClicked = true;
                        break;
                    }

                    Thread.Sleep(2000);

                    Chrome.Refresh();
                }

                // seandainya metode titik tiga tidak berhasil
                if (titikTigaClicked)
                {
                    listingLink = listingLinkElement.Item!.GetAttribute("href");
                }
                else
                {
                    // ini jika gambar sudah terupload. Listing sudah ada gambar produknya.
                    WebElement imageUploaded = Chrome.FindElementByXPath($"(//div[{Chrome.ToLower("@aria-label")}='{listingInputs.Title!.ToLower()}'])[1]", Timeout);

                    //(//div[@aria-label='Lampu Led Motor Bebek Super Terang'])[1]

                    safeClickResult = imageUploaded.SafeClick(5);

                    listingLinkElement = Chrome.FindElementByXPath($"//span[{Chrome.ToLower("text()")}='penawaran anda']/../../../../..//a");

                    listingLink = listingLinkElement.Item!.GetAttribute("href");

                    Chrome.Refresh();
                }

                ListingID = ExtractListingIDFromHref(listingLink);
            }

            PostEvent?.Invoke(new PostEventEventArgs(EventType.PostedItemListingLinkRetrieved, listingInputs, Chrome, listingLink));

            #endregion

            #region EDIT LABEL AND PRICE

            if (!SelanjutnyaORTerbitkanClickResult)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.PostSuccessWithoutImages, listingInputs, Chrome, listingLink));
                return;
            }

            EditPrice = listingInputs.StrikethroughPrice > listingInputs.Price ? true : false;

            if ((Identical && AfterPublishResult && listingInputs.Tags != "") || EditPrice)
            {
                if (!EditListingForLabelAndPrice(listingLink)) return;
            }

            #endregion

            PostEvent?.Invoke(new PostEventEventArgs(EventType.PostSuccess, listingInputs, Chrome, listingLink));

        } // end of method

        private bool InputLabel()
        {

            // labels kosong, langsung return true saja
            if (listingInputs!.Tags == "") return true;

            PostEvent?.Invoke(new PostEventEventArgs(EventType.InputtingLabel, listingInputs!, Chrome));

            WebElement labelElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='label produk']//textarea", Timeout);

            //Magic.Helper.PutContentToClipboard(listingInputs.tags);
            //SafeSendKeysResult safeSendKeysResult = labelElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            SafeSendKeysResult safeSendKeysResult = labelElement.SafeCopyAndPaste(listingInputs.Tags!);

            if (!safeSendKeysResult.Status)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.InputLabelFailed, listingInputs, Chrome));
                return false;
            }

            Thread.Sleep(2000);

            return true;

        } // end of method

        public bool SimpanDrafClick()
        {

            // KLIK SIMPAN DRAF
            PostEvent?.Invoke(new PostEventEventArgs(EventType.SavingDraftStart, listingInputs!, Chrome));

            WebElement simpanDrafButton = new WebElement();

            for (int i = Timeout - 1; i >= 0; i--)
            {
                simpanDrafButton = Chrome.FindElementByXPath($"//div[@aria-disabled='true' and {Chrome.ToLower("@aria-label")}='simpan draf']", 1);

                if (simpanDrafButton.State)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (simpanDrafButton.State)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.NoSaveDraftButton, listingInputs!, Chrome));
                return false;
            }

            simpanDrafButton = Chrome.FindElementByXPath($"//span[{Chrome.ToLower("text()")}='simpan draf']", Timeout);

            if (!simpanDrafButton.State)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.NoSaveDraftButton, listingInputs!, Chrome));
                return false;
            }

            try
            {
                simpanDrafButton.SafeClick();
            }
            catch
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.SaveDraftButtonClickFailed, listingInputs!, Chrome));
                return false;
            }

            /*
            WebPage currentUrl;

            for (int i = 0; i < timeout; i++)
            {
                currentUrl = Chrome.GetCurrentUrl();

                // draf sudah berhasil disimpan
                if (!currentUrl.url.Contains(".facebook.com/marketplace/create/item")) break;

                Thread.Sleep(1000);
            }
            */

            WebElement drafBerhasilDisimpanNotif = new WebElement();

            for (int i = 0; i < Timeout; i++)
            {
                drafBerhasilDisimpanNotif = Chrome.FindElementByXPath($"//*[{Chrome.ToLower("text()")}='draf berhasil disimpan']", 1);

                if (drafBerhasilDisimpanNotif.State) break;
            }

            Chrome.Navigate("https://www.facebook.com/marketplace/you/selling");


            // KLIK TOMBOL LANJUT

            PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickingContinueDraftButton, listingInputs!, Chrome));

            WebElement lanjutkanButton = new WebElement();

            bool readyToLanjutkan = false;

            for (int i = 0; i < Timeout / 3; i++)
            {
                // tidak ada kepastian halaman selesai direfresh, jadi langsung pakai timeout untuk batas waktunya

                lanjutkanButton = Chrome.FindElementByXPath($"(//div[{Chrome.ToLower("@aria-label")}='{TitleDraft!.ToLower()}'])[2]/..//span[{Chrome.ToLower("text()")}='lanjut']", Timeout);

                if (lanjutkanButton.State)
                {
                    readyToLanjutkan = true;
                    break;
                }

                // jika ga ada tombol lanjutkan, berarti listing belum terdaftar. Perlu refresh.
                // tidak ada thread sleep karena timeout di atas ngedelay
                Chrome.Refresh();
            }

            if (!readyToLanjutkan)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.PostedItemListingNotShowedUp, listingInputs!, Chrome));
                return false;
            }

            // tombol lanjutkan sudah ada. Tapi apakah listing sudah sempurna didaftarkan?

            WebElement imageUploaded = new WebElement();

            readyToLanjutkan = false;
            int sleepTimer;

            // ulangi 2x
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < Timeout / 4; i++)
                {
                    // pastikan halaman sudah ter-load
                    Chrome.FindElementByXPath($"(//span[{Chrome.ToLower("text()")}='lanjut'])[1]", Timeout);

                    // ini jika gambar sudah terupload. Listing sudah ada gambar produknya.
                    imageUploaded = Chrome.FindElementByXPath($"(//div[{Chrome.ToLower("@aria-label")}='{TitleDraft!.ToLower()}'])[1]//img", 1);

                    if (imageUploaded.State)
                    {
                        readyToLanjutkan = true;
                        break;
                    }

                    // pengulang kedua (j == 1), maka sleepnya dipanjangin
                    sleepTimer = j == 0 ? 2000 : 3000;
                    Thread.Sleep(sleepTimer);

                    Chrome.Refresh();
                }

                if (readyToLanjutkan) break;
            }

            if (!readyToLanjutkan)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.ItemListingNotUploaded, listingInputs!, Chrome));
                return false;
            }

            try
            {
                WebElement atagLanjutkan = Chrome.FindElementByXPath($"(//span[{Chrome.ToLower("text()")}='lanjut'])[1]/ancestor::a", Timeout);

                string listingLink = atagLanjutkan.Item!.GetAttribute("href");
                ListingID = ExtractListingIDFromHref(listingLink);

                Chrome.Navigate("https://www.facebook.com/marketplace/edit/?listing_id=" + ListingID.ToString());
                //lanjutkanButton.SafeClick();
            }
            catch
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.ContinueDraftButtonClickFailed, listingInputs!, Chrome));
                return false;
            }

            // Proses edit draft listing
            PostEvent?.Invoke(new PostEventEventArgs(EventType.ReinputtingTitle, listingInputs!, Chrome));

            WebElement inputJudulElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='judul']//input[@type='text']", Timeout);

            //Magic.Helper.PutContentToClipboard(listingInputs.Title);

            SafeSendKeysResult safeSendKeysResult = inputJudulElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "a");
            //safeSendKeysResult = inputJudulElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
            safeSendKeysResult = inputJudulElement.SafeCopyAndPaste(listingInputs!.Title!);

            PostEvent?.Invoke(new PostEventEventArgs(EventType.DraftAndEditSuccess, listingInputs, Chrome));

            return true;
        } // end of method

        public bool SelanjutnyaORTerbitkanClick()
        {
            PostEvent?.Invoke(new PostEventEventArgs(EventType.ClickingContinueButton, listingInputs!, Chrome));

            WebElement selanjutnyaORTerbitkanButton = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='selanjutnya']|//div[{Chrome.ToLower("@aria-label")}='berikutnya']|//div[{Chrome.ToLower("@aria-label")}='terbitkan']|//div[{Chrome.ToLower("@aria-label")}='publikasikan']", Timeout);

            if (!selanjutnyaORTerbitkanButton.State)
            {
                //DisposeInstance();
                PostEvent?.Invoke(new PostEventEventArgs(EventType.NoContinueButton, listingInputs!, Chrome));
                return false;
            }

            string ariaLabel = selanjutnyaORTerbitkanButton.Item!.GetAttribute("aria-label");

            if (ariaLabel == "Selanjutnya" || ariaLabel == "Berikutnya")
            {
                try
                {
                    // publishButton ada di halaman berikutnya
                    selanjutnyaORTerbitkanButton.SafeClick();

                    WebElement publishButton = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='terbitkan']|//div[{Chrome.ToLower("@aria-label")}='publikasikan']", Timeout);

                    if (!publishButton.State)
                    {
                        //DisposeInstance();
                        PostEvent?.Invoke(new PostEventEventArgs(EventType.NoPublishButton, listingInputs!, Chrome));
                        return false;
                    }

                    try
                    {
                        publishButton.SafeClick();
                    }
                    catch
                    {
                        PostEvent?.Invoke(new PostEventEventArgs(EventType.PublishButtonClickFailed, listingInputs!, Chrome));
                        return false;
                    }

                }
                catch
                {
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.ContinueButtonClickFailed, listingInputs!, Chrome));
                    return false;
                }
            }
            else if (ariaLabel == "Terbitkan" || ariaLabel == "Publikasikan")
            {
                try
                {
                    selanjutnyaORTerbitkanButton.SafeClick();
                }
                catch
                {
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.PublishButtonClickFailed, listingInputs!, Chrome));
                    return false;
                }
            }

            WebPage currentUrl;

            for (int i = 0; i < Timeout; i++)
            {
                currentUrl = Chrome.GetCurrentUrl();

                if (currentUrl.Url.Contains(".facebook.com/marketplace/you/selling")) break;

                Thread.Sleep(1000);
            }

            PostEvent?.Invoke(new PostEventEventArgs(EventType.PublishButtonClicked, listingInputs!, Chrome));

            PostEvent?.Invoke(new PostEventEventArgs(EventType.GoingToPostedItemListingsPage, listingInputs!, Chrome));

            // Console.WriteLine("Refresh untuk menghilangkan dialog promosikan tawaran");
            Chrome.Navigate("https://www.facebook.com/marketplace/you/selling");

            // Sudah berhasil publish, namun apakah sudah terload

            WebElement imageUploaded;

            AfterPublishResult = false;
            int sleepTimer;

            // ulangi 2x
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < Timeout / 4; i++)
                {
                    Debug.WriteLine($"----------------- Cek gambar terupload ke-{i + 1}, putaran ke-{j + 1}");

                    // pastikan halaman sudah terload
                    Chrome.FindElementByXPath($"(//span[{Chrome.ToLower("text()")}='tandai sebagai habis'])[1]", Timeout);

                    // ini jika gambar sudah terupload. Listing sudah ada gambar produknya.
                    imageUploaded = Chrome.FindElementByXPath($"(//div[{Chrome.ToLower("@aria-label")}='{listingInputs!.Title!.ToLower()}'])[1]//img", 1);

                    if (imageUploaded.State)
                    {
                        AfterPublishResult = true;
                        return true;
                    }

                    // pengulang kedua (j == 1), maka sleepnya dipanjangin
                    sleepTimer = j == 0 ? 2000 : 3000;
                    Thread.Sleep(sleepTimer);

                    Chrome.Refresh();
                }
            }

            if (!AfterPublishResult) return false;

            return true;
        } // end of method

        public bool EditListingForLabelAndPrice(string listingLink)
        {

            Chrome.Navigate("https://www.facebook.com/marketplace/edit/?listing_id=" + ListingID.ToString());

            SafeSendKeysResult safeSendKeysResult;

            if (Identical)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.EditingForLabel, listingInputs!, Chrome, listingLink));

                // check title apakah masih belum sempurna

                WebElement inputJudulElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='judul']//input[@type='text']", Timeout);

                //Magic.Helper.PutContentToClipboard(listingInputs.Title);

                inputJudulElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "a");
                //safeSendKeysResult = inputJudulElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "v");
                safeSendKeysResult = inputJudulElement.SafeCopyAndPaste(listingInputs!.Title!);

                if (!safeSendKeysResult.Status)
                {
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.TitleEditFailed, listingInputs, Chrome, listingLink));
                    return false;
                }
            }

            // edit harga

            if (EditPrice)
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.EditingForStrikethroughPrice, listingInputs!, Chrome, listingLink));

                WebElement inputHargaElement = Chrome.FindElementByXPath($"//label[{Chrome.ToLower("@aria-label")}='harga']//input[@type='text']", Timeout);

                inputHargaElement.SafeSendKeys(OpenQA.Selenium.Keys.Control + "a");
                safeSendKeysResult = inputHargaElement.SafeCopyAndPaste(listingInputs!.Price.ToString());

                if (!safeSendKeysResult.Status)
                {
                    PostEvent?.Invoke(new PostEventEventArgs(EventType.PriceEditFailed, listingInputs, Chrome, listingLink));
                    return false;
                }
            }

            // edit label

            if (Identical && listingInputs!.Tags != "")
            {
                if (!InputLabel()) return false;
            }

            WebElement perbaruiButton = Chrome.FindElementByXPath($"//div[{Chrome.ToLower("@aria-label")}='perbarui']", Timeout);

            try
            {
                perbaruiButton.SafeClick();
            }
            catch
            {
                PostEvent?.Invoke(new PostEventEventArgs(EventType.RenewButtonClickFailed, listingInputs!, Chrome, listingLink));
                return false;
            }

            WebPage currentUrl;

            for (int i = 0; i < Timeout; i++)
            {
                currentUrl = Chrome.GetCurrentUrl();

                if (currentUrl.Url.Contains(".facebook.com/marketplace/you/selling")) break;

                Thread.Sleep(1000);
            }

            PostEvent?.Invoke(new PostEventEventArgs(EventType.EditForLabelSuccess, listingInputs!, Chrome, listingLink));

            return true;

        } // end of method

        private long ExtractListingIDFromHref(string href)
        {
            string listingId = string.Empty;

            if (href.Contains("/item/"))
            {
                string[] firstListingHrefArray = href.Split(new string[] { "/item/" }, StringSplitOptions.None);

                listingId = firstListingHrefArray[1].Replace("/", "");

            }
            else if (href.Contains("listing_id="))
            {
                string[] firstListingHrefArray = href.Split(new string[] { "listing_id=" }, StringSplitOptions.None);

                listingId = firstListingHrefArray[1];

            }

            return Convert.ToInt64(listingId);
        } // end of method

        private string BuildRandomSequenceWordsXPathForLocationSelection(string inputText)
        {

            // Remove commas from the input text for word-by-word processing
            string cleanedInput = inputText.Replace(",", "");
            // Split the cleaned input text by spaces
            string[] words = cleanedInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder xpath = new StringBuilder();

            // Start building the xpath
            xpath.Append("(//ul[contains(translate(@aria-label, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'pencarian yang')]//li[.//span");

            if (words.Length > 0)
            {
                xpath.Append("[");
                for (int i = 0; i < words.Length; i++)
                {
                    string lowerCaseWord = words[i].ToLower();
                    xpath.Append("contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '")
                         .Append(lowerCaseWord)
                         .Append("')");
                    if (i < words.Length - 1)
                    {
                        xpath.Append(" and ");
                    }
                }
                xpath.Append(" and contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '")
                     .Append(inputText.ToLower())
                     .Append("')");
                xpath.Append("]");
            }

            xpath.Append("])[1]");

            return xpath.ToString();

            // contoh
            // input text : Kota Bandung, Kecamatan Arcamanik
            // hasil xpath :
            // (//ul[contains(translate(@aria-label, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'pencarian yang')]//li[.//span[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'kota') and contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'bandung') and contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'kecamatan') and contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'arcamanik') and contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'kota bandung, kecamatan arcamanik')]])[1]

        } // end of method

    } // end of class

} // end of namespace
