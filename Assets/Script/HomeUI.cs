using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeUI : MonoBehaviour
{
    public Button start, shop, chose, set, music, ret, cleart;
    public GameObject main, setobj;
    public Button lebtn, ribtn;

    public SystemLanguage[] cuslan;
    void Start()
    {
        ShowMusic();
        start.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.OpenPanel<StageChoose>();
        });
        shop.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.OpenPanel<Shop>();
        });
        cleart.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
        });
        chose.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        set.onClick.AddListener(() =>
        {
            main.SetActive(false);
            setobj.SetActive(true);
            ShowMusic();
        });
        music.onClick.AddListener(() =>
        {
            GlobelControl.instance.Setmusic();
            ShowMusic();
        });
        ret.onClick.AddListener(() =>
        {
            main.SetActive(true);
            setobj.SetActive(false);
        });
        SetLan();
    }

    private void SetLan()
    {
        lebtn.onClick.AddListener(() =>
        {
            for (int i = 0; i < cuslan.Length; i++)
            {
                if (cuslan[i] == GlobelControl.instance.cuslanguage ||
                GlobelControl.instance.cuslanguage == SystemLanguage.Unknown)
                {
                    if (i == 0)
                    {
                        GlobelControl.instance.cuslanguage = cuslan[cuslan.Length - 1];
                    }
                    else
                    {
                        GlobelControl.instance.cuslanguage = cuslan[i - 1];
                    }
                    GlobelControl.instance.UpUiText();
                    ShowMusic();
                    return;
                }
            }
        });
        ribtn.onClick.AddListener(() =>
        {
            for (int i = 0; i < cuslan.Length; i++)
            {
                if (cuslan[i] == GlobelControl.instance.cuslanguage ||
                GlobelControl.instance.cuslanguage == SystemLanguage.Unknown)
                {
                    if (i == cuslan.Length - 1)
                    {
                        GlobelControl.instance.cuslanguage = cuslan[0];
                    }
                    else
                    {
                        GlobelControl.instance.cuslanguage = cuslan[i + 1];
                    }
                    GlobelControl.instance.UpUiText();
                    ShowMusic();
                    return;
                }
            }
        });
    }

    private void ShowMusic()
    {
        Font font;
        string sy;
        sy = Language.instance.GetLan("1005",out font);
        if (GlobelControl.instance.music)
        {
            sy = string.Format(sy, Language.instance.GetLan("1008", out font));
        }
        else
        {
            sy = string.Format(sy, Language.instance.GetLan("1009", out font));
        }
        music.GetComponentInChildren<Text>().text = sy;
        music.GetComponentInChildren<Text>().font = font;
    }
}
