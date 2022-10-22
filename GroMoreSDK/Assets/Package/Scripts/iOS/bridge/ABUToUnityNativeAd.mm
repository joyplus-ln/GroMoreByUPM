//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import <ABUAdSDK/ABUAdSDK.h>
#import "UnityAppController.h"
#import "ABUCanvasView+Layout.h"

extern const char* AutonomousStringCopy(const char* string);

typedef void(*NativeAd_OnError)(int code, const char* message, int context);
typedef void(*NativeAd_OnNativeAdLoad)(void* nativeAd, int adCount, int context);
typedef void(*NativeAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);

typedef void(*NativeAd_OnAdShow)(int index, int context);
typedef void(*NativeAd_OnAdDidClick)(int index, int context);
typedef void(*NativeAd_OnAdClose)(int index, int context);
typedef void(*NativeAd_OnAllAdClose)(int context);

@interface ABUToUnityNativeAd : NSObject <ABUNativeAdsManagerDelegate, ABUNativeAdViewDelegate, ABUNativeAdVideoDelegate>

@property (nonatomic, strong) ABUNativeAdsManager *nadManager;

@property (nonatomic, strong) NSMutableArray<ABUNativeAdView *> *nativeAdViews;
@property (nonatomic, strong) NSMutableArray<ABUNativeAdView *> *notCloseViews;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) NativeAd_OnError onError;
@property (nonatomic, assign) NativeAd_OnNativeAdLoad onNativeAdLoad;
@property (nonatomic, assign) NativeAd_OnWaterfallRitFillFail onWaterfallRitFillFail;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) NativeAd_OnAdShow onAdShow;
@property (nonatomic, assign) NativeAd_OnAdDidClick onAdDidClick;
@property (nonatomic, assign) NativeAd_OnAdClose onAdClose;
@property (nonatomic, assign) NativeAd_OnAllAdClose onAllAdClose;

@property (nonatomic, assign) float adWidth;
@property (nonatomic, assign) float adHeight;

@end

@implementation ABUToUnityNativeAd

- (void)dealloc {
    
}

- (void)refreshUIWithIndex:(NSInteger)index loaction:(CGPoint)point {
    if (index < self.nativeAdViews.count) {
        ABUNativeAdView *adView = (ABUNativeAdView *)[self.nativeAdViews objectAtIndex:index];
        [adView exampleLayoutWithFrame:CGRectMake(point.x, point.y, self.adWidth, self.adHeight)];
        adView.dislikeBtn.tag = index;
        [adView.dislikeBtn addTarget:self action:@selector(closeNativeAd:) forControlEvents:UIControlEventTouchUpInside];
    }
}

- (void)closeNativeAd:(UIButton *)btn {
    if (self.onAdClose) {
        self.onAdClose((int)btn.tag, self.interactionContext);
    }
    if (btn.tag < self.nativeAdViews.count) {
        ABUNativeAdView *temp = self.nativeAdViews[btn.tag];
        [temp removeFromSuperview];
        [self.notCloseViews removeObject:temp];
        if (self.notCloseViews.count == 0 && self.onAllAdClose) {
            self.onAllAdClose(self.interactionContext);
        };
    }
}

- (NSMutableArray<ABUNativeAdView *> *)nativeAdViews {
    if (!_nativeAdViews) {
        _nativeAdViews = [[NSMutableArray alloc] init];
    }
    return _nativeAdViews;
}

- (NSMutableArray<ABUNativeAdView *> *)notCloseViews {
    if (!_notCloseViews) {
        _notCloseViews = [[NSMutableArray alloc] init];
    }
    return _notCloseViews;
}

