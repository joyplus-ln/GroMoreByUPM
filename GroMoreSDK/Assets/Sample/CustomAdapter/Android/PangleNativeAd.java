package com.bytedance.ad.sdk.mediation.adapter;

import android.util.Log;
import android.view.View;
import android.view.ViewGroup;

import com.bytedance.msdk.api.v2.ad.custom.GMCustomAdError;
import com.bytedance.msdk.api.v2.ad.custom.nativeAd.GMCustomNativeAd;
import com.bytedance.msdk.api.v2.ad.nativeAd.GMViewBinder;
import com.bytedance.sdk.openadsdk.TTAdConstant;
import com.bytedance.sdk.openadsdk.TTFeedAd;
import com.bytedance.sdk.openadsdk.TTImage;
import com.bytedance.sdk.openadsdk.TTNativeAd;

import java.util.ArrayList;
import java.util.List;

public class PangleNativeAd extends GMCustomNativeAd {

    private static final String TAG = "<Unity Log>";

    private TTFeedAd mTTFeedAd;

    public PangleNativeAd(TTFeedAd feedAd) {
        this.mTTFeedAd = feedAd;
        mTTFeedAd.setVideoAdListener(mVideoAdListener);
        this.setTitle(feedAd.getTitle()); // appName
        this.setDescription(feedAd.getDescription());
        this.setActionText(feedAd.getButtonText());
        this.setIconUrl(feedAd.getIcon() != null ? feedAd.getIcon().getImageUrl() : null);
        this.setAdImageMode(feedAd.getImageMode());
        this.setInteractionType(feedAd.getInteractionType());
        this.setSource(feedAd.getSource()); // 从数据看也是appName
        this.setStarRating(feedAd.getAppScore());
        //大图和小图
        if (feedAd.getImageMode() == TTAdConstant.IMAGE_MODE_VERTICAL_IMG ||
                feedAd.getImageMode() == TTAdConstant.IMAGE_MODE_LARGE_IMG ||
                feedAd.getImageMode() == TTAdConstant.IMAGE_MODE_SMALL_IMG) {
            if (feedAd.getImageList() != null && !feedAd.getImageList().isEmpty() && feedAd.getImageList().get(0) != null) {
                TTImage image = feedAd.getImageList().get(0);
                this.setImageUrl(image.getImageUrl());
                this.setImageHeight(image.getHeight());
                this.setImageWidth(image.getWidth());
            }
        } else if (feedAd.getImageMode() == TTAdConstant.IMAGE_MODE_GROUP_IMG) {//组图(3图)
            if (feedAd.getImageList() != null && feedAd.getImageList().size() > 0) {
                List<String> images = new ArrayList<>();
                for (TTImage image : feedAd.getImageList()) {
                    images.add(image.getImageUrl());
                }
                this.setImageList(images);
            }
        }
        this.setMediaExtraInfo(feedAd.getMediaExtraInfo());
    }

    TTNativeAd.AdInteractionListener mAdInteractionListener = new TTNativeAd.AdInteractionListener() {
        @Override
        public void onAdClicked(View view, TTNativeAd ttNativeAd) {
            callNativeAdClick();
        }

        @Override
        public void onAdCreativeClick(View view, TTNativeAd ttNativeAd) {
            callNativeAdClick();
        }

        @Override
        public void onAdShow(TTNativeAd ttNativeAd) {
            callNativeAdShow();
        }
    };

    @Override
    public void registerViewForInteraction(ViewGroup container, List<View> clickViews, List<View> creativeViews, GMViewBinder viewBinder) {
        if (mTTFeedAd != null) {
            mTTFeedAd.registerViewForInteraction(container, clickViews, creativeViews, mAdInteractionListener);
        }
    }

    TTFeedAd.VideoAdListener mVideoAdListener = new TTFeedAd.VideoAdListener() {
        @Override
        public void onVideoLoad(TTFeedAd ttFeedAd) {

        }

        @Override
        public void onVideoError(int i, int code) {
            callNativeVideoError(new GMCustomAdError(i, code + ""));
        }

        @Override
        public void onVideoAdStartPlay(TTFeedAd ttFeedAd) {
            callNativeVideoStart();
        }

        @Override
        public void onVideoAdPaused(TTFeedAd ttFeedAd) {
            callNativeVideoPause();
        }

        @Override
        public void onVideoAdContinuePlay(TTFeedAd ttFeedAd) {
            callNativeVideoResume();
        }

        @Override
        public void onProgressUpdate(long l, long l1) {

        }

        @Override
        public void onVideoAdComplete(TTFeedAd ttFeedAd) {
            callNativeVideoCompleted();
        }
    };

    @Override
    public void onPause() {
        super.onPause();
        Log.i(TAG,"onPause");
    }

    @Override
    public void onResume() {
        super.onResume();
        Log.i(TAG,"onResume");
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.i(TAG,"onDestroy");
    }
}
