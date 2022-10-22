//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"

extern const char* AutonomousStringCopy(const char* string);

typedef void(*InterstitialFullAd_OnAdLoad)(void* interstitialFullAd, int context);
typedef void(*InterstitialFullAd_OnError)(int errCode,const char* errMsg,int context);
typedef void(*InterstitialFullAd_OnCached)(int context);

typedef void(*InterstitialFullAd_OnViewRenderFail)(int code, const char* message, int context);
typedef void(*InterstitialFullAd_OnAdClicked)(int context);
typedef void(*InterstitialFullAd_OnAdShow)(int context);
typedef void(*InterstitialFullAd_OnAdShowFailed)(int errcode, const char* errorMsg, int context);
typedef void(*InterstitialFullAd_OnSkippedVideo)(int context);
typedef void(*InterstitialFullAd_OnAdClose)(int context);
typedef void(*InterstitialFullAd_OnVideoComplete)(int context);
typedef void(*InterstitialFullAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);
typedef void(*InterstitialFullAd_OnRewardVerify)(bool rewardVerify, const char* rewardInfoJson, int context);

@interface ABUToUnityInterstitialFullAd : NSObject <ABUInterstitialProAdDelegate>

@property(nonatomic, strong) ABUInterstitialProAd *ad;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) InterstitialFullAd_OnAdLoad onLoad;
@property (nonatomic, assign) InterstitialFullAd_OnError onLoadError;
@property (nonatomic, assign) InterstitialFullAd_OnCached onCached;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) InterstitialFullAd_OnViewRenderFail onViewRenderFail;
@property (nonatomic, assign) InterstitialFullAd_OnAdClicked onAdClicked;
@property (nonatomic, assign) InterstitialFullAd_OnAdShow onAdShow;
@property (nonatomic, assign) InterstitialFullAd_OnAdShowFailed onAdShowFailed;
@property (nonatomic, assign) InterstitialFullAd_OnSkippedVideo onSkippedVideo;
@property (nonatomic, assign) InterstitialFullAd_OnAdClose onAdClose;
@property (nonatomic, assign) InterstitialFullAd_OnVideoComplete onVideoComplete;
@property (nonatomic, assign) InterstitialFullAd_OnWaterfallRitFillFail onWaterfallRitFillFail;
@property (nonatomic, assign) InterstitialFullAd_OnRewardVerify onRewardVerify;

@end

@implementation ABUToUnityInterstitialFullAd

- (void)dealloc {
    
}

/// 广告加载成功回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidLoad:(ABUInterstitialProAd *_Nonnull)interstitialProAd {
    if (self.onLoad) {
        self.onLoad((__bridge void*)self, self.loadContext);
    }
}

/// 广告已加载视频素材回调；非视频素材会在load之后立即给出，开发者可统一在该回调后作为离线展示广告的条件
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidDownLoadVideo:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    if (self.onCached) {
        self.onCached(self.loadContext);
    }
}

/// 视频素材广告播放完毕；
/// @param interstitialProAd 广告管理对象
/// @param error 正常播放完毕为nil；异常结束时承载错误信息
- (void)interstitialProAdDidPlayFinish:(ABUInterstitialProAd * _Nonnull)interstitialProAd didFailWithError:(NSError *_Nullable)error{
    if (self.onVideoComplete) {
        self.onVideoComplete(self.interactionContext);
    }
}

/// 广告加载失败回调
/// @param interstitialProAd 广告管理对象
/// @param error 加载错误信息
- (void)interstitialProAd:(ABUInterstitialProAd *_Nonnull)interstitialProAd didFailWithError:(NSError *_Nullable)error{
    if (self.onLoadError) {
        NSString *errMsg = error.localizedDescription?:@"";
        self.onLoadError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/// 模板广告渲染失败时回调，非模板广告不会回调该方法
/// @param interstitialProAd 广告管理对象
/// @param error 错误信息
- (void)interstitialProAdViewRenderFail:(ABUInterstitialProAd *_Nonnull)interstitialProAd error:(NSError *__nullable)error{
    if (self.onViewRenderFail) {
        NSString *errMsg = error.localizedDescription?:@"";
        self.onViewRenderFail((int)error.code, [errMsg UTF8String], self.interactionContext);
    }
}

/// 广告展示回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidVisible:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
    if (self.onWaterfallRitFillFail && self.ad.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.ad.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/// 广告展示失败回调
/// @param interstitialProAd 广告管理对象
/// @param error 展示失败的原因
- (void)interstitialProAdDidShowFailed:(ABUInterstitialProAd *_Nonnull)interstitialProAd error:(NSError *_Nonnull)error{
    if (self.onAdShowFailed) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onAdShowFailed((int)error.code, [errMsg UTF8String], self.interactionContext);
    }
}

/// 广告点击事件回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidClick:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    if (self.onAdClicked) {
        self.onAdClicked(self.interactionContext);
    }
}

/// 广告点击跳过事件回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidSkip:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    if (self.onSkippedVideo) {
        self.onSkippedVideo(self.interactionContext);
    }
}

/// 广告关闭事件回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdDidClose:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

/// 即将弹出广告详情页回调
/// @param interstitialProAd 广告管理对象
- (void)interstitialProAdWillPresentFullScreenModal:(ABUInterstitialProAd *_Nonnull)interstitialProAd{
    
}

