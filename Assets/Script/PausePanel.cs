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
        ShowMusic();
        GameControll.instance.Pause(true);
        reture.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.Close();
            GameControll.instance.Pause(false);
        });
        mus.onClick.AddListener(() =>
        {
            GlobelControl.instance.Setmusic();
            ShowMusic();
        });
        shop.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.OpenPanel<Shop>();
        });
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

    private void ShowMusic()
    {
        Font font;
        string sy;
        sy = Language.instance.GetLan("1005", out font);
        if (GlobelControl.instance.music)
        {
            sy = string.Format(sy, Language.instance.GetLan("1008", out font));
        }
        else
        {
            sy = string.Format(sy, Language.instance.GetLan("1009", out font));
        }
        mus.GetComponentInChildren<Text>().text = sy;
        mus.GetComponentInChildren<Text>().font = font;
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
