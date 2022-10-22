package com.bytedance.ad.sdk.mediation;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.Handler;
import android.os.Looper;
import android.text.TextUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.bytedance.msdk.api.AdError;
import com.bytedance.msdk.api.TTAdConstant;
import com.bytedance.msdk.api.v2.GMAdDislike;
import com.bytedance.msdk.api.TTAdSize;
import com.bytedance.msdk.api.TTDislikeCallback;
import com.bytedance.msdk.api.nativeAd.TTNativeAd;
import com.bytedance.msdk.api.nativeAd.TTNativeAdListener;
import com.bytedance.msdk.api.nativeAd.TTNativeExpressAdListener;
import com.bytedance.msdk.api.nativeAd.TTVideoListener;
import com.bytedance.msdk.api.nativeAd.TTViewBinder;

import java.util.ArrayList;
import java.util.List;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.ImageRequest;
import com.android.volley.toolbox.Volley;

public class FeedAdManager {

    private static volatile FeedAdManager sManager;
    private static Activity mContext;
    private FeedView mFeedView;
    private static final String TAG = "FeedAdManager";
    private static final int ITEM_VIEW_TYPE_NORMAL = 0;
    private static final int ITEM_VIEW_TYPE_GROUP_PIC_AD = 1;
    private static final int ITEM_VIEW_TYPE_SMALL_PIC_AD = 2;
    private static final int ITEM_VIEW_TYPE_LARGE_PIC_AD = 3;
    private static final int ITEM_VIEW_TYPE_VIDEO = 4;
    private static final int ITEM_VIEW_TYPE_VERTICAL_IMG = 5;//竖版图片
    private static final int ITEM_VIEW_TYPE_EXPRESS_AD = 6;//竖版图片
    private Handler mHandler;
    private RequestQueue mQueue;
    private ViewGroup mCurrentFeedLayout;
    private TTNativeAdListener mNativeAdListener;

    public static FeedAdManager getAdManager(Activity context) {
        mContext = context;
        if (sManager == null) {
            synchronized (FeedAdManager.class) {
                if (sManager == null) {
                    sManager = new FeedAdManager();
                }
            }
        }
        return sManager;
    }

    public static ViewGroup getRootLayout(Activity context) {
        if (context == null) {
            return null;
        }
        final ViewGroup rootGroup = context.findViewById(android.R.id.content);
        return rootGroup;
    }


    public void removeViewFromRootView(Activity activity, View view) {
        ViewGroup viewGroup = getRootLayout(activity);
        if (viewGroup == null || view == null) {
            return;
        }
        viewGroup.removeView(view);
    }

    public ViewGroup getFrameLayout(Activity context) {
        if (context == null) {
            return null;
        }
        FrameLayout frameLayout = new FrameLayout(context);
        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        layoutParams.gravity = Gravity.CENTER;
        frameLayout.setLayoutParams(layoutParams);
        frameLayout.setBackgroundColor(Color.parseColor("#ffffff"));
        ViewGroup rootGroup = getRootLayout(context);
        rootGroup.addView(frameLayout);
        return frameLayout;
    }

