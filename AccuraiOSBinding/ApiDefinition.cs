using System;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace NativeLibrary
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. VisualStudio auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see https://aka.ms/ios-binding
    //

    // @interface GigyaAccountProxy : NSObject
    [BaseType(typeof(NSObject))]
    interface AccuraError
    {
        // @property (copy, nonatomic) NSString * _Nullable lastLogin;
        [NullAllowed, Export("message")]
        string message { get; set; }
    }

    // @interface NetworkErrorProxy : NSObject
    [BaseType(typeof(NSObject))]
    interface AccuraSuccess
    {
        // @property (copy, nonatomic) NSString * _Nullable errorMessage;
        [NullAllowed, Export("result")]
        NSMutableDictionary result { get; set; }
    }

    // @interface SwiftFrameworkProxy : NSObject
    [BaseType(typeof(NSObject))]
    interface XamarinAccuraKyc
    {
        // -(NSString * _Nonnull)initForApiKey:(NSString * _Nonnull)apiKey __attribute__((objc_method_family("none"))) __attribute__((warn_unused_result));
        //[Export("initForApiKey:")]
        //string InitForApiKey(string apiKey);

        [Export("initSDKWithViewController:completion:")]
        void InitSDKWithViewController(UIViewController viewController, Action<AccuraError, AccuraSuccess> completion);

        [Export("setupAccuraConfigWithArgs:completion:")]
        void SetupAccuraConfigWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startMRZWithArgs:completion:")]
        void StartMRZWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startBankCardWithArgs:completion:")]
        void StartBankCardWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startBarcodeWithArgs:completion:")]
        void StartBarcodeWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startOcrWithCardWithArgs:completion:")]
        void StartOcrWithCardWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startLivenessWithArgs:completion:")]
        void StartLivenessWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);

        [Export("startFaceMatchWithArgs:completion:")]
        void StartFaceMatchWithArgs(NSArray args, Action<AccuraError, AccuraSuccess> completion);
    }
}