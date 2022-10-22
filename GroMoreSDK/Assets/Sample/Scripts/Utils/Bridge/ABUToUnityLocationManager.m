//
//  ABUToUnityLocationManager.m
//  UnityFramework
//
//  Created by ByteDance on 2022/3/21.
//

#import "ABUToUnityLocationManager.h"
#import <CoreLocation/CoreLocation.h>

@interface ABUToUnityLocationManager()<CLLocationManagerDelegate>

@property (nonatomic, strong) CLLocationManager *locationManager;

@end


@implementation ABUToUnityLocationManager

+ (ABUToUnityLocationManager *)sharedManager {
    static id _shared = nil;
    static dispatch_once_t once;
    dispatch_once(&once, ^{
        _shared = [[ABUToUnityLocationManager alloc] init];
    });
    return _shared;
}

- (CLLocationManager *)locationManager {
    if (_locationManager == nil) {
        _locationManager = [[CLLocationManager alloc] init];
        _locationManager.delegate = self;
        _locationManager.desiredAccuracy = kCLLocationAccuracyBest;
    }
    return _locationManager;
}

-(void)startLocating{
    if([self.locationManager respondsToSelector:@selector(requestWhenInUseAuthorization)]){
        [self.locationManager requestWhenInUseAuthorization];
    }
    [self.locationManager startUpdatingLocation];   //开始定位
}

/* 定位完成后 回调 */
-(void)locationManager:(CLLocationManager *)manager didUpdateLocations:(NSArray<CLLocation *> *)locations{
    
    CLLocation *location = [locations lastObject];
    
    CLLocationCoordinate2D coordinate = location.coordinate;
    //  经纬度
    NSLog(@"---x:%f---y:%f",coordinate.latitude,coordinate.longitude);
    
    [manager stopUpdatingLocation];   //停止定位
}

/* 定位失败后 回调 */
-(void)locationManager:(CLLocationManager *)manager didFailWithError:(NSError *)error{
    if (error.code == kCLErrorDenied) {
        // 提示用户出错
    }
}

/* 监听用户授权状态 */
-(void)locationManager:(CLLocationManager *)manager didChangeAuthorizationStatus:(CLAuthorizationStatus)status{
       
}


@end
