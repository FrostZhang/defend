using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameControll : MonoBehaviour
{
    public static GameControll instance;

    public bool canclick = true;
    public float clickArea = 0.2f;

    public GameData.CusData data;

    public Transform[] guns;

    public GameObject pointEff;
    public AudioClip pointMusic;

    public Sw[] sws;
    public Sw activeSw;

    public const string JDdata = "JDdata";
    public const string Cusdata = "Cusdata";

    public Transform[] enimies;
    public Transform[] shuienimies;
    public bool music;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        music = PlayerPrefs.GetInt("music", 1) == 1 ? true : false;
        ReadJDdatas();
        ReadCusd();
        //test
        //ChooseStage(testLv);
    }


    private void ReadJDdatas()
    {
        string d = PlayerPrefs.GetString(JDdata);
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

    private void ReadCusd()
    {
        string cusd = PlayerPrefs.GetString(Cusdata);
        var cd = JsonUtility.FromJson<GameData.CusData>(cusd);
        if (cd != null)
        {
            data = cd;
        }
        else
        {
            data = new GameData.CusData() { money = 0, stagelevel = 1 };
        }
        ChooseStage(data.stagelevel);
    }

    public void SaveCusd()
    {
        string d = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Cusdata, d);
    }

    public void SaveJDdatas()
    {
        GameData.JDdatas d = new GameData.JDdatas();
        d.fs = new List<GameData.JdData>();

        for (int i = 0; i < sws.Length; i++)
        {
            d.fs.Add(sws[i].target.jddata);
        }

        PlayerPrefs.SetString(JDdata, JsonUtility.ToJson(d));
    }

    public void GetCoin(int coinnum)
    {
        data.money += coinnum;
        GameUI.instance.Upcoin(data.money, true);
        GameUI.instance.Showlv();
        SaveCusd();
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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Home))
        {
            GameUI.instance.pause.onClick.Invoke();
        }
        //if (!canclick)
        //{
        //    return;
        //}
        //else
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (EventSystem.current.IsPointerOverGameObject())
        //        {
        //            return;
        //        }

        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hitInfo;
        //        Debug.DrawRay(Camera.main.transform.position, ray.direction * 10, Color.red, 2f);
        //        if (Physics.Raycast(ray, out hitInfo, 50))
        //        {
        //            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("House"))
        //            {
        //                Debug.Log("点击到了屋子");
        //                return;
        //            }
        //            PlayMusic(pointMusic, hitInfo.point);
        //            var t = Instantiate(fingerPoint, hitInfo.point, Quaternion.identity, transform);
        //            t.Ini(clickArea, 1);
        //            Destroy(t.gameObject, 0.1f);
        //        }
        //    }
        //}
    }

    public void PlayMusic(AudioClip clip,Vector3 pos)
    {
        if (music)
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
        int l = (lv - 1) % sws.Length;
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