    //相关调用注意放在主线程
    public void showExpressFeedAd(final Activity activity, final TTNativeAd ad, final TTNativeExpressAdListener nativeAdListener) {
        //判断是否存在dislike按钮
        final ViewGroup viewGroup = getFrameLayout(activity);
        if (ad.hasDislike()) {
            ad.setDislikeCallback((Activity) mContext, new TTDislikeCallback() {
                @Override
                public void onSelected(int position, String value) {
                    //用户选择不喜欢原因后，移除广告展示
                    TToast.show(mContext, "dislike 点击了"+value);
                    Log.d(TAG, "dislike 点击了"+value);
                    removeViewFromRootView(activity, viewGroup);
                }

                @Override
                public void onCancel() {
                    TToast.show(mContext, "dislike 点击了取消");
                    Log.d(TAG, "dislike 点击了取消");
                }

                /**
                 * 拒绝再次提交
                 */
                @Override
                public void onRefuse() {

                }

                @Override
                public void onShow() {

                }
            });
        }

        //设置点击展示回调监听
        ad.setTTNativeAdListener(new TTNativeExpressAdListener() {
            @Override
            public void onAdClick() {
                Log.d(TAG, "onAdClick");
                TToast.show(mContext, "模板广告被点击");
                if (nativeAdListener != null) {
                    nativeAdListener.onAdClick();
                }
            }

            @Override
            public void onAdShow() {
                Log.d(TAG, "onAdShow");
                TToast.show(mContext, "模板广告show");
                if (nativeAdListener != null) {
                    nativeAdListener.onAdShow();
                }
            }

            @Override
            public void onRenderFail(View view, String msg, int code) {
                TToast.show(mContext, "模板广告渲染失败code=" + code + ",msg=" + msg);
                Log.d(TAG, "onRenderFail   code=" + code + ",msg=" + msg);
                if (nativeAdListener != null) {
                    nativeAdListener.onRenderFail(view, msg, code);
                }
            }

            @Override
            public void onRenderSuccess(float width, float height) {
                Log.d(TAG, "onRenderSuccess");
                TToast.show(mContext, "模板广告渲染成功:width=" + width + ",height=" + height);
                if (nativeAdListener != null) {
                    nativeAdListener.onRenderSuccess(width, height);
                }
                //回调渲染成功后将模板布局添加的父View中
                if (viewGroup != null) {
                    //获取视频播放view,该view SDK内部渲染，在媒体平台可配置视频是否自动播放等设置。
                    int sWidth;
                    int sHeight;
                    final View video = ad.getExpressView();
                    if (width == TTAdSize.FULL_WIDTH && height == TTAdSize.AUTO_HEIGHT) {
                        sWidth = FrameLayout.LayoutParams.MATCH_PARENT;
                        sHeight = FrameLayout.LayoutParams.WRAP_CONTENT;
                    } else {
                        sWidth = UIUtils.getScreenWidth(mContext);
                        sHeight = (int) ((sWidth * height) / width);
                    }
                    if (video != null) {
                        if (video.getParent() == null) {
                            FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(sWidth, sHeight);
                            viewGroup.removeAllViews();
                            viewGroup.addView(video, layoutParams);
                        }
                    }
                }
            }
        });

        //视频广告设置播放状态回调（可选）
        ad.setTTVideoListener(new TTVideoListener() {

            @Override
            public void onVideoStart() {
                TToast.show(mContext, "模板广告视频开始播放");
                Log.d(TAG, "onVideoStart");
            }

            @Override
            public void onVideoPause() {
                TToast.show(mContext, "模板广告视频暂停");
                Log.d(TAG, "onVideoPause");

            }

            @Override
            public void onVideoResume() {
                TToast.show(mContext, "模板广告视频继续播放");
                Log.d(TAG, "onVideoResume");

            }

            @Override
            public void onVideoCompleted() {
                TToast.show(mContext, "模板播放完成");
                Log.d(TAG, "onVideoCompleted");
            }

            @Override
            public void onVideoError(AdError adError) {
                TToast.show(mContext, "模板广告视频播放出错");
                Log.d(TAG, "onVideoError");
            }
        });
        ad.render();
    }


    //相关调用注意放在主线程
    public void showNativeFeedAd(final Activity context, final TTNativeAd nativeAd, TTNativeAdListener adListener) {
        if (context == null || nativeAd == null) {
            return;
        }
        if (nativeAd.getNativeAdAppInfo() != null) {
            String name = nativeAd.getNativeAdAppInfo().getAppName();
            String authorName = nativeAd.getNativeAdAppInfo().getAuthorName();
            String versionName = nativeAd.getNativeAdAppInfo().getVersionName();
            Log.e("showNativeFeedAd","合规五要素 name--authorName--versionname:" + name + "--" + authorName + "--" + versionName);
        }
        mNativeAdListener = adListener;
        mCurrentFeedLayout = getFrameLayout(context);
        mCurrentFeedLayout.addView(getNativeFeedView(nativeAd));
    }

    public View getNativeFeedView(TTNativeAd ad) {
        switch (getItemViewType(ad)) {
            case ITEM_VIEW_TYPE_SMALL_PIC_AD:
                return getSmallAdView(ad);
            case ITEM_VIEW_TYPE_LARGE_PIC_AD:
                return getLargeAdView(ad);
            case ITEM_VIEW_TYPE_GROUP_PIC_AD:
                return getGroupAdView(ad);
            case ITEM_VIEW_TYPE_VIDEO:
                return getVideoView(ad);
            case ITEM_VIEW_TYPE_VERTICAL_IMG:
                return getVerticalAdView(ad);
            default:
                return getNormalView();
        }
    }

