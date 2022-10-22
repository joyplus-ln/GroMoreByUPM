//
//  BridgeTools.m
//  UnityFramework
//
//  Created by bytedance on 2020/11/4.
//

#import <Foundation/Foundation.h>

#import "ABUToUnityLocationManager.h"

#if defined (__cplusplus)
extern "C" {
#endif

UIWindow *mainWindow() {
    UIWindow *window = nil;
    if ([[UIApplication sharedApplication].delegate respondsToSelector:@selector(window)]) {
        window = [[UIApplication sharedApplication].delegate window];
    }
    if (![window isKindOfClass:[UIView class]]) {
        window = [UIApplication sharedApplication].keyWindow;
    }
    if (!window) {
        window = [[UIApplication sharedApplication].windows objectAtIndex:0];
    }
    return window;
}

UIScreen *screen() {
    return [UIScreen mainScreen];
}

float UnionPlatform_BridgeTools_GetScreenWidth() {
    return screen().bounds.size.width * screen().scale;
}

float UnionPlatform_BridgeTools_GetScreenHeight() {
    return screen().bounds.size.height * screen().scale;
}

float UnionPlatform_BridgeTools_SafeAreaInsets_Top() {
    UIWindow *window = mainWindow();
    if (@available(iOS 11.0, *)) {
        return window.safeAreaInsets.top * screen().scale;
    } else {
        return 0;
    }
}

float UnionPlatform_BridgeTools_SafeAreaInsets_Left() {
    UIWindow *window = mainWindow();
    if (@available(iOS 11.0, *)) {
        return window.safeAreaInsets.left * screen().scale;
    } else {
        return 0;
    }
}

float UnionPlatform_BridgeTools_SafeAreaInsets_Bottom() {
    UIWindow *window = mainWindow();
    if (@available(iOS 11.0, *)) {
        return window.safeAreaInsets.bottom * screen().scale;
    } else {
        return 0;
    }
}

float UnionPlatform_BridgeTools_SafeAreaInsets_Right() {
    UIWindow *window = mainWindow();
    if (@available(iOS 11.0, *)) {
        return window.safeAreaInsets.right * screen().scale;
    } else {
        return 0;
    }
}

void UnionPlatform_BridgeTools_getSystemLocationPrivilege() {
    [ABUToUnityLocationManager.sharedManager startLocating];
}

#if defined (__cplusplus)
}
#endif


