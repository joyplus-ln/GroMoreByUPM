package com.bytedance.ad.sdk.mediation;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.telephony.TelephonyManager;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;

import com.bytedance.msdk.adapter.gdt.GdtNetworkRequestInfo;
import com.bytedance.msdk.adapter.pangle.PangleNetworkRequestInfo;
import com.bytedance.msdk.api.AdError;
import com.bytedance.msdk.api.AdmobNativeAdOptions;
import com.bytedance.msdk.api.BaiduExtraOptions;
import com.bytedance.msdk.api.GDTExtraOption;
import com.bytedance.msdk.api.TTNetworkRequestInfo;
import com.bytedance.msdk.api.TTVideoOption;
import com.bytedance.msdk.api.reward.RewardItem;
import com.bytedance.msdk.api.v2.GMLocation;
import com.bytedance.msdk.api.v2.ad.AdUtils;
import com.bytedance.msdk.api.v2.ad.banner.GMBannerAd;
import com.bytedance.msdk.api.fullVideo.TTFullVideoAd;
import com.bytedance.msdk.api.fullVideo.TTFullVideoAdListener;
import com.bytedance.msdk.api.interstitial.TTInterstitialAd;
import com.bytedance.msdk.api.nativeAd.TTUnifiedNativeAd;
import com.bytedance.msdk.api.reward.TTRewardAd;
import com.bytedance.msdk.api.reward.TTRewardedAdListener;
import com.bytedance.msdk.api.splash.TTSplashAd;
import com.bytedance.msdk.api.AdSlot;
import com.bytedance.msdk.api.v2.GMAdSize;
import com.bytedance.msdk.api.v2.ad.banner.GMBannerAdLoadCallback;
import com.bytedance.msdk.api.v2.ad.banner.GMNativeAdInfo;
import com.bytedance.msdk.api.v2.ad.banner.GMNativeToBannerListener;
import com.bytedance.msdk.api.v2.ad.draw.GMDrawAd;
import com.bytedance.msdk.api.v2.ad.draw.GMDrawAdLoadCallback;
import com.bytedance.msdk.api.v2.ad.draw.GMDrawExpressAdListener;
import com.bytedance.msdk.api.v2.ad.draw.GMUnifiedDrawAd;
import com.bytedance.msdk.api.v2.ad.interstitialFull.GMInterstitialFullAd;
import com.bytedance.msdk.api.v2.ad.interstitialFull.GMInterstitialFullAdListener;
import com.bytedance.msdk.api.v2.ad.interstitialFull.GMInterstitialFullAdLoadCallback;
import com.bytedance.msdk.api.v2.slot.GMAdSlotBanner;
import com.bytedance.msdk.api.TTAdConstant;
import com.bytedance.msdk.api.TTMediationAdSdk;
import com.bytedance.msdk.api.v2.GMPrivacyConfig;
import com.bytedance.msdk.api.v2.slot.GMAdSlotDraw;
import com.bytedance.msdk.api.v2.slot.GMAdSlotInterstitialFull;
import com.bytedance.msdk.api.v2.slot.paltform.GMAdSlotGDTOption;
import com.bytedance.sdk.openadsdk.TTRewardVideoAd;


import java.util.HashMap;
import java.util.Map;

import android.util.Log;

import org.json.JSONObject;


public class AdManager {
    // ritscene类型的key，value为ABURitSceneType；用于激励，全屏展示
    public static String ABUShowExtroInfoKeySceneType = "ABUShowExtroInfoKeySceneType";
    // ritscene描述的key，当ABUShowExtroInfoKeySceneType=BURitSceneType_custom时需要设置该key；用于激励，全屏展示
    public static String ABUShowExtroInfoKeySceneDescription = "ABUShowExtroInfoKeySceneDescription";

    public static String BURitSceneType_custom = "BURitSceneType_custom";
    public static String ABURitSceneType_home_open_bonus = "ABURitSceneType_home_open_bonus";
    public static String ABURitSceneType_home_svip_bonus = "ABURitSceneType_home_svip_bonus";
    public static String ABURitSceneType_home_get_props = "ABURitSceneType_home_get_props";
    public static String ABURitSceneType_home_try_props = "ABURitSceneType_home_try_props";
    public static String ABURitSceneType_home_get_bonus = "ABURitSceneType_home_get_bonus";
    public static String ABURitSceneType_home_gift_bonus = "ABURitSceneType_home_gift_bonus";
    public static String ABURitSceneType_game_start_bonus = "ABURitSceneType_game_start_bonus";
    public static String ABURitSceneType_game_reduce_waiting = "ABURitSceneType_game_reduce_waiting";
    public static String ABURitSceneType_game_more_opportunities = "ABURitSceneType_game_more_opportunities";
    public static String ABURitSceneType_game_finish_rewards = "ABURitSceneType_game_finish_rewards";
    public static String ABURitSceneType_game_gift_bonus = "ABURitSceneType_game_gift_bonus";

