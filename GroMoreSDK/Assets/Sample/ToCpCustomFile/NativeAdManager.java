//package com.bytedance.android;
//
//import android.annotation.SuppressLint;
//import android.app.Activity;
//import android.app.Dialog;
//import android.content.Context;
//import android.content.res.Resources;
//import android.graphics.Bitmap;
//import android.graphics.PixelFormat;
//import android.os.Handler;
//import android.os.Looper;
//import android.util.Log;
//import android.view.Gravity;
//import android.view.View;
//import android.view.ViewGroup;
//import android.view.WindowManager;
//import android.widget.FrameLayout;
//
//import android.widget.Button;
//import android.widget.ImageView;
//import android.widget.Toast;
//
//import com.android.volley.RequestQueue;
//import com.android.volley.Response;
//import com.android.volley.VolleyError;
//import com.android.volley.toolbox.ImageRequest;
//import com.android.volley.toolbox.Volley;
//import com.bytedance.sdk.openadsdk.FilterWord;
//import com.bytedance.sdk.openadsdk.TTAdDislike;
//import com.bytedance.sdk.openadsdk.TTNativeExpressAd;
//import com.ss.union.sdk.ad.bean.LGImage;
//import com.ss.union.sdk.ad.callback.LGAppDownloadCallback;
//import com.ss.union.sdk.ad.type.LGNativeAd;
//import com.ss.union.sdk.ad.views.LGAdDislike;
//
//import java.util.ArrayList;
//import java.util.List;
//
//@SuppressWarnings("EmptyMethod")
//public class NativeAdManager {
//
//    private static volatile NativeAdManager sManager;
//
//    private WindowManager.LayoutParams wmParams;
//    private WindowManager mWindowManager;
//    private Context mContext;
//    private BannerView mBannerView;
//    private View splashView;
//    private boolean mHasAddView = false;
//    private boolean mHasAddSplashView = false;
//    private Handler mHandler;
//    private RequestQueue mQueue;
//
//    private View mExpressView;
//
//    private Dialog mAdDialog;
//
//    private NativeAdManager() {
//         if (mHandler == null) {
//            mHandler = new Handler(Looper.getMainLooper());
//        }
//    }
//
//    public static NativeAdManager getNativeAdManager() {
//        if (sManager == null) {
//            synchronized (NativeAdManager.class) {
//                if (sManager == null) {
//                    sManager = new NativeAdManager();
//                }
//            }
//        }
//        return sManager;
//    }
//
// //相关调用注意放在主线程
//    public void showExpressFeedAd(final Context context, final TTNativeExpressAd nativeExpressAd,final int x,final int y ,
//                                  final TTNativeExpressAd.ExpressAdInteractionListener listener,
//                                  final TTAdDislike.DislikeInteractionCallback dislikeCallback) {
//        if (context == null || nativeExpressAd == null) {
//            return;
//        }
//        mContext = context;
//        nativeExpressAd.setExpressInteractionListener(new TTNativeExpressAd.ExpressAdInteractionListener() {
//            @Override
//            public void onAdClicked(View view, int i) {
//                if (listener != null) {
//                    listener.onAdClicked(view, i);
//                }
//            }
//
//            @Override
//            public void onAdShow(View view, int i) {
//                if (listener != null) {
//                    listener.onAdShow(view, i);
//                }
//            }
//
//            @Override
//            public void onRenderFail(View view, String s, int i) {
//                if (listener != null) {
//                    listener.onRenderFail(view, s, i);
//                }
//            }
//
//            @Override
//            public void onRenderSuccess(final View view, final float v, final float v1) {
//                if (listener != null) {
//                    listener.onRenderSuccess(view, v, v1);
//                }
//                mHandler.post(new Runnable() {
//                    @Override
//                    public void run() {
//                        removeAdView((Activity) context, mExpressView);
//                        mExpressView = view;
//                        FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams((int) dip2Px(context, v), (int) dip2Px(context, v1));
//                        layoutParams.leftMargin=dip2Px(context,x);
//                        layoutParams.topMargin=dip2Px(context,y);
//                        addAdView((Activity) context, mExpressView, layoutParams);
//                    }
//                });
//            }
//        });
//        mHandler.post(new Runnable() {
//            @Override
//            public void run() {
//                nativeExpressAd.setDislikeCallback((Activity) mContext, new TTAdDislike.DislikeInteractionCallback() {
//                    @Override
//                    public void onSelected(int i, String s) {
//                        if (dislikeCallback != null) {
//                            dislikeCallback.onSelected(i, s);
//                        }
//                        mHandler.post(new Runnable() {
//                            @Override
//                            public void run() {
//                                removeExpressView();
//                            }
//                        });
//                    }
//
//                    @Override
//                    public void onCancel() {
//                        if (dislikeCallback != null) {
//                            dislikeCallback.onCancel();
//                        }
//                    }
//                    
//                     @Override
//                     public void onRefuse() {
//                        
//                     }
//                });
//            }
//        });
//
//        mHandler.post(new Runnable() {
//            @Override
//            public void run() {
//                nativeExpressAd.render();
//            }
//        });
//
//    }
//
//    public void showNativeBannerAd(final Context context, final LGNativeAd nativeAd) {
//        if (context == null || nativeAd == null) {
//            return;
//        }
//        if (mHandler == null) {
//            mHandler = new Handler(Looper.getMainLooper());
//        }
//        mContext = context;
//        if (mQueue == null) {
//            mQueue = Volley.newRequestQueue(mContext);
//        }
//
//        mHandler.post(new Runnable() {
//            @Override
//            public void run() {
//                if (mWindowManager == null) {
//                    mWindowManager = (WindowManager) context.getSystemService(context.WINDOW_SERVICE);
//                }
//                if (wmParams == null) {
//                    wmParams = new WindowManager.LayoutParams();
//                    wmParams.gravity = Gravity.CENTER | Gravity.BOTTOM;
//                    // wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE; //注意：不能使用
//                    // FLAG_NOT_FOCUSABLE，会影响广告展示计费
//                    wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL;
//                    wmParams.width = 500;
//                    wmParams.height = 400;
//                    // 设置Banner 位置距离底部一个导航栏高度，接入方可定制。如果完全靠近底部位置，在一加手机上存在导航栏遮挡广告的情况
//                    wmParams.y = getNavigationBarHeight(mContext);
//                    wmParams.format = PixelFormat.RGBA_8888;
//                    getNavigationBarHeight(mContext);
//                }
//                removeBannerView();
//                mBannerView = new BannerView(context);
//                addBannerView();
//                // 绑定原生广告的数据
//                setBannerAdData(mBannerView, nativeAd);
//            }
//        });
//    }
//
//    public ViewGroup getRootLayout(Activity context) {
//        if (context == null) {
//            return null;
//        }
//        ViewGroup rootGroup = null;
//        rootGroup = context.findViewById(android.R.id.content);
//        return rootGroup;
//    }
//
//    public void addAdView(Activity context, View adView, ViewGroup.LayoutParams layoutParams) {
//        if (context == null || adView == null || layoutParams == null) {
//            return;
//        }
//        ViewGroup group = getRootLayout(context);
//        if (group == null) {
//            return;
//        }
//        group.addView(adView, layoutParams);
//    }
//
//    public void removeAdView(Activity context, View adView) {
//        if (context == null || adView == null) {
//            return;
//        }
//        ViewGroup group = getRootLayout(context);
//        if (group == null) {
//            return;
//        }
//        group.removeView(adView);
//    }
//
//    //广告使用完毕后，比如关闭或移除后，请调用destory释放资源。
//    public void destoryExpressAd(final TTNativeExpressAd nativeExpressAd) {
//        if (nativeExpressAd == null) {
//            return;
//        }
//        mHandler.post(new Runnable() {
//            @Override
//            public void run() {
//                nativeExpressAd.destroy();
//            }
//        });
//    }
//
//
//    private int dip2Px(Context context, float dipValue) {
//        final float scale = context.getResources().getDisplayMetrics().density;
//        return (int)(dipValue * scale + 0.5f);
//    }
//
//    private void removeBannerView() {
//        if (mHasAddView) {
//            mWindowManager.removeView(mBannerView);
//            mHasAddView = false;
//        }
//    }
//
//    private void addBannerView() {
//        if (!mHasAddView) {
//            mWindowManager.addView(mBannerView, wmParams);
//            mHasAddView = true;
//        }
//    }
//
//    private void removeExpressView() {
//        removeAdView((Activity) mContext, mExpressView);
//    }
//
//    private void setBannerAdData(BannerView nativeView, LGNativeAd nativeAd) {
//        nativeView.setTitle(nativeAd.getTitle());
//        View dislike = nativeView.getDisLikeView();
//        Button mCreativeButton = nativeView.getCreateButton();
//        bindDislikeAction(nativeAd, dislike, new LGAdDislike.InteractionCallback() {
//            @Override
//            public void onSelected(int position, String value) {
//                removeBannerView();
//            }
//
//            @Override
//            public void onCancel() {
//
//            }
//        });
//
//        if (nativeAd.getImageList() != null && !nativeAd.getImageList().isEmpty()) {
//            LGImage image = nativeAd.getImageList().get(0);
//            if (image != null && image.isValid()) {
//                ImageView im = nativeView.getImageView();
//                loadImgByVolley(image.getImageUrl(), im, 300, 200);
//            }
//        }
//
//        // 可根据广告类型，为交互区域设置不同提示信息
//        LGNativeAd.InteractionType type = nativeAd.getInteractionType();
//        if (type == LGNativeAd.InteractionType.DOWNLOAD) {
//            // 如果初始化ttAdManager.createAdNative(getApplicationContext())没有传入activity
//            // 则需要在此传activity，否则影响使用Dislike逻辑
//            nativeAd.setActivityForDownloadApp((Activity) mContext);
//            mCreativeButton.setVisibility(View.VISIBLE);
//            nativeAd.setDownloadCallback(new MyDownloadListener(mCreativeButton)); // 注册下载监听器
//        } else if (type == LGNativeAd.InteractionType.DIAL) {
//            mCreativeButton.setVisibility(View.VISIBLE);
//            mCreativeButton.setText("立即拨打");
//        } else if (type == LGNativeAd.InteractionType.LANDING_PAGE
//                || type == LGNativeAd.InteractionType.BROWSER) {
//            mCreativeButton.setVisibility(View.VISIBLE);
//            mCreativeButton.setText("查看详情");
//        } else {
//            mCreativeButton.setVisibility(View.GONE);
//        }
//
//        // 可以被点击的view, 也可以把nativeView放进来意味整个广告区域可被点击
//        List<View> clickViewList = new ArrayList<>();
//        clickViewList.add(nativeView);
//
//        // 触发创意广告的view（点击下载或拨打电话）
//        List<View> creativeViewList = new ArrayList<>();
//        // 如果需要点击图文区域也能进行下载或者拨打电话动作，请将图文区域的view传入
//        // creativeViewList.add(nativeView);
//        creativeViewList.add(mCreativeButton);
//
//        // 重要! 这个涉及到广告计费，必须正确调用。convertView必须使用ViewGroup。
//        nativeAd.registerViewForInteraction((ViewGroup) nativeView, clickViewList, creativeViewList, dislike,
//                new LGNativeAd.AdInteractionCallback() {
//                    @Override
//                    public void onAdClicked(View view, LGNativeAd ad) {
//                        if (ad != null) {
//                            Toast.makeText(mContext, "广告" + ad.getTitle() + "被点击", Toast.LENGTH_SHORT).show();
//                        }
//                        removeBannerView();
//                    }
//
//                    @Override
//                    public void onAdCreativeClick(View view, LGNativeAd ad) {
//                        if (ad != null) {
//                            Toast.makeText(mContext, "广告" + ad.getTitle() + "被创意按钮被点击", Toast.LENGTH_SHORT).show();
//                        }
//                        removeBannerView();
//                    }
//
//                    @Override
//                    public void onAdShow(LGNativeAd ad) {
//                        // 展示之后，让当前View 失去焦点，否则会出现进入后台再返回导致当前页面出现黑屏的问题
//
//                        wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL
//                                | WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE;
//                        mWindowManager.updateViewLayout(mBannerView, wmParams);
//                        if (ad != null) {
//                            Toast.makeText(mContext, "广告" + ad.getTitle() + "展示", Toast.LENGTH_SHORT).show();
//                        }
//                    }
//                });
//    }
//
//    // 接入网盟的dislike 逻辑，有助于提示广告精准投放度
//    private void bindDislikeAction(LGNativeAd ad, View dislikeView, LGAdDislike.InteractionCallback callback) {
//        final LGAdDislike ttAdDislike = ad.getDislikeDialog((Activity) mContext);
//        if (ttAdDislike != null) {
//            ttAdDislike.setDislikeInteractionCallback(callback);
//        }
//        dislikeView.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                if (ttAdDislike != null)
//                    ttAdDislike.showDislikeDialog();
//            }
//        });
//    }
//
//    static class MyDownloadListener implements LGAppDownloadCallback {
//        Button mDownloadButton;
//
//        public MyDownloadListener(Button button) {
//            mDownloadButton = button;
//        }
//
//        @Override
//        public void onIdle() {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("开始下载");
//            }
//        }
//
//        @SuppressLint("SetTextI18n")
//        @Override
//        public void onDownloadActive(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                if (totalBytes <= 0L) {
//                    mDownloadButton.setText("下载中 percent: 0");
//                } else {
//                    mDownloadButton.setText("下载中 percent: " + (currBytes * 100 / totalBytes));
//                }
//            }
//        }
//
//        @SuppressLint("SetTextI18n")
//        @Override
//        public void onDownloadPaused(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("下载暂停 percent: " + (currBytes * 100 / totalBytes));
//            }
//        }
//
//        @Override
//        public void onDownloadFailed(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("重新下载");
//            }
//        }
//
//        @Override
//        public void onInstalled(String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("点击打开");
//            }
//        }
//
//        @Override
//        public void onDownloadFinished(long totalBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("点击安装");
//            }
//        }
//    }
//
//    public void loadImgByVolley(String imgUrl, final ImageView imageView, int maxWidth, int maxHeight) {
//        ImageRequest imgRequest = new ImageRequest(imgUrl, new Response.Listener<Bitmap>() {
//            @Override
//            public void onResponse(final Bitmap arg0) {
//                mHandler.post(new Runnable() {
//                    @Override
//                    public void run() {
//                        imageView.setImageBitmap(arg0);
//                    }
//                });
//            }
//        }, maxWidth, maxHeight, Bitmap.Config.ARGB_8888, new Response.ErrorListener() {
//            @Override
//            public void onErrorResponse(VolleyError arg0) {
//            }
//        });
//        mQueue.add(imgRequest);
//    }
//
//    /**
//     * 获取底部导航栏高度
//     */
//    private int getNavigationBarHeight(Context context) {
//        Resources resources = context.getResources();
//        int resourceId = resources.getIdentifier("navigation_bar_height", "dimen", "android");
//        int height = resources.getDimensionPixelSize(resourceId);
//        if (height < 0) {
//            height = 0;
//        }
//        return height;
//    }
//}
