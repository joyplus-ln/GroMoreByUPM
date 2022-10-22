package com.bytedance.ad.sdk.mediation.adapter;

import android.content.Context;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;

import com.bytedance.msdk.api.v2.GMAdConstant;
import com.bytedance.msdk.api.v2.ad.custom.GMCustomAdError;
import com.bytedance.msdk.api.v2.ad.custom.bean.GMCustomServiceConfig;
import com.bytedance.msdk.api.v2.ad.custom.splash.GMCustomSplashAdapter;
import com.bytedance.msdk.api.v2.slot.GMAdSlotSplash;
import com.bytedance.sdk.openadsdk.TTAdNative;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTSplashAd;

import java.util.Map;

public class CustomerSplashAdapter extends GMCustomSplashAdapter {

    private static final String TAG = "<Unity Log>";

    private TTSplashAd mTTSplashAd;

    @Override
    public void load(Context context, GMAdSlotSplash adSlot, GMCustomServiceConfig serviceConfig) {
        Log.i(TAG, " Thread = " + Thread.currentThread().getName() + "   加载到自定义的开屏了 serviceConfig = " + serviceConfig);
        TTAdNative adNativeLoader = TTAdSdk.getAdManager().createAdNative(context);
        com.bytedance.sdk.openadsdk.AdSlot.Builder adSlotBuilder = new com.bytedance.sdk.openadsdk.AdSlot.Builder()
                .setCodeId(serviceConfig.getADNNetworkSlotId()) //广告位id
                .setSupportDeepLink(true)
                .setExpressViewAcceptedSize(1080,1920)
                .setAdCount(1); //请求广告数量为1到3条
        adNativeLoader.loadSplashAd(adSlotBuilder.build(), new TTAdNative.SplashAdListener() {
            @Override
            public void onError(int i, String s) {
                Log.i(TAG, "onError");
                callLoadFail(new GMCustomAdError(i, s));
            }

            @Override
            public void onTimeout() {
                Log.i(TAG, "onTimeout");
            }

            @Override
            public void onSplashAdLoad(TTSplashAd ttSplashAd) {
                Log.i(TAG, "onSplashAdLoad");
                mTTSplashAd = ttSplashAd;
                mTTSplashAd.setSplashInteractionListener(new TTSplashAd.AdInteractionListener() {
                    @Override
                    public void onAdClicked(View view, int i) {
                        Log.i(TAG, "onAdClicked");
                        callSplashAdClicked();
                    }

                    @Override
                    public void onAdShow(View view, int i) {
                        Log.i(TAG, "onAdShow");
                        callSplashAdShow();
                    }

                    @Override
                    public void onAdSkip() {
                        Log.i(TAG, "onAdSkip");
                        callSplashAdSkip();
                    }

                    @Override
                    public void onAdTimeOver() {
                        Log.i(TAG, "onAdTimeOver");
                        callSplashAdDismiss();
                    }
                });
                if (isClientBidding()) {//bidding广告类型
                    Map<String, Object> extraInfo = mTTSplashAd.getMediaExtraInfo();
                    double cpm = 0;
                    //设置cpm
                    if (extraInfo != null) {
                        cpm = TTNumberUtil.getValue(extraInfo.get("price"));
                    }
                    callLoadSuccess(cpm);//bidding广告成功回调，回传竞价广告价格
                } else {
                    callLoadSuccess();//普通广告成功回调
                }
            }
        }, adSlot.getTimeOut());

    }

    @Override
    public void showAd(ViewGroup container) {
        if (mTTSplashAd != null) {
            View view = mTTSplashAd.getSplashView();
            if (view != null) {
                ViewParent parent = view.getParent();
                if (parent instanceof ViewGroup) {
                    ((ViewGroup) parent).removeView(view);
                }
                container.removeAllViews();
                container.addView(view);
            }
        }
    }


    public boolean isReady() {
        return mTTSplashAd != null;
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
