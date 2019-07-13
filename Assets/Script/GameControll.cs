using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameControll : MonoBehaviour
{
    public static GameControll instance;

    public bool canclick = true;
    public float clickArea = 0.2f;


    public Sw[] sws;
    public Sw activeSw;

    public Transform[] enimies;
    public Transform[] shuienimies;

    private void Awake()
    {
        instance = this;
        GlobelControl.instance.panelControl.pa = FindObjectOfType<Canvas>().transform;
    }

    private void OnDestroy()
    {
        if (GlobelControl.instance)
        {
            GlobelControl.instance.panelControl.Clear();
        }
        instance = null;
    }

    void Start()
    {
        ReadJDdatas();
        ChooseStage(GlobelControl.instance.chooseStage);
    }

    private void ReadJDdatas()
    {
        string d = PlayerPrefs.GetString(GlobelControl.JDdata);
        if (!string.IsNullOrEmpty(d))
        {
            GameData.JDdatas ds = JsonUtility.FromJson<GameData.JDdatas>(d);
            if (ds != null && ds.fs != null && ds.fs.Count == sws.Length)
            {
                for (int i = 0; i < sws.Length; i++)
                {
                    sws[i].target.jddata = ds.fs[i];
                }
                return;
            }
        }
        for (int i = 0; i < sws.Length; i++)
        {
            sws[i].target.jddata = new GameData.JdData();
            sws[i].target.jddata.level = 1;
        }
    }

    public void SaveJDdatas()
    {
        GameData.JDdatas d = new GameData.JDdatas();
        d.fs = new List<GameData.JdData>();

        for (int i = 0; i < sws.Length; i++)
        {
            d.fs.Add(sws[i].target.jddata);
        }

        PlayerPrefs.SetString(GlobelControl.JDdata, JsonUtility.ToJson(d));
    }

    public void GetCoin(int coinnum)
    {
        GlobelControl.instance.cusdata.money += coinnum;
        GameUI.instance.Upcoin(GlobelControl.instance.cusdata.money, true);
        GameUI.instance.Showlv();
        GlobelControl.instance.SaveCusd();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Home))
        {
            GameUI.instance.pause.onClick.Invoke();
        }
    }

    public void PlayMusic(AudioClip clip,Vector3 pos)
    {
        if (GlobelControl.instance.music)
        {
            AudioSource.PlayClipAtPoint(clip, pos,1);
        }
        else
        {
            Debug.Log("-----------------静音");
        }
    }

    public void ChooseStage(int lv)
    {
        Debug.Log("选择关卡" + lv);
        int l = lv % sws.Length;
        for (int i = 0; i < sws.Length; i++)
        {
            sws[i].gameObject.SetActive(false);
        }
        sws[l].gameObject.SetActive(true);
        activeSw = sws[l];
        activeSw.Ini(lv);
    }

    public void Pause(bool b)
    {
        activeSw.pause = b;
        foreach (var item in activeSw.swpos)
        {
            var e = item.GetComponentsInChildren<Enimy>();
            foreach (var c in e)
            {
                c.Pause(b);
            }
        }
    }
}