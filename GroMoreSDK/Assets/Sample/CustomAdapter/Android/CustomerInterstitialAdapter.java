package com.bytedance.ad.sdk.mediation.adapter;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.View;

import com.bytedance.msdk.api.v2.GMAdConstant;
import com.bytedance.msdk.api.v2.ad.custom.GMCustomAdError;
import com.bytedance.msdk.api.v2.ad.custom.bean.GMCustomServiceConfig;
import com.bytedance.msdk.api.v2.ad.custom.interstitial.GMCustomInterstitialAdapter;
import com.bytedance.msdk.api.v2.slot.GMAdSlotInterstitial;
import com.bytedance.sdk.openadsdk.TTAdNative;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTNativeExpressAd;

import java.util.List;
import java.util.Map;

public class CustomerInterstitialAdapter extends GMCustomInterstitialAdapter {

    private static final String TAG = "<Unity Log>";

    private TTNativeExpressAd mTTNativeExpressAd;
    private boolean isLoadSuccess;

    @Override
    public void load(Context context, GMAdSlotInterstitial adSlot, GMCustomServiceConfig serviceConfig) {
        Log.i(TAG, " Thread = " + Thread.currentThread().getName() + "   加载到自定义的插屏了 serviceConfig = " + serviceConfig);
        TTAdNative adNativeLoader = TTAdSdk.getAdManager().createAdNative(context);
        com.bytedance.sdk.openadsdk.AdSlot.Builder adSlotBuilder = new com.bytedance.sdk.openadsdk.AdSlot.Builder()
                .setCodeId(serviceConfig.getADNNetworkSlotId()) //广告位id
                .setSupportDeepLink(true)
                .setAdCount(1); //请求广告数量为1到3条
        adNativeLoader.loadInteractionExpressAd(adSlotBuilder.build(), new TTAdNative.NativeExpressAdListener() {
            @Override
            public void onError(int code, String message) {
                Log.i(TAG, "广告加载失败  code = " + code + " message = " + message);
                callLoadFail(new GMCustomAdError(code, message));
            }

            @Override
            public void onNativeExpressAdLoad(List<TTNativeExpressAd> ads) {
                Log.i(TAG, "广告加载成功了");
                mTTNativeExpressAd = ads.get(0);
                mTTNativeExpressAd.setExpressInteractionListener(new TTNativeExpressAd.AdInteractionListener() {
                    @Override
                    public void onAdDismiss() {
                        Log.i(TAG, "onAdDismiss");
                        callInterstitialClosed();
                    }

                    @Override
                    public void onAdClicked(View view, int i) {
                        Log.i(TAG, "onAdClicked");
                        callInterstitialAdClick();
                    }

                    @Override
                    public void onAdShow(View view, int i) {
                        Log.i(TAG, "onAdShow");
                        callInterstitialShow();
                    }

                    @Override
                    public void onRenderFail(View view, String s, int i) {
                        Log.i(TAG, "onRenderFail");
                    }

                    @Override
                    public void onRenderSuccess(View view, float v, float v1) {
                        isLoadSuccess = true;
                        Log.i(TAG, "onRenderSuccess");
                    }
                });
                mTTNativeExpressAd.render();

                if (isClientBidding()) {//bidding广告类型
                    Map<String, Object> extraInfo = mTTNativeExpressAd.getMediaExtraInfo();
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
        });
    }

    @Override
    public void showAd(Activity activity) {
        Log.i(TAG, "自定义的showAd");
        if (mTTNativeExpressAd != null) {
            mTTNativeExpressAd.showInteractionExpressAd(activity);
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