    private static class VideoAdViewHolder extends AdViewHolder {
        FrameLayout videoView;
    }

    private static class LargeAdViewHolder extends AdViewHolder {
        ImageView mLargeImage;
    }

    private static class SmallAdViewHolder extends AdViewHolder {
        ImageView mSmallImage;
    }

    private static class VerticalAdViewHolder extends AdViewHolder {
        ImageView mVerticalImage;
    }

    private static class GroupAdViewHolder extends AdViewHolder {
        ImageView mGroupImage1;
        ImageView mGroupImage2;
        ImageView mGroupImage3;
    }

    private static class AdViewHolder {
        ImageView mIcon;
        ImageView mDislike;
        Button mCreativeButton;
        TextView mTitle;
        TextView mDescription;
        TextView mSource;
        RelativeLayout mLogo;

    }

    private static class NormalViewHolder {
        TextView idle;
    }


    private View getVerticalAdView(final TTNativeAd ad) {
        VerticalAdViewHolder adViewHolder;
        View convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_ad_vertical_pic"), null, false);
        adViewHolder = new VerticalAdViewHolder();
        adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title"));
        adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc"));
        adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source"));
        adViewHolder.mVerticalImage = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image"));
        adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_icon"));
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_dislike"));
        adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(mContext, "id", "btn_listitem_creative"));
        adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(mContext, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型
        TTViewBinder viewBinder = new TTViewBinder.Builder(MResource.getIdByName(mContext, "layout", "listitem_ad_vertical_pic")).
                titleId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title")).
                decriptionTextId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc")).
                sourceId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source")).
                mainImageId(MResource.getIdByName(mContext, "id", "iv_listitem_image")).
                callToActionId(MResource.getIdByName(mContext, "id", "btn_listitem_creative")).
                logoLayoutId(MResource.getIdByName(mContext, "id", "tt_ad_logo")).//logoView 建议传入GroupView类型
                iconImageId(MResource.getIdByName(mContext, "id", "iv_listitem_icon")).build();
        bindData(convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(ad.getImageUrl(), adViewHolder.mVerticalImage, 900, 600);
        }
        return convertView;
    }

