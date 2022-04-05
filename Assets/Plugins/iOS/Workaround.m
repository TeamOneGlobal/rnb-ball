#import <GoogleMobileAds/GoogleMobileAds.h>
#import "GADUTypes.h"

// Workaround for https://github.com/googleads/googleads-mobile-unity/issues/1616

void GADURequestInterstitial(GADUTypeInterstitialRef interstitial, GADUTypeRequestRef request) { }

void GADURequestRewardedAd(GADUTypeRewardedAdRef rewardedAd, GADUTypeRequestRef request) { }
