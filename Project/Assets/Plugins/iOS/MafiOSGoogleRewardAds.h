//
//  MafiOSGoogleRewardAds.h
//  MoneyHero
//
//  Created by SanHeo on 2016. 5. 9..
//
//

#import <GoogleMobileAds/GoogleMobileAds.h>

@interface MafiOSGoogleRewardAds : NSObject<GADFullScreenContentDelegate>
{
    NSString *strAdmobOpenAdId;
}

@property (retain, nonatomic) NSString *strAdmobOpenAdId;
@property(nonatomic, strong) GADAppOpenAd* appOpenAd;

+ (MafiOSGoogleRewardAds*) sharediOSGoogleRewardAdsPlugin;

- (void) initGoogleRewardOpenAd:(NSString*)strId;
- (void) showGoogleRewardOpenAd;
- (void) loadRewardOpenAd;

@end
