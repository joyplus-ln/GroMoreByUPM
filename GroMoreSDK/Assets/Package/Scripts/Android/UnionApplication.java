package com.bytedance.ad.sdk.mediation;

import android.app.Application;
import android.content.Context;
import android.support.multidex.MultiDex;
import com.bytedance.msdk.api.TTAdConfig;
import com.bytedance.msdk.api.TTAdConstant;
import com.bytedance.msdk.api.TTMediationAdSdk;
import com.bytedance.msdk.api.UserInfoForSegment;


public class UnionApplication  extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        TTMediationAdSdk.initialize(this, buildConfig());
    }


    private static TTAdConfig buildConfig() {
        UserInfoForSegment userInfo = new UserInfoForSegment();
        userInfo.setUserId("msdk demo");
        userInfo.setGender(UserInfoForSegment.GENDER_MALE);
        userInfo.setChannel("msdk channel");
        userInfo.setSubChannel("msdk sub channel");
        userInfo.setAge(999);
        userInfo.setUserValueGroup("msdk demo user value group");
        return new TTAdConfig.Builder()
                .appId("5001121")
                .appName("APP测试媒体")
                .openAdnTest(false)//开启第三方ADN测试时需要设置为true，会每次重新拉去最新配置，release 包情况下必须关闭.默认false
                .isPanglePaid(false)//是否为费用户
                .openDebugLog(true) //测试阶段打开，可以通过日志排查问题，上线时去除该调用
                .usePangleTextureView(true) //使用TextureView控件播放视频,默认为SurfaceView,当有SurfaceView冲突的场景，可以使用TextureView
                .setPangleTitleBarTheme(TTAdConstant.TITLE_BAR_THEME_DARK)
                .allowPangleShowNotify(true) //是否允许sdk展示通知栏提示
                .allowPangleShowPageWhenScreenLock(true) //是否在锁屏场景支持展示广告落地页
                .setPangleDirectDownloadNetworkType(TTAdConstant.NETWORK_STATE_WIFI, TTAdConstant.NETWORK_STATE_3G) //允许直接下载的网络状态集合
                .needPangleClearTaskReset()//特殊机型过滤，部分机型出现包解析失败问题（大部分是三星）。参数取android.os.Build.MODEL
                .setUserInfoForSegment(userInfo) // 设置流量分组的信息
                .build();
    }

    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(base);
    }
}
