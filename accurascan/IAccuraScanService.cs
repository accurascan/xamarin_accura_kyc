using System;
using Newtonsoft.Json.Linq;

namespace accurascan
{
    public interface IAccuraScanService
    {
        void InitSDK(AccuraServiceCallBack callback);
        void SetupAccuraConfig(string config, AccuraServiceCallBack callback);
        void StartOCR(string config, string countryId, string cardId, string cardName, string cardType, string orientation, AccuraServiceCallBack callback);
        void StartMRZ(string config, string mrzSelected, string mrzCountryList, string orientation, AccuraServiceCallBack callback);
        void StartBarcode(string config, string barcodeSelected, string orientation, AccuraServiceCallBack callback);
        void StartBankCard(string config, string orientation, AccuraServiceCallBack callback);

        void StartFaceMatch(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback);
        void StartLiveness(string accuraConfig, string config, string orientation, AccuraServiceCallBack callback);
    }

    public interface AccuraServiceCallBack
    {
        void InvokeResult(string error, string result);
    }
}