    private static volatile AdManager sManager;

    public static AdManager getAdManager() {
        if (sManager == null) {
            synchronized (AdManager.class) {
                if (sManager == null) {
                    sManager = new AdManager();
                }
            }
        }
        return sManager;
    }

    public void updatePrivacyConfig(final boolean isCanUseLocation, final boolean isCanUsePhoneState,
                                    final boolean isCanUseWifiState, final boolean isCanUseWriteExternal,
                                    final boolean isLimitPersonalAds, final boolean limitProgrammaticAds,
                                    final boolean notAdult, final double longitude, final double latitude) {
        TTMediationAdSdk.updatePrivacyConfig(new GMPrivacyConfig() {
            @Override
            public boolean isCanUseLocation() {
                return isCanUseLocation;
            }

            @Override
            public boolean isCanUsePhoneState() {
                return isCanUsePhoneState;
            }

            @Override
            public boolean isCanUseWifiState() {
                return isCanUseWifiState;
            }

            @Override
            public boolean isCanUseWriteExternal() {
                return isCanUseWriteExternal;
            }

            @Override
            public boolean isLimitPersonalAds() {
                return isLimitPersonalAds;
            }

            @Override
            public boolean isAdult() {
                return !notAdult;
            }

            @Override
            public boolean isProgrammaticRecommend() {
                return !limitProgrammaticAds;
            }

            @Override
            public GMLocation getTTLocation() {
                GMLocation location = new GMLocation(latitude, longitude);
                return location;
            }
        });
    }

    public TTUnifiedNativeAd getFeedAd(Context context, String adUnitId) {
        TTUnifiedNativeAd feedAdNative = new TTUnifiedNativeAd(context, adUnitId);//模板视频
        return feedAdNative;
    }

    public GMBannerAd getBannerAd(Activity context, String adUnitId) {
        GMBannerAd bannerAd = new GMBannerAd(context, adUnitId);
        return bannerAd;
    }

    public void loadBannerAd(Activity activity, GMBannerAd gmBannerAd, AdSlot adSlot, int refreshTime, GMBannerAdLoadCallback adBannerLoadCallBack) {
        if (gmBannerAd == null || adSlot == null) {
            return;
        }
        GMAdSlotBanner slotBanner = new GMAdSlotBanner.Builder()
                .setBannerSize(GMAdSize.BANNER_CUSTOME)
                .setImageAdSize(adSlot.getImgAcceptedWidth(), adSlot.getImgAcceptedHeight())// GMAdSize.BANNER_CUSTOME可以调用setImageAdSize设置大小
                .setRefreshTime(refreshTime)
                .setAllowShowCloseBtn(true)//如果广告本身允许展示关闭按钮，这里设置为true就是展示。注：目前只有mintegral支持。
                .build();
        gmBannerAd.setNativeToBannerListener(new GMNativeToBannerListener(){
            @Override
            public View getGMBannerViewFromNativeAd(GMNativeAdInfo ad) {
                return BannerToNativeHelper.getBannerViewFromNativeAd(activity, ad);
            }
        });
        gmBannerAd.loadAd(slotBanner, adBannerLoadCallBack);
    }

    public TTRewardAd getNormalRewardAd(Activity context, String adUnitId) {
        TTRewardAd ttRewardAd = new TTRewardAd(context, adUnitId);
        return ttRewardAd;
    }

    public void showRewardAd(TTRewardAd ttRewardAd, Activity activity, Map<String, String> extras, TTRewardedAdListener adRewardedListener) {
        Log.e("showRewardAd", "<Unity Log> showRewardAd info:ttRewardAd.hashCode()=" + ttRewardAd.hashCode());
        Map<TTAdConstant.GroMoreExtraKey, Object> mExtras = null;
        if (extras != null) {
            boolean isCustomizeScenes = false;
            mExtras = new HashMap<>();
            for (Map.Entry<String, String> entry : extras.entrySet()) {
                if (ABUShowExtroInfoKeySceneType.equals(entry.getKey())) {
                    isCustomizeScenes = BURitSceneType_custom.equals(entry.getValue());
                    if (isCustomizeScenes) {
                        mExtras.put(TTAdConstant.GroMoreExtraKey.RIT_SCENES, TTAdConstant.GroMoreRitScenes.CUSTOMIZE_SCENES);
                    } else {
                        mExtras.put(TTAdConstant.GroMoreExtraKey.RIT_SCENES, convertRitScenes(entry.getValue()));
                    }

                } else if (ABUShowExtroInfoKeySceneDescription.equals(entry.getKey())) {
                    mExtras.put(TTAdConstant.GroMoreExtraKey.CUSTOMIZE_RIT_SCENES, entry.getValue());
                }
            }
        }
        Log.e("showRewardAd", "<Unity Log> showRewardAd info:activity.hashCode()=" + activity.hashCode());

        ttRewardAd.showRewardAd(activity, mExtras, adRewardedListener);
    }

