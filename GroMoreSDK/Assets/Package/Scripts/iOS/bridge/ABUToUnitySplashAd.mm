//
//  BUToUnitySplashAd.cpp
//  Unity-iPhone
//
//  Created by wangchao on 2020/6/8.
//

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"

extern const char* AutonomousStringCopy(const char* string);

// 强持有ABUToUnitySplashAd，防止意外释放
static NSMutableArray *toUnitySplashAds = [[NSMutableArray alloc] init];
// 开屏用户兜底信息
static ABUSplashUserData *splashData;

// ISplashAdListener callbacks.
typedef void(*SplashAd_OnLoad)(void* splashAd, int context);
typedef void(*SplashAd_OnLoadError)(int errCode, const char* errMsg, int context);
typedef void(*SplashAd_OnAdShow)(int context);
typedef void(*SplashAd_OnAdClick)(int context);
typedef void(*SplashAd_OnAdClose)(int context);
typedef void(*SplashAd_OnAdSkip)(int context);
typedef void(*SplashAd_OnAdTimeOver)(int context);
typedef void(*SplashAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);
typedef void(*SplashAd_OnAdShowFailed)(int code, const char* message, int context);

@interface ABUToUnitySplashAd : NSObject<ABUSplashAdDelegate>

//@property (nonatomic, strong) ABUSplashUserData *splashData;
@property (nonatomic, strong) ABUSplashAd *splashAd;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) int interactionContext;

@property (nonatomic, assign) SplashAd_OnLoad onLoad;
@property (nonatomic, assign) SplashAd_OnLoadError onLoadError;
@property (nonatomic, assign) SplashAd_OnAdShow onAdShow;
@property (nonatomic, assign) SplashAd_OnAdClick onAdClick;
@property (nonatomic, assign) SplashAd_OnAdClose onAdClose;
@property (nonatomic, assign) SplashAd_OnAdSkip onAdSkip;
@property (nonatomic, assign) SplashAd_OnAdTimeOver onAdTimeOver;
@property (nonatomic, assign) SplashAd_OnWaterfallRitFillFail onWaterfallRitFillFail;
@property (nonatomic, assign) SplashAd_OnAdShowFailed onAdShowFailed;

@end

@implementation ABUToUnitySplashAd

- (void)splashAdDidLoad:(ABUSplashAd *)splashAd {
    if (self.onLoad) {
        self.onLoad((__bridge void*)self, self.loadContext);
    }
}

