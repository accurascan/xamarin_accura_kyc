using System;
using reactnative.Droid;
using Com.Accura.Xamarinaccurakyc;
using Org.Json;
using Android;
using Android.Support.V4.App;
using Xamarin.Essentials;
using accurascan;

[assembly: Xamarin.Forms.Dependency(typeof(AccuraScanService))]
namespace reactnative.Droid
{
    public class AccuraScanService : IAccuraScanService
    {
        XamarinAccuraKyc _accuraKyc = new XamarinAccuraKyc();
        static public AccuraServiceCallBack callback;
        int REQUEST_CAMERA = 101;

        //Code for permissions
        public AccuraScanService()
        {
            getPermissions();
        }
        public void getPermissions()
        {
            ActivityCompat.RequestPermissions(Platform.CurrentActivity, new String[] { Manifest.Permission.Camera, Manifest.Permission.RecordAudio, Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, REQUEST_CAMERA);
        }

        public void InitSDK(AccuraServiceCallBack callback)
        {
            //Code for get license info from native module
            AccuraScanService.callback = callback;
            _accuraKyc.InitSDK(Platform.CurrentActivity, Platform.AppContext, new AccuraSDKCallBack());
        }

        public void SetupAccuraConfig(string config, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            //Code for setup card scanning messages setup for native module
            _accuraKyc.SetupAccuraConfig(args, new AccuraSDKCallBack());
        }

        public void StartOCR(string config, string countryId, string cardId, string cardName, string cardType, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(countryId);
            args.Put(cardId);
            args.Put(cardName);
            args.Put(cardType);
            args.Put(orientation);
            //Code for Start scanning of OCR documents
            _accuraKyc.StartOcrWithCard(args, new AccuraSDKCallBack());
        }

        public void StartMRZ(string config, string mrzSelected, string mrzCountryList, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(mrzSelected);
            args.Put(mrzCountryList);
            args.Put(orientation);
            //Code for Start scanning of MRZ documents
            _accuraKyc.StartMRZ(args, new AccuraSDKCallBack());
        }

        public void StartBarcode(string config, string barcodeSelected, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(barcodeSelected);
            args.Put(orientation);
            //Code for Start scanning of Barcode
            _accuraKyc.StartBarcode(args, new AccuraSDKCallBack());
        }

        public void StartBankCard(string config, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(orientation);
            //Code for Start scanning of Bankcard
            _accuraKyc.StartBankCard(args, new AccuraSDKCallBack());
        }

        public void StartFaceMatch(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject accuraConfigs = new JSONObject(accuraConfig);
            args.Put(accuraConfigs);
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(orientation);
            //Code for Start facematch between two faces
            _accuraKyc.StartFaceMatch(args, new AccuraSDKCallBack());
        }

        public void StartLiveness(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback)
        {
            AccuraScanService.callback = callback;
            JSONArray args = new JSONArray();
            JSONObject accuraConfigs = new JSONObject(accuraConfig);
            args.Put(accuraConfigs);
            JSONObject configs = new JSONObject(config);
            args.Put(configs);
            args.Put(orientation);
            //Code for Start liveness scanning
            _accuraKyc.StartLiveness(args, new AccuraSDKCallBack());
        }
    }

    public class AccuraSDKCallBack : Java.Lang.Object, ICallback
    {
        //This is CallBack from native module
        public void Invoke(AccuraError p0, AccuraSuccess p1)
        {
            if (p0 != null)
            {
                //This is error block
                AccuraScanService.callback.InvokeResult(p0.Message, null);
            }
            else
            {
                //This is success block
                AccuraScanService.callback.InvokeResult(null, p1.Result.ToString());
            }
        }
    }
}