    //渲染视频广告，以视频广告为例，以下说明
    private View getVideoView(final TTNativeAd ad) {
        final VideoAdViewHolder adViewHolder;
        View convertView = null;
        try {
            convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_ad_large_video"), null, false);
            adViewHolder = new VideoAdViewHolder();
            adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title"));
            adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc"));
            adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source"));
            adViewHolder.videoView = (FrameLayout) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_video"));
            adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_icon"));
            adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_dislike"));
            adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(mContext, "id", "btn_listitem_creative"));
            adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(mContext, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型
            TTViewBinder viewBinder = new TTViewBinder.Builder(MResource.getIdByName(mContext, "layout", "listitem_ad_large_video")).
                    titleId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title")).
                    decriptionTextId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc")).
                    sourceId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source")).
                    mediaViewIdId(MResource.getIdByName(mContext, "id", "iv_listitem_video")).
                    callToActionId(MResource.getIdByName(mContext, "id", "btn_listitem_creative")).
                    logoLayoutId(MResource.getIdByName(mContext, "id", "tt_ad_logo")).//logoView 建议传入GroupView类型
                    iconImageId(MResource.getIdByName(mContext, "id", "iv_listitem_icon")).build();

            //视频广告设置播放状态回调（可选）
            ad.setTTVideoListener(new TTVideoListener() {

                @Override
                public void onVideoStart() {
                    TToast.show(mContext, "广告视频开始播放");
                    Log.d(TAG, "onVideoStart");
                }

                @Override
                public void onVideoPause() {
                    TToast.show(mContext, "广告视频暂停");
                    Log.d(TAG, "onVideoPause");
                }

                @Override
                public void onVideoResume() {
                    TToast.show(mContext, "广告视频继续播放");
                    Log.d(TAG, "onVideoResume");
                }

                @Override
                public void onVideoCompleted() {
                    TToast.show(mContext, "广告播放完成");
                    Log.d(TAG, "onVideoCompleted");
                }

                @Override
                public void onVideoError(AdError adError) {
                    TToast.show(mContext, "广告视频播放出错");
                    Log.d(TAG, "onVideoError");
                }
            });

            //绑定广告数据、设置交互回调
            bindData(convertView, adViewHolder, ad, viewBinder);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return convertView;
    }

    private View getLargeAdView(final TTNativeAd ad) {
        final LargeAdViewHolder adViewHolder;
        View convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_ad_large_pic"), null, false);
        adViewHolder = new LargeAdViewHolder();
        adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title"));
        adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc"));
        adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source"));
        adViewHolder.mLargeImage = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image"));
        adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_icon"));
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_dislike"));
        adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(mContext, "id", "btn_listitem_creative"));
        adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(mContext, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型
        TTViewBinder viewBinder = new TTViewBinder.Builder(MResource.getIdByName(mContext, "layout", "listitem_ad_large_pic")).
                titleId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title")).
                decriptionTextId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc")).
                sourceId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source")).
                mainImageId(MResource.getIdByName(mContext, "id", "iv_listitem_image")).
                callToActionId(MResource.getIdByName(mContext, "id", "btn_listitem_creative")).
                logoLayoutId(MResource.getIdByName(mContext, "id", "tt_ad_logo")).//logoView 建议传入GroupView类型
                iconImageId(MResource.getIdByName(mContext, "id", "iv_listitem_icon")).build();
        bindData(convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(ad.getImageUrl(), adViewHolder.mLargeImage, 900, 600);
        }
        return convertView;
    }

    private View getGroupAdView(final TTNativeAd ad) {
        GroupAdViewHolder adViewHolder;
        View convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_ad_group_pic"), null, false);
        adViewHolder = new GroupAdViewHolder();
        adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title"));
        adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc"));
        adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source"));
        adViewHolder.mGroupImage1 = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image1"));
        adViewHolder.mGroupImage2 = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image2"));
        adViewHolder.mGroupImage3 = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image3"));
        adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_icon"));
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_dislike"));
        adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(mContext, "id", "btn_listitem_creative"));
        adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(mContext, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型
        TTViewBinder viewBinder = new TTViewBinder.Builder(MResource.getIdByName(mContext, "layout", "listitem_ad_group_pic")).
                titleId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title")).
                decriptionTextId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc")).
                sourceId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source")).
                mainImageId(MResource.getIdByName(mContext, "id", "iv_listitem_image1")).
                callToActionId(MResource.getIdByName(mContext, "id", "btn_listitem_creative")).
                logoLayoutId(MResource.getIdByName(mContext, "id", "tt_ad_logo")).//logoView 建议传入GroupView类型
                iconImageId(MResource.getIdByName(mContext, "id", "iv_listitem_icon")).build();

        bindData(convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageList() != null && ad.getImageList().size() >= 3) {
            String image1 = ad.getImageList().get(0);
            String image2 = ad.getImageList().get(1);
            String image3 = ad.getImageList().get(2);
            if (image1 != null) {
                loadImgByVolley(image1, adViewHolder.mGroupImage1, 900, 600);
            }
            if (image2 != null) {
                loadImgByVolley(image2, adViewHolder.mGroupImage2, 900, 600);
            }
            if (image3 != null) {
                loadImgByVolley(image3, adViewHolder.mGroupImage3, 900, 600);
            }
        }
        return convertView;
    }

    private View getSmallAdView(final TTNativeAd ad) {
        SmallAdViewHolder adViewHolder;
        View convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_ad_small_pic"), null, false);
        adViewHolder = new SmallAdViewHolder();
        adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title"));
        adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc"));
        adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source"));
        adViewHolder.mSmallImage = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_image"));
        adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_icon"));
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(mContext, "id", "iv_listitem_dislike"));
        adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(mContext, "id", "btn_listitem_creative"));
        adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(mContext, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型
        TTViewBinder viewBinder = new TTViewBinder.Builder(MResource.getIdByName(mContext, "layout", "listitem_ad_small_pic")).
                titleId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_title")).
                decriptionTextId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_desc")).
                sourceId(MResource.getIdByName(mContext, "id", "tv_listitem_ad_source")).
                mainImageId(MResource.getIdByName(mContext, "id", "iv_listitem_image")).
                callToActionId(MResource.getIdByName(mContext, "id", "btn_listitem_creative")).
                logoLayoutId(MResource.getIdByName(mContext, "id", "tt_ad_logo")).//logoView 建议传入GroupView类型
                iconImageId(MResource.getIdByName(mContext, "id", "iv_listitem_icon")).build();
        bindData(convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(ad.getImageUrl(), adViewHolder.mSmallImage, 900, 600);
        }
        return convertView;
    }

