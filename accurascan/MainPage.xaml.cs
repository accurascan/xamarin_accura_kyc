using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using accurascan;

namespace accurascan
{
    public partial class MainPage : ContentPage
    {
        public static INavigation navigation = null;
        public static IAccuraScanService accuraService = null;
        String appOriantation = "portrait";
        public static String methodCalled = "";
        public static String faceMatchURI = "";
        public static String faceMatchBase64 = "";

        public static StackLayout stkViewMain = null;
        public static StackLayout stkViewInvalidLicense = null;
        public static StackLayout stkViewLoading = null;
        public static Frame stkViewOCR = null;
        public static Frame stkViewMRZ = null;
        public static Frame stkViewBarcode = null;
        public static Frame stkViewBankCard = null;

        public static Picker stkPkCountryList = null;
        public static Picker stkPkCardList = null;
        public static Picker stkPkMRZType = null;
        public static Picker stkPkBarcodeType = null;

        public static JObject allLicenseConfig = null;
        public static JObject countryList = null;
        public static JObject countryCardsList = null;
        public static JArray mrzList = JArray.Parse(@"[{'label': 'Passport', 'value': 'passport_mrz'}, {'label': 'Mrz ID', 'value': 'id_mrz'}, {'label': 'Visa Card', 'value': 'visa_mrz'}, {'label': 'Other', 'value': 'other_mrz'}]");

        String mrz_type = "";
        String barcode_type = "";
        JObject selected_country = null;
        JObject selected_card = null;

        public static String detectFaceURI = "";
        public static Label stkLblLiveness = null;
        public static Label stkLblFaceMatch = null;
        public static Image stkImgDetectProfile = null;
        public static Image stkImgProfile = null;
        public static StackLayout stkViewFace = null;
        public static StackLayout stkViewData = null;
        public static StackLayout stkViewResult = null;

        public static StackLayout stkViewScore = null;
        public static JArray sides = JArray.Parse(@"['front_data', 'back_data']");
        public static JArray images = JArray.Parse(@"['front_img', 'back_img']");

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();

            navigation = Navigation;
            accuraService = DependencyService.Get<IAccuraScanService>();
            MainPage.stkViewMain = viewMain;
            MainPage.stkViewInvalidLicense = viewInvalidLicense;
            MainPage.stkViewLoading = viewLoading;

            MainPage.stkViewOCR = viewOCR;
            MainPage.stkViewMRZ = viewMRZ;
            MainPage.stkViewBarcode = viewBarcode;
            MainPage.stkViewBankCard = viewBankCard;

            MainPage.stkPkCountryList = ocrCountry;
            MainPage.stkPkCardList = ocrCard;
            MainPage.stkPkMRZType = mrzType;
            MainPage.stkPkBarcodeType = barcodeType;

            MainPage.stkImgProfile = imgProfile;
            MainPage.stkViewFace = viewFace;
            MainPage.stkViewData = viewData;
            MainPage.stkViewResult = viewResult;

            MainPage.stkImgDetectProfile = imgDetectProfile;
            MainPage.stkLblLiveness = lblLiveness;
            MainPage.stkLblFaceMatch = lblFaceMatch;
            MainPage.stkViewScore = viewScore;

