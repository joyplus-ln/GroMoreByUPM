//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"

extern const char* AutonomousStringCopy(const char* string);

// IFullScreenVideoAdListener callbacks.
typedef void(*FullScreenVideoAd_OnError)(int code, const char* message, int context);
typedef void(*FullScreenVideoAd_OnFullScreenVideoAdLoad)(void* fullScreenVideoAd, int context);
typedef void(*FullScreenVideoAd_OnFullScreenVideoCached)(int context);

// IRewardAdInteractionListener callbacks.
typedef void(*FullScreenVideoAd_OnAdShow)(int context);
typedef void(*FullScreenVideoAd_OnAdVideoBarClick)(int context);
typedef void(*FullScreenVideoAd_OnAdVideoSkip)(int context);
typedef void(*FullScreenVideoAd_OnAdClose)(int context);
typedef void(*FullScreenVideoAd_OnVideoComplete)(int context);
typedef void(*FullScreenVideoAd_OnVideoError)(int code, const char* message, int context);
typedef void(*FullScreenVideoAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);
typedef void(*FullScreenVideoAd_OnAdShowFailed)(int code, const char* message, int context);
typedef void(*FullScreenVideoAd_OnRewardVerify)(bool rewardVerify, const char* rewardInfoJson, int context);

// The BURewardedVideoAdDelegate implement.
@interface ABUToUnityFullScreenVideoAd : NSObject

@end

@interface ABUToUnityFullScreenVideoAd () <ABUFullscreenVideoAdDelegate>

@property (nonatomic, strong) ABUFullscreenVideoAd *fullScreenVideoAd;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) FullScreenVideoAd_OnError onError;
@property (nonatomic, assign) FullScreenVideoAd_OnFullScreenVideoAdLoad onFullScreenVideoAdLoad;
@property (nonatomic, assign) FullScreenVideoAd_OnFullScreenVideoCached onFullScreenVideoCached;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) FullScreenVideoAd_OnAdShow onAdShow;
@property (nonatomic, assign) FullScreenVideoAd_OnAdVideoBarClick onAdVideoBarClick;
@property (nonatomic, assign) FullScreenVideoAd_OnAdVideoSkip onAdVideoSkip;
@property (nonatomic, assign) FullScreenVideoAd_OnAdClose onAdClose;
@property (nonatomic, assign) FullScreenVideoAd_OnVideoComplete onVideoComplete;
@property (nonatomic, assign) FullScreenVideoAd_OnVideoError onVideoError;
@property (nonatomic, assign) FullScreenVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail;
@property (nonatomic, assign) FullScreenVideoAd_OnAdShowFailed onAdShowFailed;
@property (nonatomic, assign) FullScreenVideoAd_OnRewardVerify onRewardVerify;

@end

@implementation ABUToUnityFullScreenVideoAd

- (void)dealloc {
    
}

#pragma mark - <---ABUFullscreenVideoAdDelegate--->
/**
 This method is called when video ad material loaded successfully.
 */
- (void)fullscreenVideoAdDidLoad:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    if (self.onFullScreenVideoAdLoad) {
        self.onFullScreenVideoAdLoad((__bridge void*)self, self.loadContext);
    }
}

/**
 This method is called when video ad materia failed to load.
 @param error : the reason of error
 */
