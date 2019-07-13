using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobelControl : MonoBehaviour
{
    public static GlobelControl instance;

    public int chooseStage;

    public GameData.StageData stagedata;
    public const string Stagedata = "Stagedata";
    public GamePanel panelControl;
    public Transform[] guns;
    public GameData.CusData cusdata;

    public FingerPoint activegun;

    public const string JDdata = "JDdata";
    public const string Cusdata = "Cusdata";

    public bool music;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
        panelControl = new GamePanel(transform);
        guns = new Transform[7];
        for (int i = 0; i < 7; i++)
        {
            guns[i] = Resources.Load<Transform>("bul/" + i);
            Pool.Instance.Setpool("bul" + i, guns[i]);
        }
        ReadCusd();
        ReadStageData();

        music = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
    }

    private void ReadStageData()
    {
        string d = PlayerPrefs.GetString(Stagedata);
        GameData.StageData ds = JsonUtility.FromJson<GameData.StageData>(d);
        if (ds != null)
        {
            stagedata = ds;
        }
        else
        {
            stagedata = new GameData.StageData() { fs = new List<int>() { } };
        }
    }

    public void SetStar(int startnum)
    {
        if (chooseStage+1 <= stagedata.fs.Count)
        {
            stagedata.fs[chooseStage] = startnum;
        }
        else if(chooseStage == stagedata.fs.Count)
        {
            stagedata.fs.Add(startnum);
        }
        else
        {
            Debug.Log("添加通关星星失败：" + stagedata.fs.Count + " " + chooseStage + " " + startnum);
        }
    }

    private void ReadCusd()
    {
        string cusd = PlayerPrefs.GetString(Cusdata);
        var cd = JsonUtility.FromJson<GameData.CusData>(cusd);
        if (cd != null)
        {
            cusdata = cd;
        }
        else
        {
            cusdata = new GameData.CusData() { money = 0, stagelevel = 0, gun = 0, guns = new List<int>() {0} };
        }
        ChooseGun(cusdata.gun);
    }

    public void CanLvup()
    {
        if (chooseStage ==cusdata.stagelevel)
        {
            cusdata.stagelevel++;
            SaveCusd();
        }
    }

    public void SaveCusd()
    {
        string d = JsonUtility.ToJson(cusdata);
        PlayerPrefs.SetString(Cusdata, d);
    }

    public void BuyGun(int gun)
    {
        cusdata.guns.Add(gun);
        cusdata.gun = gun;
        ChooseGun(gun);
        SaveCusd();
    }

    public void ChooseGun(int gunid)
    {
        activegun = guns[gunid].GetComponent<FingerPoint>();
    }

    public void SaveStageData()
    {
        if (stagedata != null)
        {
            string d = JsonUtility.ToJson(stagedata);
            PlayerPrefs.SetString(Stagedata, d);
        }
    }

    public void Setmusic()
    {
        music = !music;
        if (music)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }
    }

}