/// 请求的服务器验证成功包括C2C和S2S方法回调;支持全屏视频，目前支持的adn:GDT, Load前可按需传入rewardModel信息
/// @param interstitialProAd 广告管理对象
/// @param rewardInfo 奖励发放验证信息
/// @param verify 是否验证通过
- (void)interstitialProAdServerRewardDidSucceed:(ABUInterstitialProAd *_Nonnull)interstitialProAd rewardInfo:(ABUAdapterRewardAdInfo *_Nullable)rewardInfo verify:(BOOL)verify{
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

void UnionPlatform_InterstitialFullAd_Load(const char* unitID,
                                           float width,
                                           float height,
                                           bool mutedIfCan,
                                           const char* userID,
                                           const char* rewardName,
                                           int rewardAmount,
                                           const char* extraInfo,
                                           InterstitialFullAd_OnError onLoadError,
                                           InterstitialFullAd_OnAdLoad onLoad,
                                           InterstitialFullAd_OnCached onCached,
                                           int context) {
    ABUToUnityInterstitialFullAd *instance = [[ABUToUnityInterstitialFullAd alloc] init];
    CGFloat newWidth = width/[UIScreen mainScreen].scale;
    CGFloat newHeight = height/[UIScreen mainScreen].scale;
    instance.ad = [[ABUInterstitialProAd alloc]initWithAdUnitID:[NSString stringWithUTF8String:unitID? :""] sizeForInterstitial:CGSizeMake(newWidth, newHeight)];
    instance.ad.delegate = instance;
    ABURewardedVideoModel *model = [[ABURewardedVideoModel alloc] init];
    {
        if (userID != NULL) {
            NSString *userIDStr = [[NSString alloc] initWithUTF8String:userID];
            model.userId = userIDStr;
        }
        if (rewardName != NULL) {
            NSString *rewardNameStr = [[NSString alloc] initWithUTF8String:rewardName];
            model.rewardName = rewardNameStr;
        }
        model.rewardAmount = rewardAmount;
        if (extraInfo != NULL) {
            NSString *extraInfoStr = [[NSString alloc] initWithUTF8String:extraInfo];
            model.extra = extraInfoStr;
        }
    }
    instance.ad.rewardModel = model;
    instance.ad.mutedIfCan = mutedIfCan;
    instance.loadContext = context;
    instance.onLoad = onLoad;
    instance.onLoadError = onLoadError;
    instance.onCached = onCached;
    if ([ABUAdSDKManager configDidLoad]) {
        [instance.ad loadAdData];
    } else {
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            [instance.ad loadAdData];
        }];
    }
    (__bridge_retained void*)instance;
}

void UnionPlatform_InterstitialFullAd_Show(void* interstitialFullAdPtr,
                                            InterstitialFullAd_OnViewRenderFail onViewRenderFail,
                                            InterstitialFullAd_OnAdClicked onAdClicked,
                                            InterstitialFullAd_OnAdShow onAdShow,
                                            InterstitialFullAd_OnAdShowFailed onAdShowFailed,
                                            InterstitialFullAd_OnSkippedVideo onSkippedVideo,
                                            InterstitialFullAd_OnAdClose onAdClose,
                                            InterstitialFullAd_OnVideoComplete onVideoComplete,
                                            InterstitialFullAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                            InterstitialFullAd_OnRewardVerify onRewardVerify,
                                            int context) {
    ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
    interstitialFullAd.interactionContext = context;
    interstitialFullAd.onViewRenderFail = onViewRenderFail;
    interstitialFullAd.onAdClicked = onAdClicked;
    interstitialFullAd.onAdShow = onAdShow;
    interstitialFullAd.onAdShowFailed = onAdShowFailed;
    interstitialFullAd.onSkippedVideo = onSkippedVideo;
    interstitialFullAd.onAdClose = onAdClose;
    interstitialFullAd.onVideoComplete = onVideoComplete;
    interstitialFullAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    interstitialFullAd.onRewardVerify = onRewardVerify;
    [interstitialFullAd.ad showAdFromRootViewController:GetAppController().rootViewController extraInfos:nil];
}

bool UnionPlatform_InterstitialFullAd_isReady(void* interstitialFullAdPtr) {
    ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
    return interstitialFullAd.ad.isReady;
}

const char* UnionPlatform_InterstitialFullAd_GetAdRitInfoAdnName(void* interstitialFullAdPtr) {
    ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
    NSString *adnName = interstitialFullAd.ad.getShowEcpmInfo.adnName;
    return AutonomousStringCopy([adnName UTF8String]);
}

const char* UnionPlatform_InterstitialFullAd_GetAdNetworkRitId(void* interstitialFullAdPtr) {
    ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
    NSString *adNetworkRitId = [interstitialFullAd.ad getShowEcpmInfo].slotID;
    return AutonomousStringCopy([adNetworkRitId UTF8String]);
}

const char* UnionPlatform_InterstitialFullAd_GetPreEcpm(void* interstitialFullAdPtr) {
    ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
    NSString *preEcpm = [interstitialFullAd.ad getShowEcpmInfo].ecpm;
    return AutonomousStringCopy([preEcpm UTF8String]);
}

void UnionPlatform_InterstitialFullAd_Dispose(void* interstitialFullAdPtr) {
    if (interstitialFullAdPtr) {
        ABUToUnityInterstitialFullAd *interstitialFullAd = (__bridge_transfer ABUToUnityInterstitialFullAd*)interstitialFullAdPtr;
        interstitialFullAd.ad.delegate = nil;
        interstitialFullAd.ad = nil;
        interstitialFullAd = nil;
    }
}

#if defined (__cplusplus)
}
#endif
