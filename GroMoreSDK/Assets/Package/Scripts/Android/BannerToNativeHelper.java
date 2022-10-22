package com.bytedance.ad.sdk.mediation;

import android.app.Activity;
import android.graphics.Bitmap;
import android.os.Handler;
import android.os.Looper;
import android.support.annotation.NonNull;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.ImageRequest;
import com.android.volley.toolbox.Volley;
import com.bytedance.msdk.api.TToast;
import com.bytedance.msdk.api.nativeAd.TTViewBinder;
import com.bytedance.msdk.api.v2.GMAdConstant;
import com.bytedance.msdk.api.v2.GMAdDislike;
import com.bytedance.msdk.api.v2.GMDislikeCallback;
import com.bytedance.msdk.api.v2.ad.banner.GMNativeAdInfo;
import com.bytedance.msdk.api.v2.ad.nativeAd.GMNativeAdAppInfo;
import com.bytedance.msdk.api.v2.ad.nativeAd.GMViewBinder;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * created by jijiachun on 2022/7/15
 */
public class BannerToNativeHelper {

    private static Handler mHandler;
    private static RequestQueue mQueue;

    public static View getBannerViewFromNativeAd(Activity activity, GMNativeAdInfo mGMNativeAd) {
        View view = null;
        if (mGMNativeAd == null) {
            return null;
        }
        if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_SMALL_IMG) { //原生小图
            view = getSmallAdView(activity, mGMNativeAd);
        } else if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_LARGE_IMG) {//原生大图
            view = getLargeAdView(activity, mGMNativeAd);
        } else if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_GROUP_IMG) {//原生组图
            view = getGroupAdView(activity, mGMNativeAd);
        } else if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_VIDEO) {//原生视频
            view = getVideoView(activity, mGMNativeAd);
        } else if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_VERTICAL_IMG) {//原生竖版图片
            view = getVerticalAdView(activity, mGMNativeAd);
        } else if (mGMNativeAd.getAdImageMode() == GMAdConstant.IMAGE_MODE_VIDEO_VERTICAL) {//原生视频
            view = getVideoView(activity, mGMNativeAd);
        } else {
            TToast.show(activity, "图片展示样式错误");
        }
        return view;
    }


    private static View getVerticalAdView(Activity activity, @NonNull final GMNativeAdInfo ad) {
        VerticalAdViewHolder adViewHolder;
        View convertView = null;
        GMViewBinder viewBinder;
        convertView = LayoutInflater.from(activity).inflate(MResource.getIdByName(activity, "layout", "listitem_ad_vertical_pic"), null, false);
        adViewHolder = new VerticalAdViewHolder();
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_dislike"));
        fillAdViewHolder(activity, adViewHolder, convertView);
        viewBinder = new GMViewBinder.Builder(convertView.getId())
                .titleId(adViewHolder.mTitle.getId())
                .descriptionTextId(adViewHolder.mDescription.getId())
                .mainImageId(adViewHolder.mVerticalImage.getId())
                .iconImageId(adViewHolder.mIcon.getId())
                .callToActionId(adViewHolder.mCreativeButton.getId())
                .sourceId(adViewHolder.mSource.getId())
                .logoLayoutId(adViewHolder.mLogo.getId())//logoView 建议传入GroupView类型
                .build();
        adViewHolder.viewBinder = viewBinder;
        bindData(activity, convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mVerticalImage, 900, 600);
        }

        return convertView;
    }

    //渲染视频广告，以视频广告为例，以下说明
    @SuppressWarnings("RedundantCast")
    private static View getVideoView(Activity activity, @NonNull final GMNativeAdInfo ad) {
        VideoAdViewHolder adViewHolder;
        GMViewBinder viewBinder;
        View convertView = null;
        try {
            convertView = LayoutInflater.from(activity).inflate(MResource.getIdByName(activity, "layout", "listitem_ad_large_video"), null, false);
            adViewHolder = new VideoAdViewHolder();
            adViewHolder.videoView = (FrameLayout) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_video"));
            // 可以通过GMNativeAd.getVideoWidth()、GMNativeAd.getVideoHeight()来获取视频的尺寸，进行UI调整（如果有需求的话）。
            // 在使用时需要判断返回值，如果返回为0，即表示该adn的广告不支持。目前仅Pangle和ks支持。
//                    int videoWidth = ad.getVideoWidth();
//                    int videoHeight = ad.getVideoHeight();

            fillAdViewHolder(activity, adViewHolder, convertView);

            //TTViewBinder 是必须类,需要开发者在确定好View之后把Id设置给TTViewBinder类，并在注册事件时传递给SDK
            viewBinder = new GMViewBinder.Builder(MResource.getIdByName(activity, "layout", "listitem_ad_large_video")).
                    titleId(adViewHolder.mTitle.getId()).
                    sourceId(adViewHolder.mSource.getId()).
                    descriptionTextId(adViewHolder.mDescription.getId()).
                    mediaViewIdId(adViewHolder.videoView.getId()).
                    callToActionId(adViewHolder.mCreativeButton.getId()).
                    logoLayoutId(adViewHolder.mLogo.getId()).//logoView 建议传入GroupView类型
                            iconImageId(adViewHolder.mIcon.getId()).build();
            adViewHolder.viewBinder = viewBinder;

            //绑定广告数据、设置交互回调
            bindData(activity, convertView, adViewHolder, ad, viewBinder);
        } catch (Exception ignored) {
        }

        return convertView;
    }

    private static void fillAdViewHolder(Activity activity, AdViewHolder adViewHolder, View convertView) {
        adViewHolder.mTitle = (TextView) convertView.findViewById(MResource.getIdByName(activity, "id", "tv_listitem_ad_title"));
        adViewHolder.mSource = (TextView) convertView.findViewById(MResource.getIdByName(activity, "id", "tv_listitem_ad_source"));
        adViewHolder.mDescription = (TextView) convertView.findViewById(MResource.getIdByName(activity, "id", "tv_listitem_ad_desc"));
        adViewHolder.mIcon = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_icon"));
        adViewHolder.mDislike = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_dislike"));
        adViewHolder.mCreativeButton = (Button) convertView.findViewById(MResource.getIdByName(activity, "id", "btn_listitem_creative"));
        adViewHolder.mLogo = convertView.findViewById(MResource.getIdByName(activity, "id", "tt_ad_logo"));//logoView 建议传入GroupView类型


        adViewHolder.app_info = convertView.findViewById(MResource.getIdByName(activity, "id", "app_info"));
        adViewHolder.app_name = convertView.findViewById(MResource.getIdByName(activity, "id", "app_name"));
        adViewHolder.author_name = convertView.findViewById(MResource.getIdByName(activity, "id", "author_name"));
        adViewHolder.package_size = convertView.findViewById(MResource.getIdByName(activity, "id", "package_size"));
        adViewHolder.permissions_url = convertView.findViewById(MResource.getIdByName(activity, "id", "permissions_url"));
        adViewHolder.permissions_content = convertView.findViewById(MResource.getIdByName(activity, "id", "permissions_content"));
        adViewHolder.privacy_agreement = convertView.findViewById(MResource.getIdByName(activity, "id", "privacy_agreement"));
        adViewHolder.version_name = convertView.findViewById(MResource.getIdByName(activity, "id", "version_name"));
    }

    @SuppressWarnings("RedundantCast")
    private static View getLargeAdView(Activity activity, @NonNull final GMNativeAdInfo ad) {
        final LargeAdViewHolder adViewHolder;
        GMViewBinder viewBinder;
        View convertView = null;
        convertView = LayoutInflater.from(activity).inflate(MResource.getIdByName(activity, "layout", "listitem_ad_large_pic"), null, false);
        adViewHolder = new LargeAdViewHolder();
        adViewHolder.mLargeImage = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_image"));
        fillAdViewHolder(activity, adViewHolder, convertView);
        viewBinder = new GMViewBinder.Builder(convertView.getId()).
                titleId(adViewHolder.mTitle.getId()).
                descriptionTextId(adViewHolder.mDescription.getId()).
                sourceId(adViewHolder.mSource.getId()).
                mainImageId(adViewHolder.mLargeImage.getId()).
                callToActionId(adViewHolder.mCreativeButton.getId()).
                logoLayoutId(adViewHolder.mLogo.getId()).//logoView 建议传入GroupView类型
                        iconImageId(adViewHolder.mIcon.getId()).build();
        adViewHolder.viewBinder = viewBinder;
        bindData(activity, convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mLargeImage, 900, 600);
        }
        return convertView;
    }

    @SuppressWarnings("RedundantCast")
    private static View getGroupAdView(Activity activity, @NonNull final GMNativeAdInfo ad) {
        GroupAdViewHolder adViewHolder;
        GMViewBinder viewBinder;
        View convertView = null;
        convertView = LayoutInflater.from(activity).inflate(MResource.getIdByName(activity, "layout", "listitem_ad_group_pic"), null, false);
        adViewHolder = new GroupAdViewHolder();
        adViewHolder.mGroupImage1 = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_image1"));
        adViewHolder.mGroupImage2 = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_image2"));
        adViewHolder.mGroupImage3 = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_image3"));

        fillAdViewHolder(activity, adViewHolder, convertView);

        viewBinder = new TTViewBinder.Builder(MResource.getIdByName(activity, "layout", "listitem_ad_group_pic")).
                titleId(adViewHolder.mTitle.getId()).
                descriptionTextId(adViewHolder.mDescription.getId()).
                sourceId(adViewHolder.mSource.getId()).
                mainImageId(adViewHolder.mGroupImage1.getId()).//传第一张即可
                        logoLayoutId(adViewHolder.mLogo.getId()).//logoView 建议传入GroupView类型
                        callToActionId(adViewHolder.mCreativeButton.getId()).
                iconImageId(adViewHolder.mIcon.getId()).
                groupImage1Id(adViewHolder.mGroupImage1.getId()).
                groupImage2Id(adViewHolder.mGroupImage2.getId()).
                groupImage3Id(adViewHolder.mGroupImage3.getId()).
                build();
        adViewHolder.viewBinder = viewBinder;

        bindData(activity, convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageList() != null && ad.getImageList().size() >= 3) {
            String image1 = ad.getImageList().get(0);
            String image2 = ad.getImageList().get(1);
            String image3 = ad.getImageList().get(2);
            if (image1 != null) {
                loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mGroupImage1, 900, 600);

            }
            if (image2 != null) {
                loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mGroupImage2, 900, 600);

            }
            if (image3 != null) {
                loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mGroupImage3, 900, 600);

            }
        }
        return convertView;
    }


    @SuppressWarnings("RedundantCast")
    private static View getSmallAdView(Activity activity, @NonNull final GMNativeAdInfo ad) {
        SmallAdViewHolder adViewHolder;
        GMViewBinder viewBinder;
        View convertView = null;
        convertView = LayoutInflater.from(activity).inflate(MResource.getIdByName(activity, "layout", "listitem_ad_small_pic"), null, false);
        adViewHolder = new SmallAdViewHolder();
        adViewHolder.mSmallImage = (ImageView) convertView.findViewById(MResource.getIdByName(activity, "id", "iv_listitem_image"));
        fillAdViewHolder(activity, adViewHolder, convertView);
        viewBinder = new GMViewBinder.Builder(convertView.getId()).
                titleId(adViewHolder.mTitle.getId()).
                sourceId(adViewHolder.mSource.getId()).
                descriptionTextId(adViewHolder.mDescription.getId()).
                mainImageId(adViewHolder.mSmallImage.getId()).
                logoLayoutId(adViewHolder.mLogo.getId()).//logoView 建议为GroupView 类型
                        callToActionId(adViewHolder.mCreativeButton.getId()).
                iconImageId(adViewHolder.mIcon.getId()).build();
        adViewHolder.viewBinder = viewBinder;
        bindData(activity, convertView, adViewHolder, ad, viewBinder);
        if (ad.getImageUrl() != null) {
            loadImgByVolley(activity, ad.getImageUrl(), adViewHolder.mSmallImage, 900, 600);
        }
        return convertView;
    }

    public static void loadImgByVolley(Activity activity, String imgUrl, final ImageView imageView, int maxWidth, int maxHeight) {
        if (mHandler == null) {
            mHandler = new Handler(Looper.getMainLooper());
        }
        if (mQueue == null) {
            mQueue = Volley.newRequestQueue(activity);
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

    private static void bindData(Activity activity, View convertView, final AdViewHolder adViewHolder, final GMNativeAdInfo ad, GMViewBinder viewBinder) {
        //设置dislike弹窗，如果有
        if (ad.hasDislike()) {
            final GMAdDislike ttAdDislike = ad.getDislikeDialog(activity);
            adViewHolder.mDislike.setVisibility(View.VISIBLE);
            adViewHolder.mDislike.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (ttAdDislike != null) {
                        //使用接口来展示
                        ttAdDislike.showDislikeDialog();
                        ttAdDislike.setDislikeCallback(new GMDislikeCallback() {
                            @Override
                            public void onSelected(int position, String value) {
                                TToast.show(activity, "点击 " + value);
                            }

                            @Override
                            public void onCancel() {
                                TToast.show(activity, "dislike 点击了取消");
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
                }
            });
        } else {
            if (adViewHolder.mDislike != null)
                adViewHolder.mDislike.setVisibility(View.GONE);
        }

        setDownLoadAppInfo(ad, adViewHolder);

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
        //重要! 这个涉及到广告计费，必须正确调用。**** convertView必须是com.bytedance.msdk.api.format.TTNativeAdView ****
        ad.registerView(activity, (ViewGroup) convertView, clickViewList, creativeViewList, viewBinder);

        adViewHolder.mTitle.setText(ad.getTitle()); //title为广告的简单信息提示
        adViewHolder.mDescription.setText(ad.getDescription()); //description为广告的较长的说明
        adViewHolder.mSource.setText(TextUtils.isEmpty(ad.getSource()) ? "广告来源" : ad.getSource());

        String icon = ad.getIconUrl();
        if (icon != null) {
            loadImgByVolley(activity, ad.getIconUrl(), adViewHolder.mIcon, 900, 600);

        }
        Button adCreativeButton = adViewHolder.mCreativeButton;
        switch (ad.getInteractionType()) {
            case GMAdConstant.INTERACTION_TYPE_DOWNLOAD:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText(TextUtils.isEmpty(ad.getActionText()) ? "立即下载" : ad.getActionText());
                break;
            case GMAdConstant.INTERACTION_TYPE_DIAL:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText("立即拨打");
                break;
            case GMAdConstant.INTERACTION_TYPE_LANDING_PAGE:
            case GMAdConstant.INTERACTION_TYPE_BROWSER:
                adCreativeButton.setVisibility(View.VISIBLE);
                adCreativeButton.setText(TextUtils.isEmpty(ad.getActionText()) ? "查看详情" : ad.getActionText());
                break;
            default:
                adCreativeButton.setVisibility(View.GONE);
                TToast.show(activity, "交互类型异常");
        }
    }

    private static void setDownLoadAppInfo(GMNativeAdInfo drawAd, AdViewHolder adViewHolder) {
        if (adViewHolder == null) {
            return;
        }
        if (drawAd == null || drawAd.getNativeAdAppInfo() == null) {
            adViewHolder.app_info.setVisibility(View.GONE);
        } else {
            adViewHolder.app_info.setVisibility(View.VISIBLE);
            GMNativeAdAppInfo appInfo = drawAd.getNativeAdAppInfo();
            adViewHolder.app_name.setText("应用名称：" + appInfo.getAppName());
            adViewHolder.author_name.setText("开发者：" + appInfo.getAuthorName());
            adViewHolder.package_size.setText("包大小：" + appInfo.getPackageSizeBytes());
            adViewHolder.permissions_url.setText("权限url:" + appInfo.getPermissionsUrl());
            adViewHolder.privacy_agreement.setText("隐私url：" + appInfo.getPrivacyAgreement());
            adViewHolder.version_name.setText("版本号：" + appInfo.getVersionName());
            adViewHolder.permissions_content.setText("权限内容:" + getPermissionsContent(appInfo.getPermissionsMap()));
        }
    }

    private static String getPermissionsContent(Map<String, String> permissionsMap) {
        if (permissionsMap == null) {
            return "";
        }
        StringBuilder stringBuffer = new StringBuilder();
        Set<String> keyList = permissionsMap.keySet();
        for (String s : keyList) {
            stringBuffer.append(s).append(" : ").append(permissionsMap.get(s)).append(" \n");
        }

        return stringBuffer.toString();
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

    private static class ExpressAdViewHolder {
        FrameLayout mAdContainerView;
    }

    private static class AdViewHolder {
        GMViewBinder viewBinder;
        ImageView mIcon;
        ImageView mDislike;
        Button mCreativeButton;
        TextView mTitle;
        TextView mDescription;
        TextView mSource;
        RelativeLayout mLogo;

        LinearLayout app_info;
        TextView app_name;
        TextView author_name;
        TextView package_size;
        TextView permissions_url;
        TextView privacy_agreement;
        TextView version_name;
        TextView permissions_content;
    }
}