    private TTAdConstant.GroMoreRitScenes convertRitScenes(String value) {
        if (ABURitSceneType_home_open_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_OPEN_BONUS;
        } else if (ABURitSceneType_home_svip_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_SVIP_BONUS;
        } else if (ABURitSceneType_home_get_props.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_GET_PROPS;
        } else if (ABURitSceneType_home_try_props.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_TRY_PROPS;
        } else if (ABURitSceneType_home_get_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_GET_BONUS;
        } else if (ABURitSceneType_home_gift_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.HOME_GIFT_BONUS;
        } else if (ABURitSceneType_game_start_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.GAME_START_BONUS;
        } else if (ABURitSceneType_game_reduce_waiting.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.GAME_REDUCE_WAITING;
        } else if (ABURitSceneType_game_more_opportunities.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.GAME_MORE_KLLKRTUNITIES;
        } else if (ABURitSceneType_game_finish_rewards.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.GAME_FINISH_REWARDS;
        } else if (ABURitSceneType_game_gift_bonus.equals(value)) {
            return TTAdConstant.GroMoreRitScenes.GAME_GIFT_BONUS;
        } else {
            return TTAdConstant.GroMoreRitScenes.CUSTOMIZE_SCENES;
        }
    }

    public TTFullVideoAd getFullVideoAd(Activity context, String adUnitId) {
        TTFullVideoAd ttFullVideoAd = new TTFullVideoAd(context, adUnitId);
        return ttFullVideoAd;
    }

    public void showFullAd(TTFullVideoAd ttFullVideoAd, Activity activity, Map<String, String> extras, TTFullVideoAdListener ttFullVideoAdListener) {
        Log.e("showFullAd", "<Unity Log> showFullAd info:ttFullVideoAd.hashCode()=" + ttFullVideoAd.hashCode() + ";extras=" + extras.toString());
        Map<TTAdConstant.GroMoreExtraKey, Object> mExtras = null;
        if (extras != null) {
            boolean isCustomizeScenes = false;
            mExtras = new HashMap<>();
            for (Map.Entry<String, String> entry : extras.entrySet()) {
                if (ABUShowExtroInfoKeySceneType.equals(entry.getKey())) {
                    isCustomizeScenes = BURitSceneType_custom.equals(entry.getValue());
                    if (isCustomizeScenes) {
                        mExtras.put(TTAdConstant.GroMoreExtraKey.RIT_SCENES, TTAdConstant.GroMoreRitScenes.CUSTOMIZE_SCENES);
                    } else {
                        mExtras.put(TTAdConstant.GroMoreExtraKey.RIT_SCENES, convertRitScenes(entry.getValue()));
                    }

                } else if (ABUShowExtroInfoKeySceneDescription.equals(entry.getKey())) {
                    mExtras.put(TTAdConstant.GroMoreExtraKey.CUSTOMIZE_RIT_SCENES, entry.getValue());
                }
            }
        }
        Log.e("showFullAd", "<Unity Log> showFullAd info:activity.hashCode()=" + activity.hashCode() + ";mExtras=" + mExtras.toString());
        ttFullVideoAd.showFullAd(activity, mExtras, ttFullVideoAdListener);
    }

    public TTInterstitialAd getInterstitialAd(Activity context, String adUnitId) {
        TTInterstitialAd ttInterstitialAd = new TTInterstitialAd(context, adUnitId);
        return ttInterstitialAd;
    }

    public TTSplashAd getSplashAd(Activity context, String adUnitId) {
        TTSplashAd ttSplashAd = new TTSplashAd(context, adUnitId);
        return ttSplashAd;
    }

    public TTNetworkRequestInfo getSplashPangolinNetworkRequestInfo(String appId, String ritID) {
        //选择穿山甲兜底
        //Pangolin
        TTNetworkRequestInfo ttNetworkRequestInfo = new PangleNetworkRequestInfo(appId, ritID);
        return ttNetworkRequestInfo;
    }

