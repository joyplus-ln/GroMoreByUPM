package com.bytedance.ad.sdk.mediation.adapter;

import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.bytedance.msdk.api.v2.GMAdConstant;
import com.bytedance.msdk.api.v2.ad.custom.GMCustomAdError;
import com.bytedance.msdk.api.v2.ad.custom.bean.GMCustomServiceConfig;
import com.bytedance.msdk.api.v2.ad.custom.fullvideo.GMCustomFullVideoAdapter;
import com.bytedance.msdk.api.v2.slot.GMAdSlotFullVideo;
import com.bytedance.sdk.openadsdk.TTAdNative;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTFullScreenVideoAd;

import java.util.Map;

public class CustomerFullVideoAdapter extends GMCustomFullVideoAdapter {

    private static final String TAG = "<Unity Log>";

    private TTFullScreenVideoAd mTTFullScreenVideoAd;
    private boolean isLoadSuccess;

    @Override
    public void load(Context context, GMAdSlotFullVideo adSlot, GMCustomServiceConfig serviceConfig) {
        Log.i(TAG, " Thread = " + Thread.currentThread().getName() + "   加载到自定义的banner了 serviceConfig = " + serviceConfig);
        TTAdNative adNativeLoader = TTAdSdk.getAdManager().createAdNative(context);
        com.bytedance.sdk.openadsdk.AdSlot.Builder adSlotBuilder = new com.bytedance.sdk.openadsdk.AdSlot.Builder()
                .setCodeId(serviceConfig.getADNNetworkSlotId()) //广告位id
                .setSupportDeepLink(true)
                .setAdCount(1); //请求广告数量为1到3条
        adNativeLoader.loadFullScreenVideoAd(adSlotBuilder.build(), new TTAdNative.FullScreenVideoAdListener() {
            @Override
            public void onError(int code, String message) {
                Log.i(TAG, "广告加载失败  code = " + code + " message = " + message);
                callLoadFail(new GMCustomAdError(code, message));
            }

            @Override
            public void onFullScreenVideoAdLoad(TTFullScreenVideoAd ttFullScreenVideoAd) {
                Log.i(TAG, "onFullScreenVideoAdLoad");
                mTTFullScreenVideoAd = ttFullScreenVideoAd;
                isLoadSuccess = true;
                mTTFullScreenVideoAd.setFullScreenVideoAdInteractionListener(new TTFullScreenVideoAd.FullScreenVideoAdInteractionListener() {
                    @Override
                    public void onAdShow() {
                        Log.i(TAG, "onAdShow");
                        callFullVideoAdShow();
                    }

                    @Override
                    public void onAdVideoBarClick() {
                        Log.i(TAG, "onAdVideoBarClick");
                        callFullVideoAdClick();
                    }

                    @Override
                    public void onAdClose() {
                        Log.i(TAG, "onAdClose");
                        callFullVideoAdClosed();
                    }

                    @Override
                    public void onVideoComplete() {
                        Log.i(TAG, "onVideoComplete");
                        callFullVideoComplete();
                    }

                    @Override
                    public void onSkippedVideo() {
                        Log.i(TAG, "onSkippedVideo");
                        callFullVideoSkippedVideo();
                    }
                });


                if (isClientBidding()) {//bidding广告类型
                    Map<String, Object> extraInfo = mTTFullScreenVideoAd.getMediaExtraInfo();
                    double cpm = 0;
                    //设置cpm
                    if (extraInfo != null) {
                        cpm = TTNumberUtil.getValue(extraInfo.get("price"));
                    }
                    callLoadSuccess(cpm);  //bidding广告成功回调，回传竞价广告价格
                } else {
                    callLoadSuccess();//普通广告成功回调
                }
            }

            @Override
            public void onFullScreenVideoCached() {
                Log.i(TAG, "onFullScreenVideoCached 1111");
            }

            @Override
            public void onFullScreenVideoCached(TTFullScreenVideoAd ttFullScreenVideoAd) {
                Log.i(TAG, "onFullScreenVideoCached 2222");
                isLoadSuccess = true;
                callAdVideoCache();
            }
        });
    }

    @Override
    public void showAd(Activity activity) {
        Log.i(TAG, "自定义的showAd");
        if (mTTFullScreenVideoAd != null) {
            mTTFullScreenVideoAd.showFullScreenVideoAd(activity);
        }
    }

    @Override
    public boolean isReady() {
        return isLoadSuccess;
    }

    /**
     * 是否clientBidding广告
     *
     * @return
     */
    public boolean isClientBidding() {
        return getBiddingType() == GMAdConstant.AD_TYPE_CLIENT_BIDING;
    }
}
