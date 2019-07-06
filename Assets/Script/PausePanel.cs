using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Button reture, mus, shop, retry, quit;

    private void Start()
    {
        AdScript.instance.ShowPauseAd(true);
        if (GameControll.instance.music)
        {
            mus.GetComponentInChildren<Text>().text = "声音:开";
        }
        else
        {
            mus.GetComponentInChildren<Text>().text = "声音:关";
        }
        GameControll.instance.Pause(true);
        reture.onClick.AddListener(() =>
        {
            GameUI.instance.panelControl.Close();
            GameControll.instance.Pause(false);
        });
        mus.onClick.AddListener(() =>
        {
            GameControll.instance.Setmusic();
            if (GameControll.instance.music)
            {
                mus.GetComponentInChildren<Text>().text = "声音:开";
            }
            else
            {
                mus.GetComponentInChildren<Text>().text = "声音:关";
            }
        });
        shop.onClick.AddListener(() => { });
        retry.onClick.AddListener(() =>
        {
            //GameUI.instance.panelControl.Close();
            //GameControll.instance.Pause(false);
        });
        quit.onClick.AddListener(() =>
        {
            GameControll.instance.Pause(false);
            SceneManager.LoadScene("Home");
        });

    }

    private void OnDestroy()
    {
        AdScript.instance.ShowPauseAd(false);
        reture.onClick.RemoveAllListeners();
        shop.onClick.RemoveAllListeners();
        retry.onClick.RemoveAllListeners();
        quit.onClick.RemoveAllListeners();
    }
}
