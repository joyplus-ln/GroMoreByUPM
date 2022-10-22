//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"

extern const char* AutonomousStringCopy(const char* string);

// IRewardVideoAdListener callbacks.
typedef void(*RewardVideoAd_OnError)(int code, const char* message, int context);
typedef void(*RewardVideoAd_OnRewardVideoAdLoad)(void* rewardVideoAd, int context);
typedef void(*RewardVideoAd_OnRewardVideoCached)(int context);

// IRewardAdInteractionListener callbacks.
typedef void(*RewardVideoAd_OnAdShow)(int context);
typedef void(*RewardVideoAd_OnAdVideoBarClick)(int context);
typedef void(*RewardVideoAd_OnAdVideoSkip)(int context);
typedef void(*RewardVideoAd_OnAdClose)(int context);
typedef void(*RewardVideoAd_OnVideoComplete)(int context);
typedef void(*RewardVideoAd_OnVideoError)(int code, const char* message, int context);
typedef void(*RewardVideoAd_OnRewardVerify)(bool rewardVerify, const char* rewardInfoJson, int context);
typedef void(*RewardVideoAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);
typedef void(*RewardVideoAd_OnAdShowFailed)(int code, const char* message, int context);

@interface ABUToUnityRewardVideoAd : NSObject

@end

@interface ABUToUnityRewardVideoAd ()<ABURewardedVideoAdDelegate>

@property (nonatomic, strong) ABURewardedVideoAd *rewardedVideoAd;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) RewardVideoAd_OnError onError;
@property (nonatomic, assign) RewardVideoAd_OnRewardVideoAdLoad onRewardVideoAdLoad;
@property (nonatomic, assign) RewardVideoAd_OnRewardVideoCached onRewardVideoCached;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) RewardVideoAd_OnAdShow onAdShow;
@property (nonatomic, assign) RewardVideoAd_OnAdVideoBarClick onAdVideoBarClick;
@property (nonatomic, assign) RewardVideoAd_OnAdVideoSkip onAdVideoSkip;
@property (nonatomic, assign) RewardVideoAd_OnAdClose onAdClose;
@property (nonatomic, assign) RewardVideoAd_OnVideoComplete onVideoComplete;
@property (nonatomic, assign) RewardVideoAd_OnVideoError onVideoError;
@property (nonatomic, assign) RewardVideoAd_OnRewardVerify onRewardVerify;
@property (nonatomic, assign) RewardVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail;
@property (nonatomic, assign) RewardVideoAd_OnAdShowFailed onAdShowFailed;

@end


@implementation ABUToUnityRewardVideoAd

- (void)dealloc {
    
}

/// 广告加载成功回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidLoad:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onRewardVideoAdLoad) {
        self.onRewardVideoAdLoad((__bridge void*)self, self.loadContext);
    }
}

/// 广告加载失败回调
/// @param rewardedVideoAd 广告管理对象
/// @param error 错误信息
- (void)rewardedVideoAd:(ABURewardedVideoAd *)rewardedVideoAd didFailWithError:(NSError *_Nullable)error {
    if (self.onError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/// 广告已加载视频素材回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidDownLoadVideo:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onRewardVideoCached) {
        self.onRewardVideoCached(self.loadContext);
    }
}

/// 广告展示回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidVisible:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
    if (self.onWaterfallRitFillFail && self.rewardedVideoAd.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.rewardedVideoAd.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/// 广告展示失败回调
/// @param rewardedVideoAd 广告管理对象
/// @param error 展示失败的原因
- (void)rewardedVideoAdDidShowFailed:(ABURewardedVideoAd *_Nonnull)rewardedVideoAd error:(NSError *_Nonnull)error {
    if (self.onAdShowFailed) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onAdShowFailed((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/// 广告点击详情事件回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidClick:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onAdVideoBarClick) {
        self.onAdVideoBarClick(self.interactionContext);
    }
}

/// 广告点击跳过事件回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidSkip:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onAdVideoSkip) {
        self.onAdVideoSkip(self.interactionContext);
    }
}

