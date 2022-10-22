//
//  ABUToUnityAdSDK.m
//  UnityFramework
//
//  Created by CHAORS on 2021/6/29.
//

#import <ABUAdSDK/ABUAdSDK.h>
#import <objc/runtime.h>
#import <objc/message.h>

extern const char* AutonomousStringCopy(const char* string);
const char* AutonomousStringCopy(const char* string) {
    if (string == NULL) {
        return NULL;
    }
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

#if defined (__cplusplus)
extern "C" {
#endif

void UnionPlatform_setUserInfoForSegment(int age,
                                         int  gender,
                                         const char * userID,
                                         const char * channel,
                                         const char * subChannel,
                                         const char * userGroup,
                                         const char * customInfos) {
    ABUUserInfoForSegment *segment = [[ABUUserInfoForSegment alloc] init];
    if (userID != NULL) {
        NSString *userIDStr = [[NSString alloc] initWithUTF8String:userID];
        if (userIDStr.length > 0) {
            segment.user_id = userIDStr;
        }
    }
    if (channel != NULL) {
        NSString *channelStr = [[NSString alloc] initWithUTF8String:channel];
        if (channelStr.length > 0) {
            segment.channel = channelStr;
        }
    }
    if (subChannel != NULL) {
        NSString *subChannelStr = [[NSString alloc] initWithUTF8String:subChannel];
        if (subChannelStr.length > 0) {
            segment.sub_channel = subChannelStr;
        }
    }
    segment.age = age;
    segment.gender = (ABUUserInfo_Gender)gender;
    if (userGroup != NULL) {
        NSString *userGroupStr = [[NSString alloc] initWithUTF8String:userGroup];
        if (userGroupStr.length > 0) {
            segment.user_value_group = userGroupStr;
        }
    }
    if (customInfos) {
        NSString *customInfoStr = [[NSString alloc] initWithUTF8String:customInfos];
        NSData *data = [customInfoStr dataUsingEncoding:NSUTF8StringEncoding];
        NSError *error = nil;
        NSDictionary *customInfodic = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&error];
        if (customInfodic.count > 0) {
            segment.customized_id = customInfodic;
        }
    }
    if (segment) {
        [ABUAdSDKManager setUserInfoForSegment:segment];
    }
}

void UnionPlatform_LauchVisualDebugTool() {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wundeclared-selector"
    SEL sel = @selector(startVisualDebug);
    Class ABUVisualDebugCls = NSClassFromString(@"ABUVisualDebug");
    if ([ABUVisualDebugCls respondsToSelector:sel]) {
#pragma clang diagnostic ignored "-Warc-performSelector-leaks"
        [ABUVisualDebugCls performSelector:sel];
    }
#pragma clang diagnostic pop
}

void UnionPlatform_SetThemeStatusIfCan(int themeStatus) {
    [ABUAdSDKManager setThemeStatus:(ABUAdSDKThemeStatus)themeStatus];
}

void UnionPlatform_SetPrivacyConfigForKeyValue(const char *key,
                                               double value) {
    if (!key) {
        return;
    }
    NSString *keyStr = [[NSString alloc] initWithUTF8String:key];
    NSNumber *valueNum = [NSNumber numberWithDouble:value];
    [ABUPrivacyConfig setPrivacyWithKey:keyStr andValue:valueNum];
}


void UnionPlatform_SetPublisherDid(const char *publisherDid) {
    if (!publisherDid) {
        return;
    }
    NSString *publisherDidStr = [[NSString alloc] initWithUTF8String:publisherDid];
    publisherDidStr = [publisherDidStr stringByReplacingOccurrencesOfString:@"\r" withString:@""];
    publisherDidStr = [publisherDidStr stringByReplacingOccurrencesOfString:@"\n" withString:@""];
    publisherDidStr = [publisherDidStr stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]]; //去除掉首尾的空白字符和换行字符使用
    publisherDidStr = [publisherDidStr stringByReplacingOccurrencesOfString:@" " withString:@""];
    [ABUAdSDKManager setExtDeviceData:publisherDidStr];
}

#if defined (__cplusplus)
}
#endif