- (void)splashAd:(ABUSplashAd *)splashAd didFailWithError:(NSError *)error {
    if (self.onLoadError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onLoadError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

- (void)splashAdWillVisible:(ABUSplashAd *)splashAd {
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
    if (self.onWaterfallRitFillFail && self.splashAd.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.splashAd.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

- (void)splashAdDidClick:(ABUSplashAd *)splashAd {
    if (self.onAdClick) {
        self.onAdClick(self.interactionContext);
    }
}

- (void)splashAdCountdownToZero:(ABUSplashAd *)splashAd {
    if (self.onAdTimeOver) {
        self.onAdTimeOver(self.interactionContext);
    }
}

- (void)splashAdDidClose:(ABUSplashAd *_Nonnull)splashAd {
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

- (void)splashAdDidShowFailed:(ABUSplashAd *)splashAd error:(NSError *)error {
    if (self.onAdShowFailed) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onAdShowFailed((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}



#if defined (__cplusplus)
extern "C" {
#endif
    void UnionPlatform_SplashAd_SetUserData(int adnType,
                                            const char* appID,
                                            const char* ritID) {
        //用于在广告位还在失败，采用传入的rit进行广告加载;该配置需要在load前设置
        ABUSplashUserData *userData = [[ABUSplashUserData alloc] init];
        userData.adnType = (ABUAdnType)adnType;
        userData.appID = [NSString stringWithUTF8String:appID];     // 如果使用穿山甲兜底，请务必传入与MSDK初始化时一致的appID
        userData.rit = [NSString stringWithUTF8String:ritID];    // 开屏对应的代码位
        splashData = userData;
    }
    
    void UnionPlatform_SplashAd_Load(const char* unitID,
                                     int timeout,
                                     int splashButtonType,
                                     SplashAd_OnLoadError onLoadError,
                                     SplashAd_OnLoad onLoad,
                                     int context) {
        ABUToUnitySplashAd* instance = [[ABUToUnitySplashAd alloc] init];
        instance.loadContext = context;
        instance.onLoad = onLoad;
        instance.onLoadError = onLoadError;
        instance.splashAd = [[ABUSplashAd alloc] initWithAdUnitID:[NSString stringWithUTF8String:unitID ? :""]];
        instance.splashAd.delegate = instance;
        instance.splashAd.splashButtonType = (ABUSplashButtonType)splashButtonType;
        // 用户兜底设置
        if (splashData) {
            // 在广告位配置拉取失败后，会使用传入的rit和appID兜底，进行广告加载，需要在创建manager时就调用该接口（仅支持穿山甲/MTG/Ks/GDT/百度）
            NSError *error = nil;
            [instance.splashAd setUserData:splashData error:&error];
            // ！！！如果有错误信息说明setUserData调用有误，需按错误提示重新设置
            if (error) {
                NSLog(@"%@", error);
            }
        }
        if (timeout > 0) {
            instance.splashAd.tolerateTimeout = timeout;
        }
        instance.splashAd.rootViewController = GetAppController().rootViewController;
        [instance.splashAd loadAdData];
        (__bridge_retained void*)instance;
        [toUnitySplashAds addObject:instance];
    }
    
    void UnionPlatform_SplashAd_SetInteractionListener(void* splashAdPtr,
                                                       SplashAd_OnAdShow onAdShow,
                                                       SplashAd_OnAdClick onAdClick,
                                                       SplashAd_OnAdClose onAdClose,
                                                       SplashAd_OnAdSkip onAdSkip,
                                                       SplashAd_OnAdTimeOver onAdTimeOver,
                                                       SplashAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                       SplashAd_OnAdShowFailed onAdShowFailed,
                                                       int context) {
        ABUToUnitySplashAd* instance = (__bridge ABUToUnitySplashAd*)splashAdPtr;
        instance.interactionContext = context;
        instance.onAdShow = onAdShow;
        instance.onAdClick = onAdClick;
        instance.onAdClose = onAdClose;
        instance.onAdSkip = onAdSkip;
        instance.onAdTimeOver = onAdTimeOver;
        instance.onWaterfallRitFillFail = onWaterfallRitFillFail;
        instance.onAdShowFailed = onAdShowFailed;
    }
    
    void UnionPlatform_SplashAd_Show (void* splashAdPtr,
                                      SplashAd_OnAdShow onAdShow,
                                      SplashAd_OnAdClick onAdClick,
                                      SplashAd_OnAdClose onAdClose,
                                      SplashAd_OnAdSkip onAdSkip,
                                      SplashAd_OnAdTimeOver onAdTimeOver,
                                      SplashAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                      SplashAd_OnAdShowFailed onAdShowFailed,
                                      int context) {
        ABUToUnitySplashAd *instance = (__bridge ABUToUnitySplashAd*)splashAdPtr;
        instance.interactionContext = context;
        instance.onAdShow = onAdShow;
        instance.onAdClick = onAdClick;
        instance.onAdClose = onAdClose;
        instance.onAdSkip = onAdSkip;
        instance.onAdTimeOver = onAdTimeOver;
        instance.onWaterfallRitFillFail = onWaterfallRitFillFail;
        instance.onAdShowFailed = onAdShowFailed;
        [instance.splashAd showInWindow:[UIApplication sharedApplication].keyWindow];
    }
    
    const char* UnionPlatform_SplashAd_GetAdRitInfoAdnName(void* splashAdPtr) {
        ABUToUnitySplashAd *instance = (__bridge ABUToUnitySplashAd*)splashAdPtr;
        NSString *adnName = [instance.splashAd getShowEcpmInfo].adnName;
        return AutonomousStringCopy([adnName UTF8String]);
    }
    
    const char* UnionPlatform_SplashAd_GetAdNetworkRitId( void* splashAdPtr) {
        ABUToUnitySplashAd* instance = (__bridge ABUToUnitySplashAd*)splashAdPtr;
        NSString *adNetworkRitId = [instance.splashAd getShowEcpmInfo].slotID;
        return AutonomousStringCopy([adNetworkRitId UTF8String]);
    }
    
    const char* UnionPlatform_SplashAd_GetPreEcpm(void* splashAdPtr) {
        ABUToUnitySplashAd* instance = (__bridge ABUToUnitySplashAd*)splashAdPtr;
        NSString *preEcpm = [instance.splashAd getShowEcpmInfo].ecpm;
        return AutonomousStringCopy([preEcpm UTF8String]);
    }
    
    void UnionPlatform_SplashAd_Dispose(void* splashAdPtr) {
        if (splashAdPtr) {
            ABUToUnitySplashAd *instance = (__bridge_transfer ABUToUnitySplashAd*)splashAdPtr;
            [instance.splashAd destoryAd];
            instance.splashAd.delegate = nil;
            instance.splashAd = nil;
            //            if (toUnitySplashAds && toUnitySplashAds.count) {
            //                [toUnitySplashAds removeAllObjects];
            //            }
        }
    }
    
#if defined (__cplusplus)
}
#endif

@end
