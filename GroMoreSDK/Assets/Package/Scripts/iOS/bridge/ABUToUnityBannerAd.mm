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

typedef void(*BannerAd_OnAdLoad)(void* bannerAd, float width, float height, int context);
typedef void(*BannerAd_OnError)(int errCode,const char* errMsg,int context);
typedef void(*BannerAd_OnAdShow)(int context);
typedef void(*BannerAd_OnAdClick)(int context);
typedef void(*BannerAd_OnAdClose)(int context);
typedef void(*BannerAd_OnWaterfallRitFillFail)(const char* fillFailMessageInfo, int context);

@interface ABUToUnityBannerAd : NSObject<ABUBannerAdDelegate>

@property(nonatomic, strong) ABUBannerAd *bannerAd;
@property(nonatomic, strong) UIView *bannerView;
@property (nonatomic, assign) float adWidth;
@property (nonatomic, assign) float adHeight;

@property (nonatomic, assign) int loadContext;
@property (nonatomic, assign) int interactionContext;

@property (nonatomic, assign) BannerAd_OnAdLoad onLoad;
@property (nonatomic, assign) BannerAd_OnError onLoadError;
@property (nonatomic, assign) BannerAd_OnAdShow onAdShow;
@property (nonatomic, assign) BannerAd_OnAdClick onAdClick;
@property (nonatomic, assign) BannerAd_OnAdClose onAdClose;
@property (nonatomic, assign) BannerAd_OnWaterfallRitFillFail onWaterfallRitFillFail;

@end


@implementation ABUToUnityBannerAd

- (void)dealloc {
    _bannerAd = nil;
}

#pragma mark <---ABUBannerAdViewDelegate--->
/**
 This method is called when bannerAdView ad slot loaded successfully.
 @param bannerView : view for bannerView
 */
- (void)bannerAdDidLoad:(ABUBannerAd *_Nonnull)bannerAd bannerView:(UIView *)bannerView {
    _bannerView = bannerView;
    _bannerView.frame = CGRectMake(0, CGRectGetHeight([UIScreen mainScreen].bounds)-_adHeight, _adWidth, _adHeight);
    if (self.onLoad) {
        self.onLoad((__bridge void*)self, CGRectGetWidth(_bannerView.frame), CGRectGetHeight(_bannerView.frame), self.loadContext);
    }
    if (self.onWaterfallRitFillFail && self.bannerAd.waterfallFillFailMessages.count > 0) {
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self.bannerAd.waterfallFillFailMessages options:0 error:nil];
        NSString *strJson = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.onWaterfallRitFillFail([strJson UTF8String], self.interactionContext);
    }
}

/// 广告加载成功后为「混用的信息流自渲染广告」时会触发该回调，提供给开发者自渲染的时机
/// @param bannerAd 广告操作对象
/// @param canvasView 携带物料的画布，需要对其内部提供的物料及控件做布局及设置UI
/// @warning 轮播开启时，每次轮播到自渲染广告均会触发该回调，并且canvasView为其他回调中bannerView的子控件
- (void)bannerAdNeedLayoutUI:(ABUBannerAd *)bannerAd canvasView:(ABUCanvasView *)canvasView {
#warning 开发者如有自渲染布局需求，可在此处处理
    [canvasView exampleLayoutWithFrame:CGRectMake(0, 0, _adWidth, _adHeight)];
}

/**
 This method is called when bannerAdView ad slot failed to load.
 @param error : the reason of error
 */
- (void)bannerAd:(ABUBannerAd *_Nonnull)bannerAd didLoadFailWithError:(NSError *_Nullable)error {
    if (self.onLoadError) {
        NSString *errMsg = @"";
        if (error.localizedDescription) {
            errMsg = error.localizedDescription;
        }
        self.onLoadError((int)error.code, [errMsg UTF8String], self.loadContext);
    }
}

/**
 This method is called when bannerAdView ad slot success to show.
 */
- (void)bannerAdDidBecomeVisible:(ABUBannerAd *)bannerAd bannerView:(UIView *)bannerView {
    if (self.onAdShow) {
        self.onAdShow(self.interactionContext);
    }
}

/**
 * This method is called when FullScreen modal has been presented.Include appstore jump.
 *  弹出详情广告页
 */
- (void)bannerAdWillPresentFullScreenModal:(ABUBannerAd *_Nonnull)ABUBannerAd bannerView:(UIView *)bannerView {
    
}

/**
 ** This method is called when FullScreen modal has been dismissed.Include appstore jump.
 *  详情广告页将要关闭
 */
- (void)bannerAdWillDismissFullScreenModal:(ABUBannerAd *_Nonnull)ABUBannerAd bannerView:(UIView *)bannerView {
    
}

/**
 This method is called when bannerAdView is clicked.
 */
- (void)bannerAdDidClick:(ABUBannerAd *_Nonnull)ABUBannerAd bannerView:(UIView *)bannerView {
    if (self.onAdClick) {
        self.onAdClick(self.interactionContext);
    }
}

/**
 This method is called when the user clicked dislike button and chose dislike reasons.
 @param filterwords : the array of reasons for dislike.
 */