# pragma mark ---<ABUNativeAdsManagerDelegate>---
- (void)nativeAdsManagerSuccessToLoad:(ABUNativeAdsManager *_Nonnull)adsManager nativeAds:(NSArray<ABUNativeAdView *> *_Nullable)nativeAdViewArray {
    if (nativeAdViewArray.count <= 0) {
        return;
    }
    
    for (NSInteger i = 0; i < 1; i++) {// i < nativeAdViewArray.count  和安卓保持一直，暂时只支持一条
        ABUNativeAdView *model = nativeAdViewArray[i];
        [self.nativeAdViews addObject:model];
        [self.notCloseViews addObject:model];
        model.delegate = self;
        if (model.hasExpressAdGot) {
            [model render];
        }
        if (model.data.imageMode == ABUFeedVideoAdModeImage) {
            model.videoDelegate = self;
        }
    }
    
    if (self.onNativeAdLoad) {
        self.onNativeAdLoad((__bridge void*)self, 1, self.loadContext);// (int)nativeAdViewArray.count
    }
    
    if (self.onWaterfallRitFillFail && self.nadManager.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.nadManager.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

- (void)nativeAdsManager:(ABUNativeAdsManager *_Nonnull)adsManager didFailWithError:(NSError *_Nullable)error {
    if (self.onError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}


# pragma mark ---<ABUNativeAdViewDelegate>---
- (void)nativeAdExpressViewRenderSuccess:(ABUNativeAdView *_Nonnull)nativeExpressAdView {
    
}

/**
 * This method is called when a nativeExpressAdView failed to render
 */
- (void)nativeAdExpressViewRenderFail:(ABUNativeAdView *_Nonnull)nativeExpressAdView error:(NSError *_Nullable)error {
    
}

/**
 This method is called when native ad slot has been shown.
 */
- (void)nativeAdDidBecomeVisible:(ABUNativeAdView *_Nonnull)nativeAdView {
    if (self.onAdShow) {
        NSInteger index = [self.nativeAdViews indexOfObject:nativeAdView];
        self.onAdShow((int)index, self.interactionContext);
    }
}

/**
 This method is called when native ad is clicked.
 */
- (void)nativeAdDidClick:(ABUNativeAdView *_Nonnull)nativeAdView withView:(UIView *_Nullable)view {
    if (self.onAdDidClick) {
        NSInteger index = [self.nativeAdViews indexOfObject:nativeAdView];
        self.onAdDidClick((int)index, self.interactionContext);
    }
}

/**
 * Sent after an ad view is clicked, a ad landscape view will present modal content
 */
- (void)nativeAdViewWillPresentFullScreenModal:(ABUNativeAdView *_Nonnull)nativeAdView {
    
}

- (void)nativeAdExpressViewDidClosed:(ABUNativeAdView *)nativeAdView closeReason:(NSArray<ABUDislikeWords *> *)filterWords {
    [nativeAdView removeFromSuperview];
    if (self.onAdClose) {
        NSInteger index = [self.nativeAdViews indexOfObject:nativeAdView];
        self.onAdClose((int)index, self.interactionContext);
    }
    [self.notCloseViews removeObject:nativeAdView];
    if (self.notCloseViews.count == 0 && self.onAllAdClose) {
        self.onAllAdClose(self.interactionContext);
    };
}

# pragma mark ---<ABUNativeAdVideoDelegate>---
/**
 This method is called when videoadview playback status changed.
 @param playerState : player state after changed
 */
- (void)nativeAdVideo:(ABUNativeAdView *_Nullable)nativeAdView stateDidChanged:(ABUPlayerPlayState)playerState {
    
}

/**
 This method is called when videoadview's finish view is clicked.
 */
- (void)nativeAdVideoDidClick:(ABUNativeAdView *_Nullable)nativeAdView {
    if (self.onAdDidClick) {
        NSInteger index = [self.nativeAdViews indexOfObject:nativeAdView];
        self.onAdDidClick((int)index, self.interactionContext);
    }
}

/**
 This method is called when videoadview end of play.
 */
- (void)nativeAdVideoDidPlayFinish:(ABUNativeAdView *_Nullable)nativeAdView {
    
}

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_NativeAd_Load(NativeAd_OnError onError,
                                 NativeAd_OnNativeAdLoad onNativeAdLoad,
                                 NativeAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                 int context,
                                 const char* unitID,
                                 int adCount,
                                 float width,
                                 float height,
                                 bool getExpress,
                                 bool useExpress2IfCanForGDT,
                                 bool muted) {
    ABUToUnityNativeAd* instance = [[ABUToUnityNativeAd alloc] init];
    instance.onError = onError;
    instance.onNativeAdLoad = onNativeAdLoad;
    instance.onWaterfallRitFillFail = onWaterfallRitFillFail;
    instance.loadContext = context;
    // 像素转点
    CGFloat xWidth = width/[UIScreen mainScreen].scale;
    CGFloat xHeight = height/[UIScreen mainScreen].scale;
    
    // ABUAdUnit相当于穿山甲的BUAdSlot类。聚合sdk为了和平台统一口径，统一对外主rit描述有slotID-->unitID  add in 2200 by wangchao
    ABUAdUnit *slot1 = [[ABUAdUnit alloc] init];
    ABUSize *imgSize1 = [[ABUSize alloc] init];
    imgSize1.width = 1080;
    imgSize1.height = 1920;
    //    slot1.AdType = ABUAdSlotAdTypeFeed;
    //    slot1.position = ABUAdSlotPositionFeed;
    slot1.imgSize = imgSize1;
    slot1.ID = [NSString stringWithUTF8String:unitID? :""];
    slot1.adSize = CGSizeMake(xWidth, xHeight);
    slot1.getExpressAdIfCan = getExpress;
    
    instance.nadManager= [[ABUNativeAdsManager alloc] initWithSlot:slot1];
    instance.nadManager.rootViewController = GetAppController().rootViewController;
    instance.nadManager.startMutedIfCan = muted;
    instance.nadManager.useExpress2IfCanForGDT = useExpress2IfCanForGDT;
    instance.nadManager.delegate = instance;
    instance.adWidth = xWidth;
    instance.adHeight = xHeight;
    if ([ABUAdSDKManager configDidLoad]) {
//        [instance.nadManager loadAdDataWithCount:adCount]; //因为安卓只支持一条，暂时先保持一致
        [instance.nadManager loadAdDataWithCount:1];
    } else {
        __weak ABUNativeAdsManager *wNadManager = instance.nadManager;
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            __strong ABUNativeAdsManager *sNadManager = wNadManager;
//            [sNadManager loadAdDataWithCount:adCount];
            [instance.nadManager loadAdDataWithCount:1];
        }];
    }
    //    [instance buildNativeAdViewAtIndex:0];
    (__bridge_retained void*)instance;
}

void UnionPlatform_NativeAd_SetInteractionListener(void* nativeAdPtr,
                                                   NativeAd_OnAdShow onAdShow,
                                                   NativeAd_OnAdDidClick onAdDidClick,
                                                   NativeAd_OnAdClose onAdClose,
                                                   NativeAd_OnAllAdClose onAllAdClose,
                                                   int context) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    nativeAd.onAdShow = onAdShow;
    nativeAd.onAdDidClick = onAdDidClick;
    nativeAd.onAdClose = onAdClose;
    nativeAd.onAllAdClose = onAllAdClose;
    nativeAd.interactionContext = context;
}

