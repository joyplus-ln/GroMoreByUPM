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

typedef void(*DrawAd_OnError)(int code, const char* message, int context);
typedef void(*DrawAd_OnDrawAdLoad)(void* nativeAd, int adCount, int context);
typedef void(*DrawAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);

typedef void(*DrawAd_OnAdShow)(int index, int context);
typedef void(*DrawAd_OnAdClicked)(int index, int context);
typedef void(*DrawAd_OnAdClose)(int index, int context);
typedef void(*DrawAd_OnAllAdClose)(int context);
typedef void(*DrawAd_OnRenderFail)(const char* msg, int code, int context);
typedef void(*DrawAd_OnRenderSuccess)(float width, float height, int context);

@interface ABUToUnityDrawAd : NSObject <ABUDrawAdsManagerDelegate, ABUDrawAdViewDelegate, ABUDrawAdVideoDelegate>

@property (nonatomic, strong) ABUDrawAdsManager *drawAdsManager;

@property (nonatomic, strong) NSMutableArray<ABUDrawAdView *> *drawAdViews;
@property (nonatomic, strong) NSMutableArray<ABUDrawAdView *> *notCloseViews;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) DrawAd_OnError onError;
@property (nonatomic, assign) DrawAd_OnDrawAdLoad onDrawAdLoad;
@property (nonatomic, assign) DrawAd_OnWaterfallRitFillFail onWaterfallRitFillFail;

@property (nonatomic, assign) int interactionContext;
@property (nonatomic, assign) DrawAd_OnAdShow onAdShow;
@property (nonatomic, assign) DrawAd_OnAdClicked onAdClicked;
@property (nonatomic, assign) DrawAd_OnAdClose onAdClose;
@property (nonatomic, assign) DrawAd_OnAllAdClose onAllAdClose;
@property (nonatomic, assign) DrawAd_OnRenderFail onRenderFail;
@property (nonatomic, assign) DrawAd_OnRenderSuccess onRenderSuccess;

@property (nonatomic, assign) float adWidth;
@property (nonatomic, assign) float adHeight;

@end

@implementation ABUToUnityDrawAd

- (void)dealloc {

}

- (void)refreshUIWithIndex:(NSInteger)index loaction:(CGPoint)point {
    if (index < self.drawAdViews.count) {
        ABUDrawAdView *adView = [self.drawAdViews objectAtIndex:index];
        [adView exampleLayoutWithFrame:CGRectMake(point.x, point.y, self.adWidth, self.adHeight)];
        adView.dislikeBtn.tag = index;
        [adView.dislikeBtn addTarget:self action:@selector(closeAd:) forControlEvents:UIControlEventTouchUpInside];
    }
}

- (void)closeAd:(UIButton *)btn {
    if (self.onAdClose) {
        self.onAdClose((int)btn.tag, self.interactionContext);
    }
    [self removeDrawAd:btn.tag];
}

- (void)removeDrawAd:(NSInteger)index {
    if (index < self.drawAdViews.count) {
        ABUDrawAdView *temp = self.drawAdViews[index];
        [temp removeFromSuperview];
        [self.notCloseViews removeObject:temp];
        if (self.notCloseViews.count == 0 && self.onAllAdClose) {
            self.onAllAdClose(self.interactionContext);
        };
    }
}

- (NSMutableArray<ABUDrawAdView *> *)drawAdViews {
    if (!_drawAdViews) {
        _drawAdViews = [[NSMutableArray alloc] init];
    }
    return _drawAdViews;
}

- (NSMutableArray<ABUDrawAdView *> *)notCloseViews {
    if (!_notCloseViews) {
        _notCloseViews = [[NSMutableArray alloc] init];
    }
    return _notCloseViews;
}

#pragma mark - ABUDrawAdsManagerDelegate

