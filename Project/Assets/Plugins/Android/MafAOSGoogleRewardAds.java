
import com.google.android.gms.ads.AdError;
import com.google.android.gms.ads.FullScreenContentCallback;
import com.unity3d.player.UnityPlayer;

import android.util.Log;

import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.LoadAdError;
import com.google.android.gms.ads.appopen.AppOpenAd;

public class MafAOSGoogleRewardAds {

    private static AppOpenAd appOpenAd = null;
    private static String AD_UNIT_ID = "";
    private static boolean isShowingAd = false;

    private static AppOpenAd.AppOpenAdLoadCallback loadCallback;

    static final String RESULT_SUCCESS = "1";
    static final String RESULT_FAIL = "0";

    public static void AOSInitGoogleOpenAd(String strAdmobId)
    {
        MafAOSGoogleRewardAds.AD_UNIT_ID = strAdmobId;
        Log.d("TEST", "AOSInitGoogleOpenAd");
        fetchAd();
    }

    public static void AOSShowGoogleOpenAd()
    {
        Log.d("TEST", "AOSShowGoogleOpenAd");
        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            public void run() {
                if (!isShowingAd && isAdAvailable()) {

                    FullScreenContentCallback fullScreenContentCallback =
                            new FullScreenContentCallback() {
                                @Override
                                public void onAdDismissedFullScreenContent() {
                                    // Set the reference to null so isAdAvailable() returns false.
                                    appOpenAd = null;
                                    isShowingAd = false;
                                    fetchAd();
                                }

                                @Override
                                public void onAdFailedToShowFullScreenContent(AdError adError) {}
                                @Override
                                public void onAdShowedFullScreenContent() {
                                    isShowingAd = true;
                                }
                            };

                    appOpenAd.show(UnityPlayer.currentActivity, fullScreenContentCallback);

                } else {
                    fetchAd();
                }
            }
        });
    }

    /** Request an ad */
    public static void fetchAd() {
        // Have unused ad, no need to fetch another.

        UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
            public void run() {
                if (isAdAvailable()) {
                    Log.d("TEST", "fetchAd isAdAvailable");
                    return;
                }

                loadCallback =  new AppOpenAd.AppOpenAdLoadCallback() {

                            @Override
                            public void onAppOpenAdLoaded(AppOpenAd ad) {
                                appOpenAd = ad;
                                Log.d("TEST", "onAppOpenAdLoaded");
                                UnityPlayer.UnitySendMessage("Main", "NativeAppOpenAdLoaded", RESULT_SUCCESS);
                            }
                            @Override
                            public void onAppOpenAdFailedToLoad(LoadAdError loadAdError) {
                                // Handle the error.
                                Log.d("TEST", "onAppOpenAdFailedToLoad");
                                UnityPlayer.UnitySendMessage("Main", "NativeAppOpenAdLoaded", RESULT_FAIL);
                            }

                        };
                AdRequest request = getAdRequest();
                appOpenAd.load(UnityPlayer.currentActivity, AD_UNIT_ID, request,
                        AppOpenAd.APP_OPEN_AD_ORIENTATION_PORTRAIT, loadCallback);
                    }
        });
    }

    /** Creates and returns ad request. */
    private static AdRequest getAdRequest() {
        return new AdRequest.Builder().build();
    }

    /** Utility method that checks if ad exists and can be shown. */
    public static boolean isAdAvailable() {
        return appOpenAd != null;
    }
}
