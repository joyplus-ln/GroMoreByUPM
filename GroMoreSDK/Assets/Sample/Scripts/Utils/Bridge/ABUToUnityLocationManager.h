//
//  ABUToUnityLocationManager.h
//  UnityFramework
//
//  Created by ByteDance on 2022/3/21.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface ABUToUnityLocationManager : NSObject

+ (ABUToUnityLocationManager *)sharedManager;

-(void)startLocating;

@end

NS_ASSUME_NONNULL_END
