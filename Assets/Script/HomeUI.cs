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
            SceneManager.LoadScene("Stage");
        });
        shop.onClick.AddListener(() =>
        {
           
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
            bool m = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
            if (m)
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
            bool m = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
            m = !m;
            if (m)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
            if (m)
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