/// Draw ????????????????????????
/// @param adsManager ??????????????????
/// @param drawAds ???????????????GroMore????????????????????????????????????????????????????????????
- (void)drawAdsManagerSuccessToLoad:(ABUDrawAdsManager *_Nonnull)adsManager drawVideoAds:(NSArray<ABUDrawAdView *> *_Nullable)drawAds {
    if (drawAds.count <= 0) {
        return;
    }
    
    for (ABUDrawAdView *model in drawAds) {
        [self.drawAdViews addObject:model];
        [self.notCloseViews addObject:model];
        model.delegate = self;
        if (model.hasExpressAdGot) {
            [model render];
        }
        if (model.data.imageMode == ABUFeedVideoAdModeImage) {
            model.videoDelegate = self;
        }
    }
    
    if (self.onDrawAdLoad) {
        self.onDrawAdLoad((__bridge void*)self, (int)drawAds.count, self.loadContext);
    }
    
    if (self.onWaterfallRitFillFail && self.drawAdsManager.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.drawAdsManager.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/// Draw ????????????????????????
/// @param adsManager ??????????????????
/// @param error  ??????????????????
- (void)drawAdsManager:(ABUDrawAdsManager *_Nonnull)adsManager didFailWithError:(NSError *_Nullable)error {
    if (self.onError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

#pragma mark - ABUDrawAdViewDelegate

/// ?????????????????????????????????????????????????????????????????????????????????????????????
/// @param drawAdView ??????????????????
- (void)drawAdExpressViewRenderSuccess:(ABUDrawAdView *_Nonnull)drawAdView {
    if (self.onRenderSuccess) {
        self.onRenderSuccess(self.adWidth, self.adHeight, self.loadContext);
    }
}

/// ?????????????????????????????????????????????????????????????????????????????????????????????
/// @param drawAdView ??????????????????
/// @param error ??????????????????
- (void)drawAdExpressViewRenderFail:(ABUDrawAdView *_Nonnull)drawAdView error:(NSError *_Nullable)error {
    if (self.onRenderFail) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onRenderFail([errMsg UTF8String], (int)error.code, self.loadContext);
    }
}

/// ?????????????????????????????????
/// @param drawAdView ????????????
/// @param filterWords ?????????????????????adapter?????????????????????
- (void)drawAdExpressViewDidClosed:(ABUDrawAdView *_Nullable)drawAdView closeReason:(NSArray<NSDictionary *> *_Nullable)filterWords {
    [drawAdView removeFromSuperview];
    if (self.onAdClose) {
        NSInteger index = [self.drawAdViews indexOfObject:drawAdView];
        self.onAdClose((int)index, self.interactionContext);
    }
    [self.notCloseViews removeObject:drawAdView];
    if (self.notCloseViews.count == 0 && self.onAllAdClose) {
        self.onAllAdClose(self.interactionContext);
    };
}

/// ????????????????????????????????????
/// @param drawAdView ????????????
/// @param filterWords ?????????????????????adapter?????????????????????
- (void)drawAdDidClosed:(ABUDrawAdView *_Nullable)drawAdView closeReason:(NSArray<NSDictionary *> *_Nullable)filterWords {
    [drawAdView removeFromSuperview];
    if (self.onAdClose) {
        NSInteger index = [self.drawAdViews indexOfObject:drawAdView];
        self.onAdClose((int)index, self.interactionContext);
    }
    [self.notCloseViews removeObject:drawAdView];
    if (self.notCloseViews.count == 0 && self.onAllAdClose) {
        self.onAllAdClose(self.interactionContext);
    };
}

/// ????????????????????????????????????????????????
/// @param drawAdView ????????????
- (void)drawAdDidBecomeVisible:(ABUDrawAdView *_Nonnull)drawAdView {
    if (self.onAdShow) {
        NSInteger index = [self.drawAdViews indexOfObject:drawAdView];
        self.onAdShow((int)index, self.interactionContext);
    }
}

/// ??????????????????????????????????????????????????????adapter????????????
/// @param drawAdView ????????????
/// @param playerState ????????????
- (void)drawAdView:(ABUDrawAdView *_Nonnull)drawAdView stateDidChanged:(ABUPlayerPlayState)playerState {
    
}

/// ????????????????????????
/// @param drawAdView ????????????
/// @param view ??????????????????
- (void)drawAdDidClick:(ABUDrawAdView *_Nonnull)drawAdView withView:(UIView *_Nullable)view {
    if (self.onAdClicked) {
        NSInteger index = [self.drawAdViews indexOfObject:drawAdView];
        self.onAdClicked((int)index, self.interactionContext);
    }
}

/// ??????????????????????????????/???????????????
/// @param drawAdView ????????????
- (void)drawAdViewWillPresentFullScreenModal:(ABUDrawAdView *_Nonnull)drawAdView {
    
}

/// ??????????????????????????????/???????????????
/// @param drawAdView ????????????
- (void)drawAdViewWillDismissFullScreenModal:(ABUDrawAdView *_Nonnull)drawAdView {
    
}

#pragma mark - ABUDrawAdVideoDelegate

/// This method is called when videoadview playback status changed.
/// @param drawAdView draw ad view
/// @param playerState player state after changed
- (void)drawAdVideo:(ABUDrawAdView *_Nullable)drawAdView stateDidChanged:(ABUPlayerPlayState)playerState {
    
}

/// This method is called when videoadview's finish view is clicked.
/// @param drawAdView draw ad view
- (void)drawAdVideoDidClick:(ABUDrawAdView *_Nullable)drawAdView {
    
}

/// This method is called when videoadview end of play.
/// @param drawAdView draw ad view
- (void)drawAdVideoDidPlayFinish:(ABUDrawAdView *_Nullable)drawAdView {
    
}

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_Ad_Load(DrawAd_OnError onError,
                           DrawAd_OnDrawAdLoad onDrawAdLoad,
                           DrawAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                           int context,
                           const char* slotID,
                           int adCount,
                           float width,
                           float height) {
    ABUToUnityDrawAd* instance = [[ABUToUnityDrawAd alloc] init];
    instance.onError = onError;
    instance.onDrawAdLoad = onDrawAdLoad;
    instance.onWaterfallRitFillFail = onWaterfallRitFillFail;
    instance.loadContext = context;
    // ????????????
    CGFloat xWidth = width/[UIScreen mainScreen].scale;
    CGFloat xHeight = height/[UIScreen mainScreen].scale;
    instance.adWidth = xWidth;
    instance.adHeight = xHeight;
    instance.drawAdsManager = [[ABUDrawAdsManager alloc] initWithAdUnitID:[NSString stringWithUTF8String:slotID? :""] adSize:CGSizeMake(xWidth, xHeight)];
    instance.drawAdsManager.rootViewController = GetAppController().rootViewController;
    instance.drawAdsManager.delegate = instance;
    if ([ABUAdSDKManager configDidLoad]) {
        [instance.drawAdsManager loadAdDataWithCount:adCount];
    } else {
        __weak ABUDrawAdsManager *wdrawAdsManager = instance.drawAdsManager;
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            __strong ABUDrawAdsManager *sNadManager = wdrawAdsManager;
            [sNadManager loadAdDataWithCount:adCount];
        }];
    }
    (__bridge_retained void*)instance;
}

void UnionPlatform_DrawAd_SetInteractionListener(void* drawAdPtr,
                                                   DrawAd_OnAdShow onAdShow,
                                                   DrawAd_OnAdClicked onAdClicked,
                                                   DrawAd_OnAdClose onAdClose,
                                                   DrawAd_OnAllAdClose onAllAdClose,
                                                   DrawAd_OnRenderFail onRenderFail,
                                                   DrawAd_OnRenderSuccess onRenderSuccess,
                                                   int context) {
    ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd *)drawAdPtr;
    drawAd.onAdShow = onAdShow;
    drawAd.onAdClicked = onAdClicked;
    drawAd.onAdClose = onAdClose;
    drawAd.onAllAdClose = onAllAdClose;
    drawAd.onRenderFail = onRenderFail;
    drawAd.onRenderSuccess = onRenderSuccess;
    drawAd.interactionContext = context;
}

