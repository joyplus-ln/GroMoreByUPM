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
// //?????????????????????????????????
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
//                    // wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE; //?????????????????????
//                    // FLAG_NOT_FOCUSABLE??????????????????????????????
//                    wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL;
//                    wmParams.width = 500;
//                    wmParams.height = 400;
//                    // ??????Banner ??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
//                    wmParams.y = getNavigationBarHeight(mContext);
//                    wmParams.format = PixelFormat.RGBA_8888;
//                    getNavigationBarHeight(mContext);
//                }
//                removeBannerView();
//                mBannerView = new BannerView(context);
//                addBannerView();
//                // ???????????????????????????
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
//    //????????????????????????????????????????????????????????????destory???????????????
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
//        // ???????????????????????????????????????????????????????????????
//        LGNativeAd.InteractionType type = nativeAd.getInteractionType();
//        if (type == LGNativeAd.InteractionType.DOWNLOAD) {
//            // ???????????????ttAdManager.createAdNative(getApplicationContext())????????????activity
//            // ??????????????????activity?????????????????????Dislike??????
//            nativeAd.setActivityForDownloadApp((Activity) mContext);
//            mCreativeButton.setVisibility(View.VISIBLE);
//            nativeAd.setDownloadCallback(new MyDownloadListener(mCreativeButton)); // ?????????????????????
//        } else if (type == LGNativeAd.InteractionType.DIAL) {
//            mCreativeButton.setVisibility(View.VISIBLE);
//            mCreativeButton.setText("????????????");
//        } else if (type == LGNativeAd.InteractionType.LANDING_PAGE
//                || type == LGNativeAd.InteractionType.BROWSER) {
//            mCreativeButton.setVisibility(View.VISIBLE);
//            mCreativeButton.setText("????????????");
//        } else {
//            mCreativeButton.setVisibility(View.GONE);
//        }
//
//        // ??????????????????view, ????????????nativeView?????????????????????????????????????????????
//        List<View> clickViewList = new ArrayList<>();
//        clickViewList.add(nativeView);
//
//        // ?????????????????????view?????????????????????????????????
//        List<View> creativeViewList = new ArrayList<>();
//        // ????????????????????????????????????????????????????????????????????????????????????????????????view??????
//        // creativeViewList.add(nativeView);
//        creativeViewList.add(mCreativeButton);
//
//        // ??????! ???????????????????????????????????????????????????convertView????????????ViewGroup???
//        nativeAd.registerViewForInteraction((ViewGroup) nativeView, clickViewList, creativeViewList, dislike,
//                new LGNativeAd.AdInteractionCallback() {
//                    @Override
//                    public void onAdClicked(View view, LGNativeAd ad) {
//                        if (ad != null) {
//                            Toast.makeText(mContext, "??????" + ad.getTitle() + "?????????", Toast.LENGTH_SHORT).show();
//                        }
//                        removeBannerView();
//                    }
//
//                    @Override
//                    public void onAdCreativeClick(View view, LGNativeAd ad) {
//                        if (ad != null) {
//                            Toast.makeText(mContext, "??????" + ad.getTitle() + "????????????????????????", Toast.LENGTH_SHORT).show();
//                        }
//                        removeBannerView();
//                    }
//
//                    @Override
//                    public void onAdShow(LGNativeAd ad) {
//                        // ????????????????????????View ??????????????????????????????????????????????????????????????????????????????????????????
//
//                        wmParams.flags = WindowManager.LayoutParams.FLAG_NOT_TOUCH_MODAL
//                                | WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE;
//                        mWindowManager.updateViewLayout(mBannerView, wmParams);
//                        if (ad != null) {
//                            Toast.makeText(mContext, "??????" + ad.getTitle() + "??????", Toast.LENGTH_SHORT).show();
//                        }
//                    }
//                });
//    }
//
//    // ???????????????dislike ?????????????????????????????????????????????
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
//                mDownloadButton.setText("????????????");
//            }
//        }
//
//        @SuppressLint("SetTextI18n")
//        @Override
//        public void onDownloadActive(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                if (totalBytes <= 0L) {
//                    mDownloadButton.setText("????????? percent: 0");
//                } else {
//                    mDownloadButton.setText("????????? percent: " + (currBytes * 100 / totalBytes));
//                }
//            }
//        }
//
//        @SuppressLint("SetTextI18n")
//        @Override
//        public void onDownloadPaused(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("???????????? percent: " + (currBytes * 100 / totalBytes));
//            }
//        }
//
//        @Override
//        public void onDownloadFailed(long totalBytes, long currBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("????????????");
//            }
//        }
//
//        @Override
//        public void onInstalled(String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("????????????");
//            }
//        }
//
//        @Override
//        public void onDownloadFinished(long totalBytes, String fileName, String appName) {
//            if (mDownloadButton != null) {
//                mDownloadButton.setText("????????????");
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
//     * ???????????????????????????
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
