//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

#import "UnityAppController.h"
#import <ABUAdSDK/ABUAdSDK.h>

#import <ABUAdAdmobAdapter/ABUAdAdmobAdapter.h>
#import <GoogleMobileAds/GoogleMobileAds.h>

static NSString * const MopubADUnitID = @"e1cbce0838a142ec9bc2ee48123fd470";

@interface UnionAppController : UnityAppController 
@property (nonatomic, assign) CFTimeInterval startTime;
@end

IMPL_APP_CONTROLLER_SUBCLASS (UnionAppController)

@implementation UnionAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    // Override point for customization after application launch.
    [super application:application didFinishLaunchingWithOptions:launchOptions];

    // 第三方开发者在这设置GADMobileAds一些个性化配置，主要为一些聚合SDK并未统一封装的配置接口(如果聚合SDK已提供，请勿在此设置！！！)
    // 不要在configAdapterWithBlock写初始化代码！！！聚合SDK已完成初始化
    // 开发者按需在此进行三方sdk的个性化配置
    [ABUPersonaliseConfigAdapter configAdapterWithKey:@"admob" andBlock:^{
        [GADMobileAds sharedInstance].applicationMuted = YES;
        // 支持模拟器和测试设备，Admob测试广告需要添加测试设备ID
        [GADMobileAds sharedInstance].requestConfiguration.testDeviceIdentifiers = @[
            GADSimulatorID,
            @"7332fdb574f3388c808b8ef2d459cd32",
            @"1ecdf6f84db8765e67fdf03913acb09b",
            @"b816051b870e7420f281f67dc9e21bb8",
            @"cadfea09bfef45bb3b5db362686e5c61",
            @"6c4c02793b387807b6ce4de330cfb05f",
            @"ca39efeb08a3eb5403440e316389f83a",
            @"fd08ac1ee543195d8b029aaf95a18034",
            @"e622fa43c526351643d773bb1c4255aa"
        ];
    }];
    
    // SDK初始化接口
    // 设置三方使用的applogID/唯一标识id/设备id等相关信息，如无必要请勿设置其他。用法见接口说明
    NSDictionary *didDic = @{ @"device_id" : @"1234567" };// !!!建议在初始化时设置；如需更改后期调用updateExtraDeviceMap
    [ABUAdSDKManager setupSDKWithAppId:@"5000546" config:^ABUUserConfig *(ABUUserConfig *c) {
        c.logEnable = YES;
        c.extraDeviceMap = didDic;
        return c;
    }]; 
    
    return YES;
}

@end
