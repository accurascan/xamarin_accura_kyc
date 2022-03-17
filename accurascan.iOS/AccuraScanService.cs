using accurascan;
using Foundation;
using NativeLibrary;
using reactnative.iOS;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AccuraScanService))]
namespace reactnative.iOS
{
    public class AccuraScanService : IAccuraScanService
    {
        private XamarinAccuraKyc _accuraKyc = new XamarinAccuraKyc();
        public NSMutableDictionary licenseConfig = null;
        public AccuraServiceCallBack callback = null;
        private NSError jsonError;

        public AccuraScanService()
        {

        }

        public void InitSDK(AccuraServiceCallBack callback)
        {
            this.callback = callback;
            //Code for get license info from native module
            _accuraKyc.InitSDKWithViewController(Platform.GetCurrentUIViewController(), (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void SetupAccuraConfig(string config, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(config);
            //Code for setup card scanning messages setup for native module
            _accuraKyc.SetupAccuraConfigWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartOCR(string config, string countryId, string cardId, string cardName, string cardType, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            int countryID1 = int.Parse(countryId);
            NSArray args = NSArray.FromObjects(config, int.Parse(countryId), int.Parse(cardId), cardName, int.Parse(cardType), orientation);
            //Code for Start scanning of OCR documents
            _accuraKyc.StartOcrWithCardWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartMRZ(string config, string mrzSelected, string mrzCountryList, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(config, mrzSelected, mrzCountryList, orientation);
            //Code for Start scanning of MRZ documents
            _accuraKyc.StartMRZWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartBarcode(string config, string barcodeSelected, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(config, barcodeSelected, orientation);
            //Code for Start scanning of Barcode
            _accuraKyc.StartBarcodeWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartBankCard(string config, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(config, orientation);
            //Code for Start scanning of Bank card
            _accuraKyc.StartBankCardWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartFaceMatch(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(accuraConfig, config, orientation);
            //Code for Start facematch between two faces
            _accuraKyc.StartFaceMatchWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }

        public void StartLiveness(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback)
        {
            this.callback = callback;
            NSArray args = NSArray.FromObjects(accuraConfig, config, orientation);
            //Code for Start liveness scanning
            _accuraKyc.StartLivenessWithArgs(args, (error, result) =>
            {
                if (error != null)
                {
                    this.callback.InvokeResult(error?.message, null);
                }
                else
                {
                    var json2 = NSJsonSerialization.Serialize(result?.result, NSJsonWritingOptions.PrettyPrinted, out jsonError);
                    licenseConfig = result?.result;
                    this.callback.InvokeResult(null, json2.ToString());
                }
            });
        }
    }
}
