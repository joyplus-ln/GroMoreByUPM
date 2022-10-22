//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"

extern const char* AutonomousStringCopy(const char* string);

typedef void(*InterstitialAd_OnAdLoad)(void* interstitialAd, int context);
typedef void(*InterstitialAd_OnError)(int errCode,const char* errMsg,int context);
typedef void(*InterstitialAd_OnAdShow)(int context);
typedef void(*InterstitialAd_OnAdClick)(int context);
typedef void(*InterstitialAd_OnAdClose)(int context);
typedef void(*InterstitialAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);
typedef void(*InterstitialAd_OnAdShowFailed)(int code, const char* message, int context);

@interface ABUToUnityInterstitialAd : NSObject<ABUInterstitialAdDelegate>

@property(nonatomic, strong) ABUInterstitialAd *interstitialAd;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) InterstitialAd_OnAdLoad onLoad;
@property (nonatomic, assign) InterstitialAd_OnError onLoadError;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) InterstitialAd_OnAdShow onAdShow;
@property (nonatomic, assign) InterstitialAd_OnAdClick onAdClick;
@property (nonatomic, assign) InterstitialAd_OnAdClose onAdClose;
@property (nonatomic, assign) InterstitialAd_OnWaterfallRitFillFail onWaterfallRitFillFail;
@property (nonatomic, assign) InterstitialAd_OnAdShowFailed onAdShowFailed;

@end

@implementation ABUToUnityInterstitialAd

- (void)dealloc {
    
}

/**
 This method is called when interstitial ad material loaded successfully.
 */
- (void)interstitialAdDidLoad:(ABUInterstitialAd *_Nonnull)interstitialAd {
    if (self.onLoad) {
        self.onLoad((__bridge void*)self, self.loadContext);
    }
}

/**
 This method is called when interstitial ad material failed to load.
 @param error : the reason of error
 */