/// 广告关闭事件回调
/// @param rewardedVideoAd 广告管理对象
- (void)rewardedVideoAdDidClose:(ABURewardedVideoAd *)rewardedVideoAd {
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

/// 请求的服务器验证成功包括C2C和S2S方法回调
/// @param rewardedVideoAd 广告管理对象
/// @param rewardInfo 奖励发放验证信息
/// @param verify 是否验证通过
- (void)rewardedVideoAdServerRewardDidSucceed:(ABURewardedVideoAd *)rewardedVideoAd rewardInfo:(ABUAdapterRewardAdInfo *_Nullable)rewardInfo verify:(BOOL)verify {
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

/// 广告视频播放完成或者出错回调
/// @param rewardedVideoAd 广告管理对象
/// @param error 播放出错时的信息，播放完成时为空
- (void)rewardedVideoAd:(ABURewardedVideoAd *)rewardedVideoAd didPlayFinishWithError:(NSError *_Nullable)error {
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

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_RewardVideoAd_Load(const char* unitID,
                                      bool getExpress,
                                      const char* userID,
                                      const char* rewardName,
                                      int rewardAmount,
                                      const char* extraInfo,
                                      RewardVideoAd_OnError onError,
                                      RewardVideoAd_OnRewardVideoAdLoad onRewardVideoAdLoad,
                                      RewardVideoAd_OnRewardVideoCached onRewardVideoCached,
                                      int context) {
    ABUToUnityRewardVideoAd *toUnityAd = [[ABUToUnityRewardVideoAd alloc] init];
    toUnityAd.onRewardVideoAdLoad = onRewardVideoAdLoad;
    toUnityAd.onRewardVideoCached = onRewardVideoCached;
    toUnityAd.onError = onError;
    toUnityAd.loadContext = context;
    ABURewardedVideoModel *model = [[ABURewardedVideoModel alloc] init];
    if (userID != NULL) {
        NSString *userIDStr = [[NSString alloc] initWithUTF8String:userID];
        if (userIDStr.length > 0) {
            model.userId = userIDStr;
        }
    }
    if (rewardName != NULL) {
        NSString *rewardNameStr = [[NSString alloc] initWithUTF8String:rewardName];
        if (rewardNameStr.length > 0) {
            model.rewardName = rewardNameStr;
        }
    }
    if (extraInfo != NULL) {
        NSString *extraInfoStr = [[NSString alloc] initWithUTF8String:extraInfo];
        if (extraInfoStr.length > 0) {
            model.extra = extraInfoStr;
        }
    }
    model.rewardAmount = rewardAmount;
    if (unitID == NULL) {
        return;
    }
    NSString *unitIDStr = [[NSString alloc] initWithUTF8String:unitID];
    if (unitIDStr.length > 0) {
        toUnityAd.rewardedVideoAd = [[ABURewardedVideoAd alloc] initWithAdUnitID:unitIDStr];
        toUnityAd.rewardedVideoAd.rewardedVideoModel = model;
        toUnityAd.rewardedVideoAd.getExpressAdIfCan = getExpress;
        toUnityAd.rewardedVideoAd.delegate = toUnityAd;
    }
    if ([ABUAdSDKManager configDidLoad]) {
        [toUnityAd.rewardedVideoAd loadAdData];
    } else {
        __weak ABURewardedVideoAd *wRewardedVideoAd = toUnityAd.rewardedVideoAd;
        [ABUAdSDKManager addConfigLoadSuccessObserver:toUnityAd withAction:^(id  _Nonnull observer) {
            __strong ABURewardedVideoAd *sRewardedVideoAd = wRewardedVideoAd;
            [sRewardedVideoAd loadAdData];
        }];
    }
    (__bridge_retained void*)toUnityAd;
}

void UnionPlatform_RewardVideoAd_SetInteractionListener(void* rewardedVideoAdPtr,
                                                        RewardVideoAd_OnAdShow onAdShow,
                                                        RewardVideoAd_OnAdVideoBarClick onAdVideoBarClick,
                                                        RewardVideoAd_OnAdClose onAdClose,
                                                        RewardVideoAd_OnVideoComplete onVideoComplete,
                                                        RewardVideoAd_OnAdVideoSkip onAdVideoSkip,
                                                        RewardVideoAd_OnVideoError onVideoError,
                                                        RewardVideoAd_OnRewardVerify onRewardVerify,
                                                        RewardVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                        RewardVideoAd_OnAdShowFailed onAdShowFailed,
                                                        int context) {
    ABUToUnityRewardVideoAd* rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    rewardedVideoAd.onAdShow = onAdShow;
    rewardedVideoAd.onAdVideoBarClick = onAdVideoBarClick;
    rewardedVideoAd.onAdClose = onAdClose;
    rewardedVideoAd.onVideoComplete = onVideoComplete;
    rewardedVideoAd.onAdVideoSkip = onAdVideoSkip;
    rewardedVideoAd.onVideoError = onVideoError;
    rewardedVideoAd.onRewardVerify = onRewardVerify;
    rewardedVideoAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    rewardedVideoAd.onAdShowFailed = onAdShowFailed;
    rewardedVideoAd.interactionContext = context;
}

void UnionPlatform_RewardVideoAd_ShowRewardVideoAd(void* rewardedVideoAdPtr,
                                                   const char* extroRitInfo,
                                                   RewardVideoAd_OnAdShow onAdShow,
                                                   RewardVideoAd_OnAdVideoBarClick onAdVideoBarClick,
                                                   RewardVideoAd_OnAdClose onAdClose,
                                                   RewardVideoAd_OnVideoComplete onVideoComplete,
                                                   RewardVideoAd_OnAdVideoSkip onAdVideoSkip,
                                                   RewardVideoAd_OnVideoError onVideoError,
                                                   RewardVideoAd_OnRewardVerify onRewardVerify,
                                                   RewardVideoAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                                   RewardVideoAd_OnAdShowFailed onAdShowFailed,
                                                   int context) {
    ABUToUnityRewardVideoAd* rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    rewardedVideoAd.onAdShow = onAdShow;
    rewardedVideoAd.onAdVideoBarClick = onAdVideoBarClick;
    rewardedVideoAd.onAdClose = onAdClose;
    rewardedVideoAd.onVideoComplete = onVideoComplete;
    rewardedVideoAd.onAdVideoSkip = onAdVideoSkip;
    rewardedVideoAd.onVideoError = onVideoError;
    rewardedVideoAd.onRewardVerify = onRewardVerify;
    rewardedVideoAd.onWaterfallRitFillFail = onWaterfallRitFillFail;
    rewardedVideoAd.onAdShowFailed = onAdShowFailed;
    rewardedVideoAd.interactionContext = context;
    NSDictionary *extroRitMap = nil;
    if (extroRitInfo) {
        NSString *extroRitStr = [NSString stringWithUTF8String:extroRitInfo];
        NSData *jsonData = [extroRitStr dataUsingEncoding:NSUTF8StringEncoding];
        NSError *error = nil;
        extroRitMap = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&error];
    }
    [rewardedVideoAd.rewardedVideoAd showAdFromRootViewController:GetAppController().rootViewController extraInfos:extroRitMap];
}

const char* UnionPlatform_RewardVideoAd_GetAdRitInfoAdnName(void* rewardedVideoAdPtr) {
    ABUToUnityRewardVideoAd *rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    NSString *adnName = rewardedVideoAd.rewardedVideoAd.getShowEcpmInfo.adnName;
    return AutonomousStringCopy([adnName UTF8String]);
}

const char* UnionPlatform_RewardVideoAd_GetAdNetworkRitId(void* rewardedVideoAdPtr) {
    ABUToUnityRewardVideoAd* rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    return AutonomousStringCopy([[rewardedVideoAd.rewardedVideoAd getShowEcpmInfo].slotID UTF8String]);
}

const char* UnionPlatform_RewardVideoAd_GetPreEcpm( void* rewardedVideoAdPtr) {
    ABUToUnityRewardVideoAd* rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    return AutonomousStringCopy([[rewardedVideoAd.rewardedVideoAd getShowEcpmInfo].ecpm UTF8String]);
}

bool UnionPlatform_RewardVideoAd_isReady(void* rewardedVideoAdPtr) {
    ABUToUnityRewardVideoAd *rewardedVideoAd = (__bridge ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
    return rewardedVideoAd.rewardedVideoAd.isReady;
}

void UnionPlatform_RewardVideoAd_ShowRewardVideoAdWithScene() {
    
}

void UnionPlatform_RewardVideoAd_Dispose(void* rewardedVideoAdPtr) {
    if (rewardedVideoAdPtr) {
        ABUToUnityRewardVideoAd* rewardedVideoAd = (__bridge_transfer ABUToUnityRewardVideoAd*)rewardedVideoAdPtr;
        rewardedVideoAd.rewardedVideoAd.delegate = nil;
        rewardedVideoAd.rewardedVideoAd = nil;
        rewardedVideoAd = nil;
    }
}

#if defined (__cplusplus)
}
#endif