    public TTNetworkRequestInfo getSplashGdtNetworkRequestInfo(String appId, String ritID) {
        //选择广点通兜底
        TTNetworkRequestInfo ttNetworkRequestInfo = new GdtNetworkRequestInfo(appId, ritID);
        return ttNetworkRequestInfo;
    }

    public ViewGroup getRootLayout(Activity context) {
        if (context == null) {
            return null;
        }
        final ViewGroup rootGroup = context.findViewById(android.R.id.content);
        return rootGroup;
    }

    //Need to update ui in the UI thread
    public ViewGroup getFrameLayout(Activity context) {
        if (context == null) {
            return null;
        }
        FrameLayout frameLayout = new FrameLayout(context);
        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        layoutParams.gravity = Gravity.CENTER;
        frameLayout.setLayoutParams(layoutParams);
        ViewGroup rootGroup = getRootLayout(context);
        rootGroup.addView(frameLayout);
        return frameLayout;
    }

    //Need to update ui in the UI thread
    public ViewGroup getFrameLayoutForSplash(Activity context) {
        if (context == null) {
            return null;
        }
        FrameLayout frameLayout = new FrameLayout(context);
        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT);
        layoutParams.gravity = Gravity.CENTER;
        frameLayout.setLayoutParams(layoutParams);
        ViewGroup rootGroup = getRootLayout(context);
        rootGroup.addView(frameLayout);
        return frameLayout;
    }

    public void removeViewFromRootView(Activity context, View view) {
        ViewGroup viewGroup = getRootLayout(context);
        if (viewGroup == null || view == null) {
            return;
        }
        viewGroup.removeView(view);
    }

    public AdmobNativeAdOptions getAdmobNativeAdOptions() {
        AdmobNativeAdOptions admobNativeAdOptions = new AdmobNativeAdOptions();
        admobNativeAdOptions.setAdChoicesPlacement(AdmobNativeAdOptions.ADCHOICES_TOP_RIGHT)//The ad selection overlay will appear in the upper right corner.
                .setRequestMultipleImages(true)//The material may contain multiple pictures. If this value is set to true, it means that multiple pictures need to be displayed. This value is set to false (default) and only the first picture is provided.
                .setReturnUrlsForImageAssets(true);//Set to true, the SDK will only provide the value of the Uri field, allowing you to decide whether to download the actual picture, and will not provide the value of the Drawable field.
        return admobNativeAdOptions;
    }

    public TTVideoOption getRewardTTVideoOption(boolean muted) {
        TTVideoOption videoOption = new TTVideoOption.Builder()
                .setMuted(muted)//Incentive ads for all SDKs take effect, except for SDKs that need to be configured on the platform, such as Pangolin SDK
                .setAdmobAppVolume(0f)//Cooperate with Admob's sound size setting [0-1]
                .build();
        return videoOption;
    }

    //Use of feed scene and full screen video scene
    public TTVideoOption getDefaultTTVideoOption() {
        TTVideoOption videoOption = VideoOptionUtil.getTTVideoOption(false);
        return videoOption;
    }

    public TTVideoOption getTTVideoOption(boolean useExpress2IfCanForGDT, boolean muted) {
        TTVideoOption videoOption;
        if (useExpress2IfCanForGDT) {
            videoOption = VideoOptionUtil.getTTVideoOption2(muted);
        } else {
            videoOption = VideoOptionUtil.getTTVideoOption(muted);
        }
        return videoOption;
    }

    public static String getImei(Context context) {
        String mImei = null;
        try {
            TelephonyManager telephonyManager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
            if (telephonyManager != null) {
                mImei = telephonyManager.getDeviceId();
            }
        } catch (Throwable e) {

        }
        return mImei;
    }

    public static void loadDrawAd(Activity context, String adUnitId, AdSlot adSlot, GMDrawAdLoadCallback callback) {
        GMUnifiedDrawAd mGMUnifiedDrawAd = new GMUnifiedDrawAd(context, adUnitId);
        mGMUnifiedDrawAd.loadAd(getAdSlotDraw(adSlot), callback);
    }

    public static void showDrawAd(Activity activity, GMDrawAd mGMDrawAd, GMDrawExpressAdListener listener) {
        if (activity != null) {
            activity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    DrawAdHelper.showAd(activity, mGMDrawAd, listener);
                }
            });
        }
    }

    public static GMAdSlotDraw getAdSlotDraw(AdSlot adSlot) {
        if (adSlot != null) {
            GMAdSlotDraw.Builder builder = new GMAdSlotDraw.Builder()
                    .setBidNotify(adSlot.isBidNotify())
                    .setTestSlotId(adSlot.getTestSlotId());
            for (Map.Entry<String, Object> entry : adSlot.getReuestParam().getExtraObject().entrySet()) {
                builder.setExtraObject(entry.getKey(), entry.getValue());
            }
            TTVideoOption ttVideoOption = adSlot.getTTVideoOption();
            if (ttVideoOption != null) {
                builder.setMuted(ttVideoOption.isMuted());
                GDTExtraOption gdtExtraOption = ttVideoOption.getGDTExtraOption();
                if (gdtExtraOption != null) {
                    GMAdSlotGDTOption.Builder gdtOptionBuilder = gdtExtraOption.getGMGDTExtraOption();
                    gdtOptionBuilder.setNativeAdLogoParams(adSlot.getGdtNativeAdLogoParams());
                    builder.setGMAdSlotGDTOption(gdtOptionBuilder.build());
                }
                BaiduExtraOptions baiduExtraOptions = ttVideoOption.getBaiduExtraOption();
                if (baiduExtraOptions != null) {
                    builder.setGMAdSlotBaiduOption(baiduExtraOptions.getGMBaiduExtra());
                }
            }

            builder.setImageAdSize(adSlot.getImgAcceptedWidth(), adSlot.getImgAcceptedHeight());
            builder.setAdCount(adSlot.getAdCount());
            return builder.build();
        }
        return null;
    }

    public static GMInterstitialFullAd loadInterstitialFullAd(Activity context, String adUnitId, AdSlot adSlot, GMInterstitialFullAdLoadCallback callback) {
        GMInterstitialFullAd interstitialFullAd = new GMInterstitialFullAd(context, adUnitId);
        GMAdSlotInterstitialFull adSlotInterstitialFull = AdUtils.getAdSlotInterstitialFull(adSlot);
        interstitialFullAd.loadAd(adSlotInterstitialFull, callback);
        return interstitialFullAd;
    }

    public static void showInterstitialFullAd(Activity activity, GMInterstitialFullAd ad, GMInterstitialFullAdListener listener) {
        if (ad != null && ad.isReady()) {
            activity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    ad.setAdInterstitialFullListener(listener);
                    ad.showAd(activity);
                }
            });

        } else {
            com.bytedance.msdk.api.TToast.show(activity, "当前广告不满足show的条件");
        }

    }

    @NonNull
    public static String rewardItemToUnityJsonString(@NonNull RewardItem rewardItem) {
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("rewardName", rewardItem.getRewardName());
            jsonObject.put("rewardAmount", rewardItem.getAmount());
            jsonObject.put("verify", rewardItem.rewardVerify());
            Map<String, Object> customData = rewardItem.getCustomData();
            if (customData != null) {
                Object tradeId = customData.get(RewardItem.KEY_GDT_TRANS_ID);
                if (tradeId instanceof String) {
                    jsonObject.put("tradeId", tradeId.toString());
                }

                Object verifyByGroMoreS2S = customData.get(RewardItem.KEY_IS_GROMORE_SERVER_SIDE_VERIFY);
                if (verifyByGroMoreS2S instanceof Boolean) {
                    jsonObject.put("verifyByGroMoreS2S", (boolean) verifyByGroMoreS2S);
                }
                Object adnName = customData.get(RewardItem.KEY_ADN_NAME);
                if (adnName instanceof String) {
                    jsonObject.put("adnName", adnName.toString());
                }
                Object reason = customData.get(RewardItem.KEY_REASON);
                if (reason instanceof String) {
                    jsonObject.put("reason", reason.toString());
                }

                Object errorCode = customData.get(RewardItem.KEY_ERROR_CODE);
                if (errorCode instanceof Integer) {
                    jsonObject.put("errorCode", (int) errorCode);
                }

                Object errorMsg = customData.get(RewardItem.KEY_ERROR_MSG);
                if (errorMsg instanceof String) {
                    jsonObject.put("errorMsg", errorMsg.toString());
                }
                Object rewardType = customData.get(RewardItem.KEY_REWARD_TYPE);
                if (rewardType instanceof Integer) {
                    jsonObject.put("rewardType", rewardType);
                }

                Object extraInfo = customData.get(RewardItem.KEY_EXTRA_INFO); //获取额外参数
                if (extraInfo instanceof Bundle) {
                    float rewardPropose = ((Bundle) extraInfo).getFloat(TTRewardVideoAd.REWARD_EXTRA_KEY_REWARD_PROPOSE);
                    jsonObject.put("rewardPropose", rewardPropose);
                }
            }
        } catch (Throwable ignored) {

        }
        return jsonObject.toString();
    }

}
