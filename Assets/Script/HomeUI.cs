using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeUI : MonoBehaviour {
    public Button start, shop, chose, set,music,ret,cleart;
    public GameObject main, setobj;
    
	void Start () {

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
            if (GlobelControl.instance.music)
            {
                music.GetComponentInChildren<Text>().text = "声音:开";
            }
            else
            {
                music.GetComponentInChildren<Text>().text = "声音:关";
            }
        });
        music.onClick.AddListener(() =>
        {
            GlobelControl.instance.Setmusic();
            if (GlobelControl.instance.music)
            {
                music.GetComponentInChildren<Text>().text = "声音:开";
            }
            else
            {
                music.GetComponentInChildren<Text>().text = "声音:关";
            }
        });
        ret.onClick.AddListener(() =>
        {
            main.SetActive(true);
            setobj.SetActive(false);
        });
    }

}
