//
//  MafiOSUnityAds.m
//  NonsenseQuiz
//
//  Created by SanHeo on 2015. 2. 17..
//
//
#import "MafiOSGoogleRewardAds.h"

@implementation MafiOSGoogleRewardAds

@synthesize strAdmobOpenAdId;
@synthesize appOpenAd;


//static const char *RESULT_SUCCESS = "1";
//static const char *RESULT_FAIL = "0";

extern "C" {
    void IOSInitGoogleOpenAd(const char* strAdmobId)
    {
        NSString *str = [[NSString alloc]initWithUTF8String:strAdmobId];
//        NSString *str = [MafiOSGoogleRewardAds CreateNSString:strAdmobId];
        [[MafiOSGoogleRewardAds sharediOSGoogleRewardAdsPlugin] initGoogleRewardOpenAd: str];
    }
    void IOSShowGoogleOpenAd()
    {
        [[MafiOSGoogleRewardAds sharediOSGoogleRewardAdsPlugin] showGoogleRewardOpenAd];
    }
    
}


+ (MafiOSGoogleRewardAds*) sharediOSGoogleRewardAdsPlugin
{
    static MafiOSGoogleRewardAds* pInstance;
    if(pInstance == NULL)
    {
        pInstance = [[MafiOSGoogleRewardAds alloc] init];
    }
    return pInstance;
}

+ (NSString*) CreateNSString:(const char*)string
{
    if (string != NULL)
        return [NSString stringWithUTF8String:string];
    else
        return [NSString stringWithUTF8String:""];
}

#pragma mark - app open ads

- (void) initGoogleRewardOpenAd:(NSString*)strId{
    NSLog(@"initGoogleRewardOpenAd");
    self.strAdmobOpenAdId =  [[NSString alloc] initWithString:strId];
    [self loadRewardOpenAd];
    
}
- (void) showGoogleRewardOpenAd{
    GADAppOpenAd *ad = self.appOpenAd;
//    self.appOpenAd = nil;
    NSLog(@"showGoogleRewardOpenAd");
    if (ad) {
        [ad presentFromRootViewController:UnityGetGLViewController()];

    } else {
    // If you don't have an ad ready, request one.
        [self loadRewardOpenAd];
    }
}

- (void) loadRewardOpenAd{
    self.appOpenAd = nil;
    [GADAppOpenAd loadWithAdUnitID:self.strAdmobOpenAdId request:[GADRequest request] orientation:UIInterfaceOrientationPortrait completionHandler:^(GADAppOpenAd *_Nullable appOpenAd, NSError *_Nullable error)
    {
        if (error) {
            NSLog(@"Failed to load app open ad: %@", error);
            UnitySendMessage("AdsHelper", "NativeAppOpenAdLoaded", "0");
           return;
        }
        
        UnitySendMessage("AdsHelper", "NativeAppOpenAdLoaded", "1");
        NSLog(@"loadRewardOpenAd");
        self.appOpenAd = appOpenAd;
        self.appOpenAd.fullScreenContentDelegate = self;
          
    }];
}

/// Tells the delegate that the ad failed to present full screen content.
- (void)ad:(nonnull id<GADFullScreenPresentingAd>)ad
didFailToPresentFullScreenContentWithError:(nonnull NSError *)error{
    // NSLog(@"didFailToPresentFullScreenContentWithError: %@", error);
}

/// Tells the delegate that the ad presented full screen content.
- (void)adDidPresentFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad{
    // NSLog(@"adDidPresentFullScreenContent");
}

/// Tells the delegate that the ad dismissed full screen content.
- (void)adDidDismissFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad{
    // NSLog(@"adDidDismissFullScreenContent");
    [self loadRewardOpenAd];
}

@end