- (void)fullscreenVideoAd:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd didFailWithError:(NSError *_Nullable)error {
    if (self.onError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/**
 This method is called when cached successfully.
 */
- (void)fullscreenVideoAdDidDownLoadVideo:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    if (self.onFullScreenVideoCached) {
        self.onFullScreenVideoCached(self.loadContext);
    }
}


/**
 This method is called when a nativeExpressAdView failed to render.
 Only for expressAd,hasExpressAdGot = yes
 @param error : the reason of error
 */
- (void)fullscreenVideoAdViewRenderFail:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd error:(NSError *_Nullable)error {
    
}

/**
 This method is called when video ad slot will be showing.
 */
- (void)fullscreenVideoAdDidVisible:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
    if (self.onWaterfallRitFillFail && self.fullScreenVideoAd.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.fullScreenVideoAd.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/**
 This method is called when video ad is clicked.
 */
- (void)fullscreenVideoAdDidClick:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    if (self.onAdVideoBarClick) {
        self.onAdVideoBarClick(self.interactionContext);
    }
}


/**
 This method is called when video ad is closed.
 */
- (void)fullscreenVideoAdDidClose:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

/**
 * This method is called when FullScreen modal has been presented.
 *  弹出详情广告页
 */
- (void)fullscreenVideoAdWillPresentFullScreenModal:(ABUFullscreenVideoAd *_Nonnull)fullscreenVideoAd {
    
}

- (void)fullscreenVideoAdDidShowFailed:(ABUFullscreenVideoAd *)fullscreenVideoAd error:(NSError *)error {
    if (self.onAdShowFailed) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onAdShowFailed((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

- (void)fullscreenVideoAdDidSkip:(ABUFullscreenVideoAd *)fullscreenVideoAd {
    if (self.onAdVideoSkip) {
        self.onAdVideoSkip(self.interactionContext);
    }
}

- (void)fullscreenVideoAd:(ABUFullscreenVideoAd *)fullscreenVideoAd didPlayFinishWithError:(NSError *_Nullable)error {
    if (error && self.onVideoError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onVideoError((int)error.code, [errMsg UTF8String], self.interactionContext);
    } else if (self.onVideoComplete) {
        self.onVideoComplete(self.interactionContext);
    }
}

/// 请求的服务器验证成功包括C2C和S2S方法回调;支持全屏视频，目前支持的adn:GDT, Load前可按需传入rewardModel信息
/// @param fullscreenVideoAd 广告管理对象
/// @param rewardInfo 奖励发放验证信息
/// @param verify 是否验证通过
- (void)fullscreenVideoAdServerRewardDidSucceed:(ABUFullscreenVideoAd *)fullscreenVideoAd rewardInfo:(ABUAdapterRewardAdInfo *_Nullable)rewardInfo verify:(BOOL)verify {
    if (self.onRewardVerify) {
        NSMutableDictionary *dict = [[NSMutableDictionary alloc]init];
        [dict setValue:rewardInfo.rewardId forKey:@"rewardId"];
        [dict setValue:rewardInfo.rewardName forKey:@"rewardName"];
        [dict setValue:@(rewardInfo.rewardAmount) forKey:@"rewardAmount"];
        [dict setValue:rewardInfo.tradeId forKey:@"tradeId"];
        [dict setValue:@(rewardInfo.verify) forKey:@"verify"];
        [dict setValue:@(rewardInfo.verifyByGroMoreS2S) forKey:@"verifyByGroMoreS2S"];
        [dict setValue:rewardInfo.customData[ABUAdapterRewardAdCustomDataReasonKey] forKey:@"reason"];
        [dict setValue:rewardInfo.customData[ABUAdapterRewardAdCustomDataErrorCodeKey] forKey:@"errorCode"];
        [dict setValue:rewardInfo.customData[ABUAdapterRewardAdCustomDataErrorMsgKey] forKey:@"errorMsg"];
        [dict setValue:rewardInfo.customData[ABUAdapterRewardAdCustomDataRewardTypeKey] forKey:@"rewardType"];
        [dict setValue:rewardInfo.customData[ABUAdapterRewardAdCustomDataRewardProposeKey] forKey:@"rewardPropose"];
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:NSJSONWritingPrettyPrinted error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onRewardVerify(verify, [strJson UTF8String], self.interactionContext);
    }
}

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_FullScreenVideoAd_Load(const char* unitID,
                                          bool useExpress2IfCanForGDT,
                                          FullScreenVideoAd_OnError onError,
                                          FullScreenVideoAd_OnFullScreenVideoAdLoad onFullScreenVideoAdLoad,
                                          FullScreenVideoAd_OnFullScreenVideoCached onFullScreenVideoCached,
                                          int context) {
    ABUFullscreenVideoAd* fullScreenVideoAd = [[ABUFullscreenVideoAd alloc] initWithAdUnitID:[NSString stringWithUTF8String:unitID ? :""]];
    fullScreenVideoAd.useExpress2IfCanForGDT = useExpress2IfCanForGDT;
    ABUToUnityFullScreenVideoAd* instance = [[ABUToUnityFullScreenVideoAd alloc] init];
    instance.fullScreenVideoAd = fullScreenVideoAd;
    instance.onError = onError;
    instance.onFullScreenVideoAdLoad = onFullScreenVideoAdLoad;
    instance.onFullScreenVideoCached = onFullScreenVideoCached;
    instance.loadContext = context;
    fullScreenVideoAd.delegate = instance;
    if ([ABUAdSDKManager configDidLoad]) {
        [instance.fullScreenVideoAd loadAdData];
    } else {
        __weak ABUFullscreenVideoAd *wFullscreenVideoAd = instance.fullScreenVideoAd;
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            __strong ABUFullscreenVideoAd *sFullscreenVideoAd = wFullscreenVideoAd;
            [sFullscreenVideoAd loadAdData];
        }];
    }
    (__bridge_retained void*)instance;
}

void UnionPlatform_FullScreenVideoAd_SetInteractionListener(void* fullScreenVideoAdPtr,
                                                            FullScreenVideoAd_OnAdShow onAdShow,
                                                            FullScreenVideoAd_OnAdVideoBarClick onAdVideoBarClick,
                                                            FullScreenVideoAd_OnAdClose onAdClose,
                                                            FullScreenVideoAd_OnVideoComplete onVideoComplete,
                                                            FullScreenVideoAd_OnVideoError onVideoError,
                                                            FullScreenVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                            FullScreenVideoAd_OnAdShowFailed onAdShowFailed,
                                                            FullScreenVideoAd_OnAdVideoSkip onAdVideoSkip,
                                                            FullScreenVideoAd_OnRewardVerify onRewardVerify,
                                                            int context) {
    ABUToUnityFullScreenVideoAd* fullScreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullScreenVideoAdPtr;
    fullScreenVideoAd.onAdShow = onAdShow;
    fullScreenVideoAd.onAdVideoBarClick = onAdVideoBarClick;
    fullScreenVideoAd.onAdClose = onAdClose;
    fullScreenVideoAd.onVideoComplete = onVideoComplete;
    fullScreenVideoAd.onVideoError = onVideoError;
    fullScreenVideoAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    fullScreenVideoAd.onAdShowFailed = onAdShowFailed;
    fullScreenVideoAd.onAdVideoSkip = onAdVideoSkip;
    fullScreenVideoAd.interactionContext = context;
    fullScreenVideoAd.onRewardVerify = onRewardVerify;
}

void UnionPlatform_FullScreenVideoAd_ShowFullScreenVideoAd(void* fullscreenVideoAdPtr,
                                                           const char* extroRitInfo,
                                                           FullScreenVideoAd_OnAdShow onAdShow,
                                                           FullScreenVideoAd_OnAdVideoBarClick onAdVideoBarClick,
                                                           FullScreenVideoAd_OnAdClose onAdClose,
                                                           FullScreenVideoAd_OnVideoComplete onVideoComplete,
                                                           FullScreenVideoAd_OnVideoError onVideoError,
                                                           FullScreenVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                           FullScreenVideoAd_OnAdShowFailed onAdShowFailed,
                                                           FullScreenVideoAd_OnAdVideoSkip onAdVideoSkip,
                                                           FullScreenVideoAd_OnRewardVerify onRewardVerify,
                                                           int context) {
    ABUToUnityFullScreenVideoAd* fullScreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
    fullScreenVideoAd.onAdShow = onAdShow;
    fullScreenVideoAd.onAdVideoBarClick = onAdVideoBarClick;
    fullScreenVideoAd.onAdClose = onAdClose;
    fullScreenVideoAd.onVideoComplete = onVideoComplete;
    fullScreenVideoAd.onVideoError = onVideoError;
    fullScreenVideoAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    fullScreenVideoAd.onAdShowFailed = onAdShowFailed;
    fullScreenVideoAd.onAdVideoSkip = onAdVideoSkip;
    fullScreenVideoAd.interactionContext = context;
    fullScreenVideoAd.onRewardVerify = onRewardVerify;
    NSDictionary *extroRitMap = nil;
    if (extroRitInfo) {
        NSString *extroRitStr = [NSString stringWithUTF8String:extroRitInfo];
        NSData *jsonData = [extroRitStr dataUsingEncoding:NSUTF8StringEncoding];
        NSError *error = nil;
        extroRitMap = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&error];
    }
    [fullScreenVideoAd.fullScreenVideoAd showAdFromRootViewController:GetAppController().rootViewController extroInfos:extroRitMap];
}

const char* UnionPlatform_FullScreenVideoAd_GetAdRitInfoAdnName(void* fullscreenVideoAdPtr) {
    ABUToUnityFullScreenVideoAd *fullscreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
    NSString *adnName = fullscreenVideoAd.fullScreenVideoAd.getShowEcpmInfo.adnName;
    return AutonomousStringCopy([adnName UTF8String]);
}

const char* UnionPlatform_FullScreenVideoAd_GetAdNetworkRitId(void* fullscreenVideoAdPtr) {
    ABUToUnityFullScreenVideoAd *fullscreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
    NSString *adNetworkRitId = [fullscreenVideoAd.fullScreenVideoAd getShowEcpmInfo].slotID;
    return AutonomousStringCopy([adNetworkRitId UTF8String]);
}

const char* UnionPlatform_FullScreenVideoAd_GetPreEcpm(void* fullscreenVideoAdPtr) {
    ABUToUnityFullScreenVideoAd *fullscreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
    NSString *PreEcpm = [fullscreenVideoAd.fullScreenVideoAd getShowEcpmInfo].ecpm;
    return AutonomousStringCopy([PreEcpm UTF8String]);
}

bool UnionPlatform_FullScreenVideoAd_isReady(void* fullscreenVideoAdPtr) {
    ABUToUnityFullScreenVideoAd *fullscreenVideoAd = (__bridge ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
    return fullscreenVideoAd.fullScreenVideoAd.isReady;
}

void UnionPlatform_FullScreenVideoAd_Dispose(void* fullscreenVideoAdPtr) {
    if (fullscreenVideoAdPtr) {
        ABUToUnityFullScreenVideoAd *fullscreenVideoAd = (__bridge_transfer ABUToUnityFullScreenVideoAd*)fullscreenVideoAdPtr;
        if (fullscreenVideoAd.fullScreenVideoAd) {
            fullscreenVideoAd.fullScreenVideoAd.delegate = nil;
            fullscreenVideoAd.fullScreenVideoAd = nil;
        }
        fullscreenVideoAd = nil;
    }
}

#if defined (__cplusplus)
}
#endif
