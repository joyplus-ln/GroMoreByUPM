package com.bytedance.ad.sdk.mediation.adapter;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;
import android.widget.FrameLayout;

import com.bytedance.msdk.api.v2.GMAdConstant;
import com.bytedance.msdk.api.v2.ad.custom.GMCustomAdError;
import com.bytedance.msdk.api.v2.ad.custom.banner.GMCustomBannerAdapter;
import com.bytedance.msdk.api.v2.ad.custom.bean.GMCustomServiceConfig;
import com.bytedance.msdk.api.v2.slot.GMAdSlotBanner;
import com.bytedance.sdk.openadsdk.TTAdDislike;
import com.bytedance.sdk.openadsdk.TTAdNative;
import com.bytedance.sdk.openadsdk.TTAdSdk;
import com.bytedance.sdk.openadsdk.TTNativeExpressAd;

import java.util.List;
import java.util.Map;

public class CustomerBannerAdapter extends GMCustomBannerAdapter {

    private static final String TAG = "<Unity Log>";

    private View mBannerView;
    private TTNativeExpressAd mTTAd;

    @Override
    public void load(Context context, GMAdSlotBanner adSlot, GMCustomServiceConfig serviceConfig) {
        Log.i(TAG, " Thread = " + Thread.currentThread().getName() + "   加载到自定义的banner了 serviceConfig = " + serviceConfig);
        TTAdNative adNativeLoader = TTAdSdk.getAdManager().createAdNative(context);
        com.bytedance.sdk.openadsdk.AdSlot.Builder adSlotBuilder = new com.bytedance.sdk.openadsdk.AdSlot.Builder()
                .setCodeId(serviceConfig.getADNNetworkSlotId()) //广告位id
                .setSupportDeepLink(true)
                .setAdCount(1); //请求广告数量为1到3条
        adNativeLoader.loadBannerExpressAd(adSlotBuilder.build(), new TTAdNative.NativeExpressAdListener() {
            @Override
            public void onError(int code, String message) {
                Log.i(TAG, "广告加载失败  code = " + code + " message = " + message);
                callLoadFail(new GMCustomAdError(code, message));
            }

            @Override
            public void onNativeExpressAdLoad(List<TTNativeExpressAd> ads) {
                Log.i(TAG, "广告加载成功了");
                mTTAd = ads.get(0);
                mTTAd.setExpressInteractionListener(new TTNativeExpressAd.AdInteractionListener() {
                    @Override
                    public void onAdDismiss() {
                        Log.i(TAG, "onAdDismiss");
                    }

                    @Override
                    public void onAdClicked(View view, int i) {
                        Log.i(TAG, "onAdClicked");
                        callBannerAdClicked();
                    }

                    @Override
                    public void onAdShow(View view, int i) {
                        Log.i(TAG, "onAdShow");
                        callBannerAdShow();
                    }

                    @Override
                    public void onRenderFail(View view, String s, int i) {
                        Log.i(TAG, "onRenderFail");
                    }

                    @Override
                    public void onRenderSuccess(View view, float v, float v1) {
                        Log.i(TAG, "onRenderSuccess");
                        if (mBannerView instanceof FrameLayout) {
                            removeFromParent(view);
                            ((FrameLayout) mBannerView).addView(view, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
                        }
                    }
                });
                mTTAd.setDislikeCallback((Activity) context, new TTAdDislike.DislikeInteractionCallback() {
                    @Override
                    public void onShow() {
                        Log.i(TAG, "setDislikeCallback onShow");
                    }

                    @Override
                    public void onSelected(int i, String s, boolean b) {
                        Log.i(TAG, "setDislikeCallback onSelected");
                    }

                    @Override
                    public void onCancel() {
                        Log.i(TAG, "setDislikeCallback onCancel");
                        callBannerAdClosed();
                    }
                });
                mTTAd.render();
                mBannerView = new FrameLayout(context);

                if (isClientBidding()) { //bidding广告类型
                    Map<String, Object> extraInfo = mTTAd.getMediaExtraInfo();
                    //设置cpm
                    double cpm = 0;
                    if (extraInfo != null) {
                        cpm = TTNumberUtil.getValue(extraInfo.get("price"));
                    }
                    callLoadSuccess(cpm);  //bidding广告成功回调，回传竞价广告价格
                } else {
                    callLoadSuccess();
                }
            }
        });
    }

    private void removeFromParent(View view) {
        if (view != null) {
            ViewParent vp = view.getParent();
            if (vp instanceof ViewGroup) {
                ((ViewGroup) vp).removeView(view);
            }
        }
    }

    @Override
    public View getAdView() {
        return mBannerView;
    }

    @Override
    public void onPause() {
        super.onPause();
        Log.i(TAG, "onPause");
    }

    @Override
    public void onResume() {
        super.onResume();
        Log.i(TAG, "onResume");
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.i(TAG, "onDestroy");
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