- (void)bannerAdDidClosed:(ABUBannerAd *_Nonnull)ABUBannerAd bannerView:(UIView *)bannerView dislikeWithReason:(NSArray<ABUDislikeWords *> *_Nullable)filterwords {
    // 移除
    [UIView animateWithDuration:0.25 animations:^{
        _bannerView.alpha = 0;
    } completion:^(BOOL finished) {
        [self.bannerView removeFromSuperview];
        self.bannerView = nil;
    }];
    
    if (self.onAdClose) {
        self.onAdClose(self.interactionContext);
    }
}

@end

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_BannerAd_Load(const char* unitID,
                                 float width,
                                 float height,
                                 int autoRefreshTime,
                                 bool muted,
                                 BannerAd_OnError onLoadError,
                                 BannerAd_OnAdLoad onLoad,
                                 BannerAd_OnWaterfallRitFillFail onWaterfallRitFillFail,
                                 int context) {
    ABUToUnityBannerAd *instance = [[ABUToUnityBannerAd alloc] init];
    instance.adWidth = [UIScreen mainScreen].bounds.size.width;
    instance.adHeight = instance.adWidth * height / width;
    instance.bannerAd = [[ABUBannerAd alloc] initWithAdUnitID:[NSString stringWithUTF8String:unitID? :""] rootViewController:GetAppController().rootViewController adSize:CGSizeMake(instance.adWidth, instance.adHeight)];
    instance.bannerAd.delegate = instance;
//    instance.bannerAd.imageOrVideoSize =
    instance.bannerAd.startMutedIfCan = muted;
    instance.onLoad = onLoad;
    instance.onLoadError = onLoadError;
    instance.onWaterfallRitFillFail = onWaterfallRitFillFail;
    instance.loadContext = context;
    if ([ABUAdSDKManager configDidLoad]) {
        [instance.bannerAd loadAdData];
    } else {
        __weak ABUBannerAd *wBannerAd = instance.bannerAd;
        [ABUAdSDKManager addConfigLoadSuccessObserver:instance withAction:^(id  _Nonnull observer) {
            __strong ABUBannerAd *sbannerAd = wBannerAd;
            [sbannerAd loadAdData];
        }];
    }
    (__bridge_retained void*)instance;
}

void UnionPlatform_BannerAd_SetInteractionListener(void* bannerAdPtr,
                                                   BannerAd_OnAdShow onAdShow,
                                                   BannerAd_OnAdClick onAdClick,
                                                   BannerAd_OnAdClose onAdClose,
                                                   int context) {
    ABUToUnityBannerAd *bannerAd = (__bridge ABUToUnityBannerAd*)bannerAdPtr;
    bannerAd.onAdShow = onAdShow;
    bannerAd.onAdClick = onAdClick;
    bannerAd.onAdClose = onAdClose;
    bannerAd.interactionContext = context;
}

void UnionPlatform_BannerAd_Show(void* bannerAdPtr,
                                 float originX,
                                 float originY) {
    ABUToUnityBannerAd *bannerAd = (__bridge ABUToUnityBannerAd*)bannerAdPtr;
    CGFloat newX = originX/[UIScreen mainScreen].scale;
    CGFloat newY = originY/[UIScreen mainScreen].scale;
    bannerAd.bannerView.frame = CGRectMake(newX, newY, bannerAd.bannerView.frame.size.width, bannerAd.bannerView.frame.size.height);
    [GetAppController().rootViewController.view addSubview:bannerAd.bannerView];
}

const char* UnionPlatform_BannerAd_GetAdRitInfoAdnName(void* bannerAdPtr) {
    ABUToUnityBannerAd *bannerAd = (__bridge ABUToUnityBannerAd*)bannerAdPtr;
    NSString *adnName = bannerAd.bannerAd.getShowEcpmInfo.adnName;
    return AutonomousStringCopy([adnName UTF8String]);
}

const char* UnionPlatform_BannerAd_GetAdNetworkRitId(void* bannerAdPtr) {
    ABUToUnityBannerAd* bannerAd = (__bridge ABUToUnityBannerAd*)bannerAdPtr;
    NSString *adNetworkRitId = [bannerAd.bannerAd getShowEcpmInfo].slotID;
    return AutonomousStringCopy([adNetworkRitId UTF8String]);
}

const char* UnionPlatform_BannerAd_GetPreEcpm(void* bannerAdPtr) {
    ABUToUnityBannerAd* bannerAd = (__bridge ABUToUnityBannerAd*)bannerAdPtr;
    NSString *preEcpm = [bannerAd.bannerAd getShowEcpmInfo].ecpm;
    return AutonomousStringCopy([preEcpm UTF8String]);
}

void UnionPlatform_BannerAd_Dispose(void* bannerAdPtr) {
    if (bannerAdPtr) {
        dispatch_async(dispatch_get_main_queue(), ^{
            ABUToUnityBannerAd *bannerAd = (__bridge_transfer ABUToUnityBannerAd*)bannerAdPtr;
            bannerAd.bannerAd.delegate = nil;
            [bannerAd.bannerView removeFromSuperview];
            [bannerAd.bannerAd destory];
            bannerAd.bannerView = nil;
            bannerAd = nil;
        });
    }
}

#if defined (__cplusplus)
}
#endif