    private void bindData(final View convertView, final AdViewHolder adViewHolder, final TTNativeAd ad, TTViewBinder viewBinder) {
        //设置dislike弹窗，如果有
        if (ad.hasDislike()) {
            final GMAdDislike ttAdDislike = ad.getDislikeDialog((Activity) mContext);
            adViewHolder.mDislike.setVisibility(View.VISIBLE);
            adViewHolder.mDislike.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    //使用接口来展示
                    ttAdDislike.showDislikeDialog();
                    ttAdDislike.setDislikeCallback(new TTDislikeCallback() {
                        @Override
                        public void onSelected(int position, String value) {
                            //用户选择不喜欢原因后，移除广告展示
                            removeFeedView();
                        }

                        @Override
                        public void onCancel() {
                            TToast.show(mContext, "dislike 点击了取消");
                        }

                        /**
                         * 拒绝再次提交
                         */
                        @Override
                        public void onRefuse() {

                        }

                        @Override
                        public void onShow() {

                        }
                    });
                }
            });
        } else {
            if (adViewHolder.mDislike != null)
                adViewHolder.mDislike.setVisibility(View.GONE);
        }

        //设置事件回调
        ad.setTTNativeAdListener(mTTNativeAdListener);
        //可以被点击的view, 也可以把convertView放进来意味item可被点击
        List<View> clickViewList = new ArrayList<>();
        clickViewList.add(convertView);
        clickViewList.add(adViewHolder.mSource);
        clickViewList.add(adViewHolder.mTitle);
        clickViewList.add(adViewHolder.mDescription);
        clickViewList.add(adViewHolder.mIcon);
        //添加点击区域
        if (adViewHolder instanceof LargeAdViewHolder) {
            clickViewList.add(((LargeAdViewHolder) adViewHolder).mLargeImage);
        } else if (adViewHolder instanceof SmallAdViewHolder) {
            clickViewList.add(((SmallAdViewHolder) adViewHolder).mSmallImage);
        } else if (adViewHolder instanceof VerticalAdViewHolder) {
            clickViewList.add(((VerticalAdViewHolder) adViewHolder).mVerticalImage);
        } else if (adViewHolder instanceof VideoAdViewHolder) {
            clickViewList.add(((VideoAdViewHolder) adViewHolder).videoView);
        } else if (adViewHolder instanceof GroupAdViewHolder) {
            clickViewList.add(((GroupAdViewHolder) adViewHolder).mGroupImage1);
            clickViewList.add(((GroupAdViewHolder) adViewHolder).mGroupImage2);
            clickViewList.add(((GroupAdViewHolder) adViewHolder).mGroupImage3);
        }
        //触发创意广告的view（点击下载或拨打电话）
        List<View> creativeViewList = new ArrayList<>();
        creativeViewList.add(adViewHolder.mCreativeButton);
        //重要! 这个涉及到广告计费，必须正确调用。convertView必须使用ViewGroup。
        ad.registerView((ViewGroup) convertView, clickViewList, creativeViewList, viewBinder);
        adViewHolder.mTitle.setText(ad.getTitle()); //title为广告的简单信息提示
        adViewHolder.mDescription.setText(ad.getDescription()); //description为广告的较长的说明
        adViewHolder.mSource.setText(TextUtils.isEmpty(ad.getSource()) ? "广告来源" : ad.getSource());

        String icon = ad.getIconUrl();
        if (icon != null) {
            // Glide.with(mContext).load(icon).into(adViewHolder.mIcon);
        }
        Button adCreativeButton = adViewHolder.mCreativeButton;
        switch (ad.getInteractionType()) {
            case TTAdConstant.INTERACTION_TYPE_DOWNLOAD:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText(TextUtils.isEmpty(ad.getActionText()) ? "立即下载" : ad.getActionText());
                break;
            case TTAdConstant.INTERACTION_TYPE_DIAL:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText("立即拨打");
                break;
            case TTAdConstant.INTERACTION_TYPE_LANDING_PAGE:
            case TTAdConstant.INTERACTION_TYPE_BROWSER:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText(TextUtils.isEmpty(ad.getActionText()) ? "查看详情" : ad.getActionText());
                break;
            default:
                adCreativeButton.setVisibility(View.GONE);
                TToast.show(mContext, "交互类型异常");
        }
    }

    TTNativeAdListener mTTNativeAdListener = new TTNativeAdListener() {
        @Override
        public void onAdClick() {
            Log.d(TAG, "onAdClick");
            TToast.show(mContext, "自渲染广告被点击");
            if (mNativeAdListener != null) {
                mNativeAdListener.onAdClick();
            }
            removeFeedView();
        }


        @Override
        public void onAdShow() {
            Log.d(TAG, "onAdShow");
            TToast.show(mContext, "广告展示");
            if (mNativeAdListener != null) {
                mNativeAdListener.onAdShow();
            }
        }
    };


    private View getNormalView() {
        NormalViewHolder normalViewHolder;
        normalViewHolder = new NormalViewHolder();
        View convertView = LayoutInflater.from(mContext).inflate(MResource.getIdByName(mContext, "layout", "listitem_normal"), null, false);
        normalViewHolder.idle = (TextView) convertView.findViewById(MResource.getIdByName(mContext, "id", "text_idle"));
        convertView.setTag(normalViewHolder);
        return convertView;
    }


    //信息流广告的样式，有大图、小图、组图和视频，通过ad.getImageMode()来判断
    public int getItemViewType(TTNativeAd ad) {
        if (ad == null) {
            return ITEM_VIEW_TYPE_NORMAL;
        }
        //模板广告特殊处理
        if (ad != null && ad.isExpressAd()) {
            return ITEM_VIEW_TYPE_EXPRESS_AD;
        }
        if (ad == null) {
            return ITEM_VIEW_TYPE_NORMAL;
        } else if (ad.getAdImageMode() == TTAdConstant.IMAGE_MODE_SMALL_IMG) {
            return ITEM_VIEW_TYPE_SMALL_PIC_AD;
        } else if (ad.getAdImageMode() == TTAdConstant.IMAGE_MODE_LARGE_IMG) {
            return ITEM_VIEW_TYPE_LARGE_PIC_AD;
        } else if (ad.getAdImageMode() == TTAdConstant.IMAGE_MODE_GROUP_IMG) {
            return ITEM_VIEW_TYPE_GROUP_PIC_AD;
        } else if (ad.getAdImageMode() == TTAdConstant.IMAGE_MODE_VIDEO) {
            return ITEM_VIEW_TYPE_VIDEO;
        } else if (ad.getAdImageMode() == TTAdConstant.IMAGE_MODE_VERTICAL_IMG) {
            return ITEM_VIEW_TYPE_VERTICAL_IMG;
        } else {
            return ITEM_VIEW_TYPE_NORMAL;
        }
    }

    public void loadImgByVolley(String imgUrl, final ImageView imageView, int maxWidth, int maxHeight) {
        if (mHandler == null) {
            mHandler = new Handler(Looper.getMainLooper());
        }
        if (mQueue == null) {
            mQueue = Volley.newRequestQueue(mContext);
        }
        ImageRequest imgRequest = new ImageRequest(imgUrl,
                new Response.Listener<Bitmap>() {
                    @Override
                    public void onResponse(final Bitmap arg0) {
                        mHandler.post(new Runnable() {
                            @Override
                            public void run() {
                                imageView.setImageBitmap(arg0);
                            }
                        });
                    }
                }, maxWidth, maxHeight, Bitmap.Config.ARGB_8888,
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError arg0) {
                    }
                });
        mQueue.add(imgRequest);
    }

    private void removeFeedView() {
        removeViewFromRootView(mContext, mCurrentFeedLayout);
    }
}