            if (accuraService != null)
            {
                _ = DelayActionAsync(500, () =>
                {
                    //call for get license data
                    methodCalled = "InitSDK";
                    accuraService.InitSDK(new AccuraScanResultCallBack());
                });

                _ = DelayActionAsync(1000, () =>
                {
                    JObject configObj = JObject.Parse(@"{
                            'ACCURA_ERROR_CODE_MOTION':'Keep Document Steady',
                            'ACCURA_ERROR_CODE_DOCUMENT_IN_FRAME' : 'Keep document in frame',
                            'ACCURA_ERROR_CODE_BRING_DOCUMENT_IN_FRAME' : 'Bring card near to frame',
                            'ACCURA_ERROR_CODE_PROCESSING' : 'Processing...',
                            'ACCURA_ERROR_CODE_BLUR_DOCUMENT' : 'Blur detect in document',
                            'ACCURA_ERROR_CODE_FACE_BLUR' : 'Blur detected over face',
                            'ACCURA_ERROR_CODE_GLARE_DOCUMENT' : 'Glare detect in document',
                            'ACCURA_ERROR_CODE_HOLOGRAM' : 'Hologram Detected',
                            'ACCURA_ERROR_CODE_DARK_DOCUMENT' : 'Low lighting detected',
                            'ACCURA_ERROR_CODE_PHOTO_COPY_DOCUMENT' : 'Can not accept Photo Copy Document',
                            'ACCURA_ERROR_CODE_FACE' : 'Face not detected',
                            'ACCURA_ERROR_CODE_MRZ' : 'MRZ not detected',
                            'ACCURA_ERROR_CODE_PASSPORT_MRZ' : 'Passport MRZ not detected',
                            'ACCURA_ERROR_CODE_ID_MRZ' : 'ID card MRZ not detected',
                            'ACCURA_ERROR_CODE_VISA_MRZ' : 'Visa MRZ not detected',
                            'ACCURA_ERROR_CODE_WRONG_SIDE' : 'Scanning wrong side of document',
                            'ACCURA_ERROR_CODE_UPSIDE_DOWN_SIDE' : 'Document is upside down. Place it properly',
                            'IS_SHOW_LOGO' : true,
                            'SCAN_TITLE_OCR_FRONT' : 'Scan Front Side of',
                            'SCAN_TITLE_OCR_BACK' : 'Scan Back Side of',
                            'SCAN_TITLE_OCR' : 'Scan',
                            'SCAN_TITLE_BANKCARD' : 'Scan Bank Card',
                            'SCAN_TITLE_BARCODE' : 'Scan Barcode',
                            'SCAN_TITLE_MRZ_PDF417_FRONT' : 'Scan Front Side of Document',
                            'SCAN_TITLE_MRZ_PDF417_BACK' : 'Now Scan Back Side of Document',
                            'SCAN_TITLE_DLPLATE' : 'Scan Number Plate',
                      }");
                    //Setup scanning messages & logo for OCR, MRZ, Barcode & Bankcard.
                    methodCalled = "SetupAccuraConfig";
                    accuraService.SetupAccuraConfig(configObj.ToString(), new AccuraScanResultCallBack());
                });
            }
        }