void UnionPlatform_NativeAd_ShowNativeAd(void* nativeAdPtr,
                                         int index,
                                         float x,
                                         float y) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    CGFloat originX = x/[UIScreen mainScreen].scale;
    CGFloat originY = y/[UIScreen mainScreen].scale;
    //    index = 0; // 3510注销，当时为何写死0
    if (index < nativeAd.nativeAdViews.count) {
        ABUNativeAdView *adView = (ABUNativeAdView *)[nativeAd.nativeAdViews objectAtIndex:index];
        if (adView.hasExpressAdGot) {// 模板直接添加
            adView.frame = CGRectMake(originX,originY,CGRectGetWidth(adView.frame),CGRectGetHeight(adView.frame));
        } else {
#warning 开发者如有自渲染布局需求，可在此处处理
            [nativeAd refreshUIWithIndex:index loaction:CGPointMake(originX, originY)];
        }
        [GetAppController().rootViewController.view addSubview:adView];
    }
}

const char* UnionPlatform_NativeAd_GetAdNetworkRitId(void* nativeAdPtr,
                                                     int index) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    if (index < nativeAd.nativeAdViews.count) {
        ABUNativeAdView *nativeAdView = [nativeAd.nativeAdViews objectAtIndex:index];
        NSString *adNetworkRitId = [nativeAdView getShowEcpmInfo].slotID;
        return AutonomousStringCopy([adNetworkRitId UTF8String]);
    }
    return "-10";
}

bool UnionPlatform_NativeAd_GetIsExpressAd(void* nativeAdPtr,
                                           int index) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    if (index < nativeAd.nativeAdViews.count) {
        ABUNativeAdView *nativeAdView = [nativeAd.nativeAdViews objectAtIndex:index];
        return nativeAdView.isExpressAd;
    }
    return false;
}

const char* UnionPlatform_NativeAd_GetAdRitInfoAdnName(void* nativeAdPtr,
                                                       int index) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    if (index < nativeAd.nativeAdViews.count) {
        ABUNativeAdView *nativeAdView = [nativeAd.nativeAdViews objectAtIndex:index];
        NSString *adnName = nativeAdView.getShowEcpmInfo.adnName;
        return AutonomousStringCopy([adnName UTF8String]);
    }
    return "";
}

const char* UnionPlatform_NativeAd_GetPreEcpm(void* nativeAdPtr,
                                              int index) {
    ABUToUnityNativeAd *nativeAd = (__bridge ABUToUnityNativeAd*)nativeAdPtr;
    if (index < nativeAd.nativeAdViews.count) {
        ABUNativeAdView *nativeAdView = [nativeAd.nativeAdViews objectAtIndex:index];
        NSString *preEcpm = [nativeAdView getShowEcpmInfo].ecpm;
        return AutonomousStringCopy([preEcpm UTF8String]);
    }
    return "-10";
}

void UnionPlatform_NativeAd_Dispose(void* nativeAdPtr) {
    if (nativeAdPtr) {
        dispatch_async(dispatch_get_main_queue(), ^{
            ABUToUnityNativeAd *nativeAd = (__bridge_transfer ABUToUnityNativeAd*)nativeAdPtr;
            nativeAd.nadManager.delegate = nil;
            [nativeAd.nadManager destory];
            nativeAd.nadManager = nil;
            nativeAd = nil;
            [nativeAd.nativeAdViews removeAllObjects];
            [nativeAd.notCloseViews removeAllObjects];
        });
    }
}

#if defined (__cplusplus)
}
#endif