- (void)interstitialAd:(ABUInterstitialAd *)interstitialAd didFailWithError:(NSError * _Nullable)error {
    if (self.onLoadError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onLoadError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/**
 This method is called when a ExpressAdView failed to render.
 Only for expressAd,hasExpressAdGot = yes
 @param error : the reason of error
 */
- (void)interstitialAdViewRenderFail:(ABUInterstitialAd *_Nonnull)interstitialAd error:(NSError * __nullable)error {
    
}

/**
 This method is called when interstitial ad slot will be showing.
 */
- (void)interstitialAdDidVisible:(ABUInterstitialAd *_Nonnull)interstitialAd {
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
    if (self.onWaterfallRitFillFail && self.interstitialAd.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.interstitialAd.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/**
 This method is called when interstitial ad is clicked.
 */
- (void)interstitialAdDidClick:(ABUInterstitialAd *_Nonnull)interstitialAd {
    if (self.onAdClick) {
        self.onAdClick(self.interactionContext);
    }
}

/**
 This method is called when interstitial ad is closed.
 */
- (void)interstitialAdDidClose:(ABUInterstitialAd *_Nonnull)interstitialAd {
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

/**
 * This method is called when FullScreen modal has been presented.
 *  弹出详情广告页
 */
- (void)interstitialAdWillPresentFullScreenModal:(ABUInterstitialAd *_Nonnull)interstitialAd {
    
}

- (void)interstitialAdDidShowFailed:(ABUInterstitialAd *)interstitialAd error:(NSError *)error {
    if (self.onAdShowFailed) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onAdShowFailed((int)error.code, [errMsg UTF8String], self.interactionContext);
    }
}

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_Interstitial_Load(const char* unitID,
                                     bool useExpress2IfCanForGDT,
                                     float width,
                                     float height,
                                     InterstitialAd_OnError onLoadError,
                                     InterstitialAd_OnAdLoad onLoad,
                                     int context) {
    ABUToUnityInterstitialAd *instance = [[ABUToUnityInterstitialAd alloc] init];
    CGFloat newWidth = width/[UIScreen mainScreen].scale;
    CGFloat newHeight = height/[UIScreen mainScreen].scale;
    instance.interstitialAd = [[ABUInterstitialAd alloc] initWithAdUnitID:[NSString stringWithUTF8String:unitID? :""] size:CGSizeMake(newWidth, newHeight)];
    instance.interstitialAd.useExpress2IfCanForGDT = useExpress2IfCanForGDT;
    instance.interstitialAd.delegate = instance;
    instance.onLoad = onLoad;
    instance.onLoadError = onLoadError;
    instance.loadContext = context;
    if ([ABUAdSDKManager configDidLoad]) {
        [instance.interstitialAd loadAdData];
    } else {
        __weak ABUInterstitialAd *wInterstitialAd = instance.interstitialAd;
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            __strong ABUInterstitialAd *sInterstitialAd = wInterstitialAd;
            [sInterstitialAd loadAdData];
        }];
    }
    (__bridge_retained void*)instance;
}

void UnionPlatform_Interstitial_SetInteractionListener(void* interstitialAdPtr,
                                                       InterstitialAd_OnAdShow onAdShow,
                                                       InterstitialAd_OnAdClick onAdClick,
                                                       InterstitialAd_OnAdClose onAdClose,
                                                       InterstitialAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                       InterstitialAd_OnAdShowFailed onAdShowFailed,
                                                       int context) {
    ABUToUnityInterstitialAd *interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    interstitialAd.onAdShow = onAdShow;
    interstitialAd.onAdClick = onAdClick;
    interstitialAd.onAdClose = onAdClose;
    interstitialAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    interstitialAd.onAdShowFailed = onAdShowFailed;
    interstitialAd.interactionContext = context;
}

void UnionPlatform_Interstitial_Show(void* interstitialAdPtr,
                                     InterstitialAd_OnAdShow onAdShow,
                                     InterstitialAd_OnAdClick onAdClick,
                                     InterstitialAd_OnAdClose onAdClose,
                                     InterstitialAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                     InterstitialAd_OnAdShowFailed onAdShowFailed,
                                     int context) {
    ABUToUnityInterstitialAd *interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    interstitialAd.onAdShow = onAdShow;
    interstitialAd.onAdClick = onAdClick;
    interstitialAd.onAdClose = onAdClose;
    interstitialAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    interstitialAd.onAdShowFailed = onAdShowFailed;
    interstitialAd.interactionContext = context;
    [interstitialAd.interstitialAd showAdFromRootViewController:GetAppController().rootViewController];
}

const char* UnionPlatform_Interstitial_GetAdRitInfoAdnName(void* interstitialAdPtr) {
    ABUToUnityInterstitialAd *interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    NSString *adnName = interstitialAd.interstitialAd.getShowEcpmInfo.adnName;
    return AutonomousStringCopy([adnName UTF8String]);
}

const char* UnionPlatform_Interstitial_GetAdNetworkRitId(void* interstitialAdPtr) {
    ABUToUnityInterstitialAd* interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    NSString *adNetworkRitId = [interstitialAd.interstitialAd getShowEcpmInfo].slotID;
    return AutonomousStringCopy([adNetworkRitId UTF8String]);
}

const char* UnionPlatform_Interstitial_GetPreEcpm(void* interstitialAdPtr) {
    ABUToUnityInterstitialAd* interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    NSString *preEcpm = [interstitialAd.interstitialAd getShowEcpmInfo].ecpm;
    return AutonomousStringCopy([preEcpm UTF8String]);
}

bool UnionPlatform_Interstitial_isReady(void* interstitialAdPtr) {
    ABUToUnityInterstitialAd *interstitialAd = (__bridge ABUToUnityInterstitialAd*)interstitialAdPtr;
    return interstitialAd.interstitialAd.isReady;
}

void UnionPlatform_Interstitial_Dispose(void* interstitialAdPtr) {
    if (interstitialAdPtr) {
        ABUToUnityInterstitialAd *interstitialAd = (__bridge_transfer ABUToUnityInterstitialAd*)interstitialAdPtr;
        interstitialAd.interstitialAd.delegate = nil;
        interstitialAd.interstitialAd = nil;
        interstitialAd = nil;
    }
}

#if defined (__cplusplus)
}
#endif
