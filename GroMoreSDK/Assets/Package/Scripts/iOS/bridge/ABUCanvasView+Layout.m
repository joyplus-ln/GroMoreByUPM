//
//  ABUCanvasView+Layout.m
//  UnityFramework
//
//  Created by ByteDance on 2022/7/13.
//

#import "ABUCanvasView+Layout.h"

static CGFloat const margin = 15;
static CGSize const logoSize = { 15, 15 };
static UIEdgeInsets const padding = { 10, 15, 10, 15 };

@implementation ABUCanvasView (Layout)

- (void)exampleLayoutWithFrame:(CGRect)frame {
    //    NSInteger index = [self.selfs indexOfObject:self];
    UIImageView *imageView1 = [[UIImageView alloc] init];
    UIImageView *imageView2 = [[UIImageView alloc] init];
    self.frame = frame;
    
    CGFloat width = CGRectGetWidth(self.bounds);
    CGFloat contentWidth = (width - 2 * margin);
    CGFloat y = padding.top;
    
    // 确定是自渲染才会调用该方法
    //    self.descLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, y, contentWidth, 40)];
    self.descLabel.text = self.data.AdTitle;
    self.descLabel.backgroundColor = [UIColor grayColor];
    y += 40;
    y += 5;
    
    CGFloat leftMargin = frame.size.width/20;
    if (self.data.icon.imageURL) {
        CGFloat cusIconWidth = 30;
        CGFloat cusIconHeight = 30;
        
        self.iconImageView = [[UIImageView alloc] initWithFrame:CGRectMake(0, y, cusIconWidth, cusIconHeight)];
        UIImage *imagePic = [UIImage imageWithData:[NSData dataWithContentsOfURL:self.data.icon.imageURL]];
        [self.iconImageView setImage:imagePic];
        
        self.descLabel.frame = CGRectMake(CGRectGetMaxX(self.iconImageView.frame), CGRectGetMinY(self.iconImageView.frame), frame.size.width - leftMargin - CGRectGetMaxX(self.iconImageView.frame), CGRectGetHeight(self.iconImageView.frame));
    }
    
    CGFloat dislikeX = width - 24;
    // 物料信息可能不包含关闭按钮需要自己实现
    if (!self.dislikeBtn) {
        self.dislikeBtn = [[UIButton alloc] init];
        [self.dislikeBtn setImage:[UIImage imageNamed:@"feedClose"] forState:UIControlStateNormal];
        self.dislikeBtn.backgroundColor = [UIColor cyanColor];
        self.dislikeBtn.userInteractionEnabled = YES;
    }
    self.dislikeBtn.frame = CGRectMake(dislikeX-24, 0, 24, 24);
    
    CGFloat originInfoX = padding.left;
    if (self.adLogoView) {
        self.adLogoView.frame = CGRectMake(originInfoX, y + 3, 26, 14);
        originInfoX += 24;
        originInfoX += 10;
    }
    
    self.titleLabel.text = self.data.AdDescription;
    self.titleLabel.frame = CGRectMake(0, CGRectGetMaxY(self.descLabel.frame), contentWidth, 40);
    self.titleLabel.backgroundColor = [UIColor grayColor];
    ABUMaterialMeta *adMeta = self.data;
    
    if (self.hasSupportActionBtn) {
        CGFloat customBtnWidth = 100;
        self.callToActionBtn.frame = CGRectMake(dislikeX - customBtnWidth - 5, CGRectGetMaxY(self.titleLabel.frame), customBtnWidth, 20);
        NSString *btnTxt = @"Click";
        if (self.data.buttonText.length > 0) {
            btnTxt = self.data.buttonText;
        }
        [self.callToActionBtn setTitle:btnTxt forState:UIControlStateNormal];
        self.callToActionBtn.backgroundColor = [UIColor redColor];
    }
    
    // imageMode decides whether to show video or not
    if (adMeta.imageMode == ABUFeedVideoAdModeImage) {
        self.imageView.hidden = YES;
        if (self.mediaView) {
            ABUImage *image = self.data.imageList.firstObject;
            const CGFloat imageHeight = contentWidth * (image.height / image.width);
            self.mediaView.frame = CGRectMake(0, CGRectGetMaxY(self.titleLabel.frame), contentWidth, imageHeight);
        }
    } else if (adMeta.imageMode == ABUFeedADModeLargeImage) {
        self.imageView.hidden = NO;
        if (adMeta.imageList.count > 0) {
            ABUImage *image = self.data.imageList.firstObject;
            const CGFloat imageHeight = contentWidth * (image.height / image.width);
            if (image.imageURL.absoluteString.length > 0) {
                
                self.imageView.frame = CGRectMake(5, CGRectGetMaxY(self.titleLabel.frame), contentWidth, imageHeight);
                UIImage *imagePic = [UIImage imageWithData:[NSData dataWithContentsOfURL:image.imageURL]];
                [self.imageView setImage:imagePic];
            }
        }
    } else if (adMeta.imageMode == ABUFeedADModeGroupImage) {
        self.imageView.hidden = NO;
        CGFloat y = CGRectGetMaxY(self.titleLabel.frame);
        if (self.callToActionBtn.frame.origin.y != 0) {
            y = CGRectGetMaxY(self.callToActionBtn.frame);
        }
        if (adMeta.imageList.count > 1) {
            CGFloat imageWidth = (contentWidth - 5 * 2) / 3;
            ABUImage *image = adMeta.imageList[1];
            const CGFloat imageHeight = imageWidth * (image.height / image.width);
            if (image.imageURL.absoluteString.length > 0) {
                self.imageView.frame = CGRectMake(5, y + 5, imageWidth, imageHeight);
                UIImage *imagePic = [UIImage imageWithData:[NSData dataWithContentsOfURL:image.imageURL]];
                [self.imageView setImage:imagePic];
            }
        }
        if (adMeta.imageList.count > 2) {
            CGFloat imageWidth = (contentWidth - 5 * 2) / 3;
            ABUImage *image = adMeta.imageList[2];
            const CGFloat imageHeight = imageWidth * (image.height / image.width);
            if (image.imageURL.absoluteString.length > 0) {
                
                imageView1.frame = CGRectMake(5+imageWidth+10, y + 5, imageWidth, imageHeight);
                UIImage *imagePic = [UIImage imageWithData:[NSData dataWithContentsOfURL:image.imageURL]];
                [imageView1 setImage:imagePic];
                [self addSubview:imageView1];
            }
        }
        if (adMeta.imageList.count > 3) {
            CGFloat imageWidth = (contentWidth - 5 * 2) / 3;
            ABUImage *image = adMeta.imageList[3];
            const CGFloat imageHeight = imageWidth * (image.height / image.width);
            if (image.imageURL.absoluteString.length > 0) {
                imageView2.frame = CGRectMake(5+imageWidth*2+10+10, y + 5, imageWidth, imageHeight);
                UIImage *imagePic = [UIImage imageWithData:[NSData dataWithContentsOfURL:image.imageURL]];
                [imageView2 setImage:imagePic];
                [self addSubview:imageView2];
            }
        }
    }
    
    [self bringSubviewToFront:self.dislikeBtn];
    self.frame = CGRectMake(frame.origin.x, frame.origin.y, frame.size.width, CGRectGetHeight(self.titleLabel.frame)+CGRectGetHeight(self.descLabel.frame)+CGRectGetHeight(self.descLabel.frame)+CGRectGetHeight(self.mediaView.frame)+CGRectGetHeight(self.descLabel.frame)+CGRectGetHeight(self.imageView.frame)+CGRectGetHeight(self.descLabel.frame)+CGRectGetHeight(self.callToActionBtn.frame)+CGRectGetHeight(self.callToActionBtn.frame)+CGRectGetHeight(self.dislikeBtn.frame));
    
    // Register UIView with the native ad; the whole UIView will be clickable.
    [self registerClickableViews:@[self.callToActionBtn,
                                   self.titleLabel,
                                   self.descLabel,
                                   self.imageView,
                                   imageView1,
                                   imageView2]];
    
}

@end
