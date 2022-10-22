package com.bytedance.ad.sdk.mediation;

import java.io.IOException;
import java.io.InputStream;

import android.content.Context;
import android.content.res.AssetManager;
import android.graphics.drawable.Drawable;
import android.util.Log;

public class MResource {
    /**
     * 根据资源的名字获取其ID值
     *
     * @author mining
     *
     */
    public static int getIdByName(Context context, String className, String name) {
        //此处的包名不能通过context获取，因为在unity中这样获取到的是unity额外设定的包名，不是jar包里面的包名
        //String packageName = "com.company.product";
        String packageName = context.getPackageName();
        Class r = null;
        int id = 0;
        try {
            r = Class.forName(packageName + ".R");

            Class[] classes = r.getClasses();
            Class desireClass = null;

            for (int i = 0; i < classes.length; ++i) {
                if (classes[i].getName().split("\\$")[1].equals(className)) {
                    desireClass = classes[i];
                    break;
                }
            }

            if (desireClass != null)
                id = desireClass.getField(name).getInt(desireClass);
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (IllegalArgumentException e) {
            e.printStackTrace();
        } catch (SecurityException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        } catch (NoSuchFieldException e) {
            e.printStackTrace();
        }

        return id;
    }

    public static int[] getIdsByName(Context context, String className,
                                     String name) {
        String packageName = context.getPackageName();
        Class r = null;
        int[] ids = null;
        try {
            r = Class.forName(packageName + ".R");

            Class[] classes = r.getClasses();
            Class desireClass = null;

            for (int i = 0; i < classes.length; ++i) {
                if (classes[i].getName().split("\\$")[1].equals(className)) {
                    desireClass = classes[i];
                    break;
                }
            }

            if ((desireClass != null)
                    && (desireClass.getField(name).get(desireClass) != null)
                    && (desireClass.getField(name).get(desireClass).getClass()
                    .isArray()))
                ids = (int[]) desireClass.getField(name).get(desireClass);
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (IllegalArgumentException e) {
            e.printStackTrace();
        } catch (SecurityException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        } catch (NoSuchFieldException e) {
            e.printStackTrace();
        }

        return ids;
    }

    public static Drawable getDrawableFromAssets(Context context,
                                                 String imageFileName) {
        Drawable result = null;
        AssetManager assetManager = context.getAssets();
        InputStream is = null;
        try {
            is = assetManager.open(imageFileName);
            result = Drawable.createFromStream(is, null);
            is.close();
            is = null;
        } catch (IOException e) {
            e.printStackTrace();
        }
        return result;
    }
}