        // Code or oriantation support
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width >= height)
            {
                appOriantation = "landscape";
            }
            else
            {
                appOriantation = "portrait";
            }
        }

        //Module start for OCR scanning
        // OCR country selection
        void OnCountryIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            JArray list = (JArray)allLicenseConfig["countries"];
            selected_country = (JObject)list[selectedIndex];

            JArray listCards = (JArray)selected_country["cards"];
            var newList = new List<string>();
            foreach (var x in listCards)
            {
                var newItem = (JObject)x;
                newList.Add((string)newItem["name"]);
            }
            MainPage.stkPkCardList.ItemsSource = newList;
            MainPage.stkPkCardList.SelectedIndex = -1;
        }

        // OCR card type selection
        void OnCardTypeIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex >= 0)
            {
                JArray list = (JArray)selected_country["cards"];
                selected_card = (JObject)list[selectedIndex];
            }
        }


        void OnStartOCRClick(object sender, EventArgs args)
        {
            _ = OnStartOCRClickAsync();
        }

        //Start calling of OCR documents
        async Task OnStartOCRClickAsync()
        {
            if (selected_country != null && selected_card != null)
            {
                JObject configObj = JObject.Parse(@"{
                    'enableLogs' : false
                }");
                //Start OCR scanning.
                methodCalled = "StartOCR";
                accuraService.StartOCR(configObj.ToString(), (string)selected_country["id"], (string)selected_card["id"], (string)selected_card["name"], (string)selected_card["type"], appOriantation, new AccuraScanResultCallBack());
            }
            else if (selected_country == null)
            {
                await DisplayAlert("Validation error!", "Please select country & card first.", "OK");
            }
            else if (selected_card == null)
            {
                await DisplayAlert("Validation error!", "Please select card first.", "OK");
            }
        }
        //Module end OCR scanning

        //Module start MRZ scanning
        // MRZ type selection
        void OnMRZTypeIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            var newItem = (JObject)MainPage.mrzList[selectedIndex];
            mrz_type = (String)newItem["value"];
        }

        void OnStartMRZClick(object sender, EventArgs args)
        {
            _ = OnStartMRZClickAsync();
        }

        async Task OnStartMRZClickAsync()
        {
            if (mrz_type != "")
            {
                JObject configObj = JObject.Parse(@"{
                    'enableLogs' : false
                }");
                methodCalled = "StartMRZ";
                accuraService.StartMRZ(configObj.ToString(), mrz_type, "all", appOriantation, new AccuraScanResultCallBack());
            }
            else
            {
                await DisplayAlert("Validation error!", "Please select MRZ type first.", "OK");
            }
        }

        // Barcode scanning
        void OnBarcodeTypeIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            JArray list = (JArray)MainPage.allLicenseConfig["barcodes"];
            var newItem = (JObject)list[selectedIndex];
            barcode_type = (String)newItem["name"];
        }

        void OnStartBarcodeClick(object sender, EventArgs args)
        {
            _ = OnStartBarcodeClickAsync();
        }

        async Task OnStartBarcodeClickAsync()
        {
            if (barcode_type != "")
            {
                JObject configObj = JObject.Parse(@"{
                    'enableLogs' : false
                }");
                methodCalled = "StartBarcode";
                accuraService.StartBarcode(configObj.ToString(), barcode_type, appOriantation, new AccuraScanResultCallBack());
            }
            else
            {
                await DisplayAlert("Validation error!", "Please select barcode type first.", "OK");
            }
        }

        void OnStartBankCardClick(object sender, EventArgs args)
        {
            JObject configObj = JObject.Parse(@"{
                'enableLogs' : false
            }");
            methodCalled = "StartBankCard";
            accuraService.StartBankCard(configObj.ToString(), appOriantation, new AccuraScanResultCallBack());
        }

        public static async Task navigateToResultAsync(string result)
        {
            await Task.Delay(500);
            var resultJSON = JObject.Parse(result);
            generateResultPage(resultJSON);
            //_ = MainPage.navigation.PushModalAsync(new ResultPage(resultJSON));
        }

        void OnCloseResultClick(object sender, EventArgs args)
        {
            MainPage.stkViewResult.IsVisible = false;
            MainPage.stkViewMain.IsVisible = true;
            MainPage.stkViewData.Children.Clear();
            stkImgProfile.Source = "";
        }

        public static void generateResultPage(JObject result)
        {
            MainPage.stkViewResult.IsVisible = true;
            MainPage.stkViewMain.IsVisible = false;
            MainPage.stkImgDetectProfile.IsVisible = false;
            MainPage.stkViewScore.IsVisible = false;

            if (result.ContainsKey("face"))
            {
                stkViewFace.IsVisible = true;
                faceMatchURI = result["face"].ToString();
                stkImgProfile.Source = ImageSource.FromFile(faceMatchURI.Replace("file://", ""));
            }
            else
            {
                stkViewFace.IsVisible = false;
            }
            foreach (var e in sides)
            {
                if (result[(String)e].ToString() != "{}")
                {
                    StackLayout titleStackLayout = new StackLayout
                    {
                        Padding = 10,
                        BackgroundColor = Color.LightGray,
                        Children =
                        {
                            new Label
                            {
                                HorizontalTextAlignment= TextAlignment.Start,
                                TextColor= Color.Black,
                                FontSize= 17,
                                FontAttributes= FontAttributes.Bold,
                                Text = (String)e == "back_data" ? "OCR Back" : getResultType(result["type"].ToString()),
                            }
                        }
                    };
                    stkViewData.Children.Add(titleStackLayout);

                    JObject dataObj = (JObject)result[(string)e];
                    foreach (var data in dataObj)
                    {
                        if (data.Key.ToString() != "front_img" && data.Key.ToString() != "back_img" && data.Key.ToString() != "signature")
                        {
                            StackLayout dataStackLayout = new StackLayout
                            {
                                Children =
                            {
                                new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                        new Label
                                        {
                                            HorizontalTextAlignment= TextAlignment.Start,
                                            TextColor= Color.Black,
                                            FontSize= 15,
                                            WidthRequest = 150,
                                            FontAttributes= FontAttributes.Bold,
                                            Text = getLabel(data.Key.ToString()),
                                        },
                                        new Label
                                        {
                                            WidthRequest = 1,
                                            BackgroundColor = Color.LightGray
                                        },
                                        new Label
                                        {
                                            HorizontalTextAlignment= TextAlignment.Start,
                                            TextColor= Color.Black,
                                            FontSize= 15,
                                            Text = data.Value.ToString().Replace("\r","").Replace("\n", ""),
                                        },
                                    }
                                },
                                new Label
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = Color.LightGray
                                },
                            }
                            };
                            stkViewData.Children.Add(dataStackLayout);
                        }
                        else if (data.Key.ToString() == "signature")
                        {
                            StackLayout dataStackLayout = new StackLayout
                            {
                                Children =
                            {
                                new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children =
                                    {
                                        new Label
                                        {
                                            HorizontalTextAlignment= TextAlignment.Start,
                                            TextColor= Color.Black,
                                            FontSize= 15,
                                            WidthRequest = 150,
                                            FontAttributes= FontAttributes.Bold,
                                            Text = getLabel(data.Key.ToString()),
                                        },
                                        new Label
                                        {
                                            WidthRequest = 1,
                                            BackgroundColor = Color.LightGray
                                        },
                                        new Image
                                        {
                                            HeightRequest = 80,
                                            WidthRequest = 120,
                                            Aspect= Aspect.AspectFill,
                                            Source = ImageSource.FromFile(data.Value.ToString().Replace("file://", "")),
                                        },
                                    }
                                },
                                new Label
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = Color.LightGray
                                },
                            }
                            };
                            stkViewData.Children.Add(dataStackLayout);
                        }
                    }
                }
            }

            foreach (var img in images)
            {
                if (result.ContainsKey((String)img))
                {
                    StackLayout titleStackLayout = new StackLayout
                    {
                        Padding = 10,
                        BackgroundColor = Color.LightGray,
                        Children =
                        {
                            new Label
                            {
                                HorizontalTextAlignment= TextAlignment.Start,
                                TextColor= Color.Black,
                                FontSize= 17,
                                FontAttributes= FontAttributes.Bold,
                                Text = img.ToString().Replace('_', ' ')
                            },
                        }
                    };
                    StackLayout imageStackLayout = new StackLayout
                    {
                        BackgroundColor = Color.LightGray,
                        Children =
                        {
                            new Image
                            {
                                HeightRequest = 200,
                                Aspect= Aspect.AspectFill,
                                Source = ImageSource.FromFile(result[(String)img].ToString().Replace("file://", "")),
                            }
                        }
                    };
                    stkViewData.Children.Add(titleStackLayout);
                    stkViewData.Children.Add(imageStackLayout);
                }
            }

        }

        public static String getLabel(string key)
        {
            var lableText = "";
            switch (key)
            {
                case "mrz":
                    lableText += "MRZ";
                    break;
                case "placeOfBirth":
                    lableText += "Place Of Birth";
                    break;
                case "retval":
                    lableText += "Retval";
                    break;
                case "givenNames":
                    lableText += "First Name";
                    break;
                case "country":
                    lableText += "Country";
                    break;
                case "surName":
                    lableText += "Last Name";
                    break;
                case "expirationDate":
                    lableText += "Date of Expiry";
                    break;
                case "passportType":
                    lableText += "Document Type";
                    break;
                case "personalNumber":
                    lableText += "Other ID";
                    break;
                case "correctBirthChecksum":
                    lableText += "Correct Birth Check No.";
                    break;
                case "correctSecondrowChecksum":
                    lableText += "Correct Second Row Check No.";
                    break;
                case "personalNumberChecksum":
                    lableText += "Other Id Check No.";
                    break;
                case "secondRowChecksum":
                    lableText += "Second Row Check No.";
                    break;
                case "expirationDateChecksum":
                    lableText += "Expiration Check No.";
                    break;
                case "correctPersonalChecksum":
                    lableText += "Correct Document check No.";
                    break;
                case "passportNumber":
                    lableText += "Document No.";
                    break;
                case "correctExpirationChecksum":
                    lableText += "Correct Expiration Check No.";
                    break;
                case "sex":
                    lableText += "Sex";
                    break;
                case "birth":
                    lableText += "Date Of Birth";
                    break;
                case "birthChecksum":
                    lableText += "Birth Check No.";
                    break;
                case "personalNumber2":
                    lableText += "Other ID2";
                    break;
                case "correctPassportChecksum":
                    lableText += "Correct Document check No.";
                    break;
                case "placeOfIssue":
                    lableText += "Place Of Issue";
                    break;
                case "nationality":
                    lableText += "Nationality";
                    break;
                case "passportNumberChecksum":
                    lableText += "Document check No.";
                    break;
                case "issueDate":
                    lableText += "Date Of Issue";
                    break;
                case "departmentNumber":
                    lableText += "Department No.";
                    break;
                case "signature":
                    lableText += "Signature";
                    break;
                default:
                    lableText += key;
                    break;
            }
            return lableText;
        }

        public static String getResultType(string type)
        {
            switch (type)
            {
                case "BANKCARD":
                    return "Bank Card Data";
                case "DL_PLATE":
                    return "Vehicle Plate";
                case "BARCODE":
                    return "Barcode Data";
                case "PDF417":
                    return "PDF417 Barcode";
                case "OCR":
                    return "OCR Front";
                case "MRZ":
                    return "MRZ";
                case "BARCODEPDF417":
                    return "USA DL Result";
                default:
                    return "Front Side";
            }
        }

        public async Task DelayActionAsync(int delay, Action action)
        {
            await Task.Delay(delay);

            action();
        }

        void OnStartFaceMatchClick(object sender, EventArgs args)
        {
            JObject accuraConfigObj = JObject.Parse(@"{
                'enableLogs' : false,
                'with_face' : true,
                'face_uri' : '" + faceMatchURI + "'}");
            JObject configObj = JObject.Parse(@"{
                'feedbackTextSize' : 18,
                'feedBackframeMessage' : 'Frame Your Face',
                'feedBackLowLightMessage' : 'Low light detected',
                'feedBackStartMessage' : 'Put your face inside the oval',
                'feedBackAwayMessage' : 'Move Phone Away',
                'feedBackOpenEyesMessage' : 'Keep Your Eyes Open',
                'feedBackCloserMessage' : 'Move Phone Closer',
                'feedBackCenterMessage' : 'Move Phone Center',
                'feedBackMultipleFaceMessage' : 'Multiple Face Detected',
                'feedBackHeadStraightMessage' : 'Keep Your Head Straight',
                'feedBackBlurFaceMessage' : 'Blur Detected Over Face',
                'feedBackGlareFaceMessage' : 'Glare Detected',
                'feedBackProcessingMessage' : 'Processing...',
                'setBlurPercentage' : 99,
                'setGlarePercentage_0' : -1,
                'setGlarePercentage_1' : -1,
                'isShowLogo' : true
            }");
            methodCalled = "StartFaceMatch";
            accuraService.StartFaceMatch(accuraConfigObj.ToString(), configObj.ToString(), appOriantation, new AccuraScanResultCallBack());
        }

        void OnStartLivenessClick(object sender, EventArgs args)
        {
            JObject accuraConfigObj = JObject.Parse(@"{
                'enableLogs' : false,
                'with_face' : false,
                'face_uri' : '" + faceMatchURI + "'}");
            JObject configObj = JObject.Parse(@"{
                'feedbackTextSize' : 18,
                'feedBackframeMessage' : 'Frame Your Face',
                'feedBackAwayMessage' : 'Move Phone Away',
                'feedBackOpenEyesMessage' : 'Keep Your Eyes Open',
                'feedBackCloserMessage' : 'Move Phone Closer',
                'feedBackCenterMessage' : 'Move Phone Center',
                'feedBackMultipleFaceMessage' : 'Multiple Face Detected',
                'feedBackHeadStraightMessage' : 'Keep Your Head Straight',
                'feedBackBlurFaceMessage' : 'Blur Detected Over Face',
                'feedBackGlareFaceMessage' : 'Glare Detected',
                'setBlurPercentage' : 99,
                'setGlarePercentage_0' : -1,
                'setGlarePercentage_1' : -1,
                'isSaveImage' : true,
                'liveness_url' : 'your liveness url',
                'contentType' : 'form_data',
                'feedBackLowLightMessage' : 'Low light detected',
                'feedbackLowLightTolerence' : 39,
                'feedBackStartMessage' : 'Put your face inside the oval',
                'feedBackLookLeftMessage' : 'Look over your left shoulder',
                'feedBackLookRightMessage' : 'Look over your right shoulder',
                'feedBackOralInfoMessage' : 'Say each digits out loud',
                'feedBackProcessingMessage' : 'Processing...',
                'enableOralVerification' : false,
                'codeTextColor' : 'white',
                'isShowLogo' : true
            }");
            methodCalled = "StartLiveness";
            accuraService.StartLiveness(accuraConfigObj.ToString(), configObj.ToString(), appOriantation, new AccuraScanResultCallBack());
        }

    }

    public class AccuraScanResultCallBack : AccuraServiceCallBack
    {

        public void InvokeResult(string error, string result)
        {

            if (error != null)
            {
                System.Diagnostics.Debug.WriteLine("AccuraInitCallBack ERROR=> " + error);
            }
            else
            {
                var resultJSON = JObject.Parse(result);
                switch (MainPage.methodCalled)
                {
                    case "InitSDK":
                        MainPage.stkViewLoading.IsVisible = false;
                        if ((bool)resultJSON["isValid"])
                        {
                            MainPage.allLicenseConfig = resultJSON;
                            MainPage.stkViewInvalidLicense.IsVisible = false;
                            MainPage.stkViewMain.IsVisible = true;
                            if ((bool)resultJSON["isOCR"])
                            {
                                MainPage.stkViewOCR.IsVisible = true;
                                JArray list = (JArray)resultJSON["countries"];
                                var newList = new List<string>();
                                foreach (var x in list)
                                {
                                    var newItem = (JObject)x;
                                    newList.Add((string)newItem["name"]);
                                }
                                MainPage.stkPkCountryList.ItemsSource = newList;
                            }
                            if ((bool)resultJSON["isMRZ"])
                            {
                                MainPage.stkViewMRZ.IsVisible = true;
                                JArray list = (JArray)MainPage.mrzList;
                                var newMRZList = new List<string>();
                                foreach (var x in list)
                                {
                                    var newItem = (JObject)x;
                                    newMRZList.Add((string)newItem["label"]);
                                }
                                MainPage.stkPkMRZType.ItemsSource = newMRZList;
                            }
                            if ((bool)resultJSON["isBarcode"])
                            {
                                MainPage.stkViewBarcode.IsVisible = true;
                                JArray list = (JArray)resultJSON["barcodes"];
                                var newList = new List<string>();
                                foreach (var x in list)
                                {
                                    var newItem = (JObject)x;
                                    newList.Add((string)newItem["name"]);
                                }
                                MainPage.stkPkBarcodeType.ItemsSource = newList;
                            }
                            if ((bool)resultJSON["isBankCard"])
                            {
                                MainPage.stkViewBankCard.IsVisible = true;
                            }
                        }
                        else
                        {
                            MainPage.stkViewInvalidLicense.IsVisible = true;
                            MainPage.stkViewMain.IsVisible = false;
                        }
                        break;
                    case "SetupAccuraConfig":
                        System.Diagnostics.Debug.WriteLine("SetupAccuraConfig SUCCESS.isValid=> " + resultJSON);
                        break;
                    case "StartOCR":
                        _ = MainPage.navigateToResultAsync(result);
                        break;
                    case "StartMRZ":
                        _ = MainPage.navigateToResultAsync(result);
                        break;
                    case "StartBarcode":
                        _ = MainPage.navigateToResultAsync(result);
                        break;
                    case "StartBankCard":
                        _ = MainPage.navigateToResultAsync(result);
                        break;
                    case "StartFaceMatch":

                        var number = resultJSON["score"];
                        Double dc = Math.Round((Double)number, 2);
                        MainPage.stkLblFaceMatch.Text = dc.ToString() + "%";
                        MainPage.stkLblLiveness.Text = "0%";
                        MainPage.detectFaceURI = resultJSON["detect"].ToString();
                        MainPage.stkImgDetectProfile.Source = ImageSource.FromFile(MainPage.detectFaceURI.ToString().Replace("file://", ""));
                        MainPage.stkImgDetectProfile.IsVisible = true;
                        MainPage.stkViewScore.IsVisible = true;

                        break;
                    case "StartLiveness":

                        var number1 = resultJSON["score"];
                        Double lv = Math.Round((Double)number1, 2);

                        var number2 = resultJSON["fm_score"];
                        Double fm = Math.Round((Double)number2, 2);

                        MainPage.stkLblLiveness.Text = lv.ToString() + "%";
                        MainPage.stkLblFaceMatch.Text = fm.ToString() + "%";
                        MainPage.detectFaceURI = resultJSON["detect"].ToString();
                        MainPage.stkImgDetectProfile.Source = ImageSource.FromFile(MainPage.detectFaceURI.ToString().Replace("file://", ""));
                        MainPage.stkImgDetectProfile.IsVisible = true;
                        MainPage.stkViewScore.IsVisible = true;

                        break;
                    default:
                        break;
                }
            }
        }
    }
}