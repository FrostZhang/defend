using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Together;
using System.Text.RegularExpressions;
using System;

public class AdScript : MonoBehaviour
{
    public InputField sceneId;
    public Text logField;

    private string[] scenes;
    private int sceneIndex = 0;
    private string[] sceneNames;
    public static AdScript instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Already have AdScript.");
            Destroy(gameObject);
            return;
        }
        TGSDK.SetDebugModel(true);
        TGSDK.SDKInitFinishedCallback = (string msg) =>
        {
            TGSDK.TagPayingUser(TGPayingUser.TGMediumPaymentUser, "CNY", 0, 0);
            Log("TGSDK finished : " + msg);
            Debug.Log("TGSDK GetUserGDPRConsentStatus = " + TGSDK.GetUserGDPRConsentStatus());
            TGSDK.SetUserGDPRConsentStatus("yes");
            Debug.Log("TGSDK GetIsAgeRestrictedUser = " + TGSDK.GetIsAgeRestrictedUser());
            TGSDK.SetIsAgeRestrictedUser("no");
            float bannerHeight = (float)(Screen.height) * 0.123f;
            TGSDK.SetBannerConfig("OSL4KTuf4BoLhG0y0yF", "TGBannerNormal", 0, Display.main.systemHeight - bannerHeight, Display.main.systemWidth, bannerHeight, 30);
            //TGSDK.SetBannerConfig("banner1", "TGBannerNormal", 0, Display.main.systemHeight - 2 * bannerHeight, Display.main.systemWidth, bannerHeight, 30);
            //TGSDK.SetBannerConfig("banner2", "TGBannerNormal", 0, Display.main.systemHeight - 3 * bannerHeight, Display.main.systemWidth, bannerHeight, 30);
        };
#if UNITY_IOS && !UNITY_EDITOR
		TGSDK.Initialize ("hP7287256x5z1572E5n7");
#elif UNITY_ANDROID && !UNITY_EDITOR
		TGSDK.Initialize ("2I064Z57Ha35k123CpX3");//59t5rJH783hEQ3Jd7Zqr
#endif

        Invoke("PreloadAd", 0.5f);
    }

    public void Log(string message)
    {
        Debug.Log("[TGSDK-Unity]  " + message);
        if (logField)
        {
            if (logField.text.Length > 100)
            {
                logField.text = message;
            }
            else
            {
                logField.text = logField.text + "\n" + message;
            }
        }
    }
    bool preloadAd;
    bool chapin;
    bool canjiangli;
    public void PreloadAd()
    {
        TGSDK.PreloadAdSuccessCallback = (string msg) =>
        {
            Log("PreloadAdSuccessCallback : " + msg);
            scenes = Regex.Split(msg, ",", RegexOptions.IgnoreCase);
            sceneNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                string scene = scenes[i];
                string sceneName = TGSDK.GetSceneNameById(scene);
                sceneNames[i] = sceneName + "(" + scene.Substring(0, 4) + ")";
            }
            RefreshSceneId();
            preloadAd = true;
        };
        TGSDK.PreloadAdFailedCallback = (string msg) =>
        {
            //广告配置失败
            Log("PreloadAdFailedCallback : " + msg);
            preloadAd = false;
        };
        TGSDK.InterstitialLoadedCallback = (string msg) =>
        {
            //插屏准备完毕
            Log("InterstitialLoadedCallback : " + msg);
            chapin = true;
        };
        TGSDK.InterstitialVideoLoadedCallback = (string msg) =>
        {
            //插屏视频准备完毕
            Log("InterstitialVideoLoadedCallback : " + msg);
        };
        TGSDK.AwardVideoLoadedCallback = (string msg) =>
        {
            //奖励准备完毕
            Log("AwardVideoLoadedCallback : " + msg);
            canjiangli = true;
        };
        TGSDK.AdShowSuccessCallback = (string scene, string msg) =>
        {
            //插屏广告播放
            Log("AdShow : " + scene + " SuccessCallback : " + msg);
        };
        TGSDK.AdShowFailedCallback = (string scene, string msg, string err) =>
        {
            //插屏广告失败
            Log("AdShow : " + scene + " FailedCallback : " + msg + ", " + err);
        };
        TGSDK.AdCloseCallback = (string scene, string msg, bool award) =>
        {
            //奖励回调 award为true 则播放完了
            Log("AdClose : " + scene + " Callback : " + msg + " Award : " + award);
        };
        TGSDK.AdClickCallback = (string scene, string msg) =>
        {
            //广告被用户点击
            Log("AdClick : " + scene + " Callback : " + msg);
        };
        TGSDK.PreloadAd();
    }

    private void RefreshSceneId()
    {
        if (scenes != null && scenes.Length > 0)
        {
            if (sceneId)
            {
                sceneId.text = sceneNames[sceneIndex];
            }
        }
    }

    public void LastScene()
    {
        if (sceneIndex > 0)
        {
            sceneIndex--;
            RefreshSceneId();
        }
    }

    public void NextScene()
    {
        if (sceneIndex < scenes.Length - 1)
        {
            sceneIndex++;
            RefreshSceneId();
        }
    }

    public void ShowAd()
    {
        string sceneid = scenes[sceneIndex];
        if (TGSDK.CouldShowAd(sceneid))
        {
            TGSDK.ShowAd(sceneid);
        }
        else
        {
            Log("Scene " + sceneid + " could not to show");
        }
    }

    private bool ShowAd(AdType adtp)
    {
        int i = (int)adtp;
        if (scenes!=null && i < scenes.Length)
        {
            string sceneid = scenes[i];
            if (TGSDK.CouldShowAd(sceneid))
            {
                TGSDK.ShowAd(sceneid);
                return true;
            }
            else
            {
                Log("Scene " + sceneid + " could not to show");
                return false;
            }
        }
        return false;
    }

    public void ShowRewardAd(System.Action<string, string, bool> reward)
    {
        if (ShowAd(AdType.video))
        {
            TGSDK.AdCloseCallback = reward;
        }
    }

    public void ShowPauseAd(bool show)
    {
        if (scenes != null && scenes.Length>0)
        {
            string sceneid = scenes[1];
            if (show)
            {
                if (TGSDK.CouldShowAd(sceneid))
                {
                    TGSDK.ShowAd(sceneid);
                }
                else
                {
                    Log("Scene " + sceneid + " could not to show");
                }
            }
            else
            {
                TGSDK.CloseBanner(sceneid);
            }
        }
    }

    public void ShowTestView()
    {
        string sceneid = scenes[sceneIndex];
        TGSDK.ShowTestView(sceneid);
    }

    public void CloseBanner()
    {
        string sceneid = scenes[sceneIndex];
        TGSDK.CloseBanner(sceneid);
    }
}

public enum AdType
{
    video,
    chapin
}