void UnionPlatform_DrawAd_ShowNativeAd(void* drawAdPtr,
                                         int index,
                                         float x,
                                         float y) {
    ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd *)drawAdPtr;
    CGFloat originX = x/[UIScreen mainScreen].scale;
    CGFloat originY = y/[UIScreen mainScreen].scale;
//    index = 0; // 3510???????????????????????????0
    if (index < drawAd.drawAdViews.count) {
        ABUDrawAdView *adView = (ABUDrawAdView *)[drawAd.drawAdViews objectAtIndex:index];
        if (adView.hasExpressAdGot) {// ??????????????????
            adView.frame = CGRectMake(originX,originY,CGRectGetWidth(adView.frame),CGRectGetHeight(adView.frame));
        } else {
#warning ?????????????????????????????????????????????????????????
            [drawAd refreshUIWithIndex:index loaction:CGPointMake(originX, originY)];
        }
        [GetAppController().rootViewController.view addSubview:adView];
    }
}

const char* UnionPlatform_DrawAd_GetAdNetworkRitId(void* drawAdPtr,
                                                     int index) {
    ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd*)drawAdPtr;
    if (index < drawAd.drawAdViews.count) {
        ABUDrawAdView *adView = [drawAd.drawAdViews objectAtIndex:index];
        NSString *adNetworkRitId = [adView getShowEcpmInfo].slotID;
        return AutonomousStringCopy([adNetworkRitId UTF8String]);
    }
    return "-10";
}

const char* UnionPlatform_DrawAd_GetAdRitInfoAdnName(void* drawAdPtr,
                                                       int index) {
    ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd*)drawAdPtr;
    if (index < drawAd.drawAdViews.count) {
        ABUDrawAdView *adView = [drawAd.drawAdViews objectAtIndex:index];
        NSString *adnName = adView.getShowEcpmInfo.adnName;
        return AutonomousStringCopy([adnName UTF8String]);
    }
    return "";
}

const char* UnionPlatform_DrawAd_GetPreEcpm(void* drawAdPtr,
                                              int index) {
    ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd*)drawAdPtr;
    if (index < drawAd.drawAdViews.count) {
        ABUDrawAdView *adView = [drawAd.drawAdViews objectAtIndex:index];
        NSString *preEcpm = [adView getShowEcpmInfo].ecpm;
        return AutonomousStringCopy([preEcpm UTF8String]);
    }
    return "-10";
}

void UnionPlatform_DrawAd_CallClose(void* drawAdPtr,
                                    int index) {
    if (drawAdPtr) {
        ABUToUnityDrawAd *drawAd = (__bridge ABUToUnityDrawAd*)drawAdPtr;
        [drawAd removeDrawAd:index];
    }
}

void UnionPlatform_DrawAd_Dispose(void* drawAdPtr) {
    if (drawAdPtr) {
        dispatch_async(dispatch_get_main_queue(), ^{
            ABUToUnityDrawAd *drawAd = (__bridge_transfer ABUToUnityDrawAd*)drawAdPtr;
            drawAd.drawAdsManager.delegate = nil;
            drawAd.drawAdsManager = nil;
            drawAd = nil;
            [drawAd.drawAdViews removeAllObjects];
            [drawAd.notCloseViews removeAllObjects];
        });
    }
}

#if defined (__cplusplus)
}
#endif
