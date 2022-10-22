//
//  ABUDCustomDrawView.m
//  ABUDemo
//
//  Created by ByteDance on 2022/5/6.
//  Copyright © 2022 bytedance. All rights reserved.
//

#import "ABUDCustomDrawView.h"

@implementation ABUDCustomDrawView

- (instancetype)init {
    if (self = [super init]) {
        self.backgroundColor = [UIColor lightGrayColor];
    }
    return self;
}

- (void)didMoveToSuperview {
    [super didMoveToSuperview];
    if (self.superview && self.didMoveToSuperViewCallback) {
        self.didMoveToSuperViewCallback(self);
        self.didMoveToSuperViewCallback = nil;
    }
}

@end
