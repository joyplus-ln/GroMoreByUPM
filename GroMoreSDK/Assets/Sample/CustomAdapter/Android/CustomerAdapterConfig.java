package com.bytedance.ad.sdk.mediation.adapter;

import android.content.Context;
import android.util.Log;

import com.bytedance.msdk.api.v2.ad.custom.bean.GMCustomInitConfig;
import com.bytedance.msdk.api.v2.ad.custom.init.GMCustomAdapterConfiguration;
import com.bytedance.sdk.openadsdk.TTAdConfig;
import com.bytedance.sdk.openadsdk.TTAdSdk;

import java.lang.reflect.Field;
import java.util.Map;

public class CustomerAdapterConfig extends GMCustomAdapterConfiguration {

    private static final String TAG = "<Unity Log>";

    @Override
    public void initializeADN(Context context, GMCustomInitConfig gmCustomConfig, Map<String, Object> localExtra) {
        Log.i(TAG, "call initializeADN localExtra = " + localExtra);
        TTAdConfig ttAdConfig = new TTAdConfig.Builder()
                .appId(gmCustomConfig.getAppId())
                .appName("")
                .debug(true) //测试阶段打开，可以通过日志排查问题，上线时去除该调用
                .build();
        if (isPanglePluginSdk()) {
            TTAdSdk.init(context, ttAdConfig, new TTAdSdk.InitCallback() {
                @Override
                public void success() {
                    Log.i(TAG, "穿山甲初始化成功 " + Thread.currentThread().getName());
                }

                @Override
                public void fail(int i, String s) {
                    Log.i(TAG, "穿山甲初始化失败 " + Thread.currentThread().getName());
                }
            });
        } else {
            TTAdSdk.init(context, ttAdConfig);
        }

        Log.i(TAG, "CustomerAdapterConfig " + Thread.currentThread().getName());
        callInitSuccess();
    }

    private boolean isPanglePluginSdk() {
        boolean isPlugin = false;
        try {
            Class<?> aClass = Class.forName("com.bytedance.sdk.openadsdk.TTAdConstant");
            Field isPField = aClass.getDeclaredField("IS_P");
            isPField.setAccessible(true);
            isPlugin = (boolean) isPField.get(aClass);
        } catch (Throwable e) {
            e.printStackTrace();
        }
        return isPlugin;
    }

    @Override
    public String getNetworkSdkVersion() {
        return "4.0.0.0";
    }

    @Override
    public String getAdapterSdkVersion() {
        return "1.0.0";
    }

}
