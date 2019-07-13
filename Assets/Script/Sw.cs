using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sw : MonoBehaviour
{
    public Transform[] swpos;
    public JD target;

    public Vector3 capos;

    public Vector2 swtime;
    private float _swtime;

    private int enimynum;
    public bool pause = false;
    private int _enimynum;

    private int bossNum;
    private int _bossNum;

    private int killnum;

    public List<Enimy> ess;

    //1级 30个兵 50级 125兵
    public void Ini(int lv)
    {
        enimynum = (int)(lv * 1.938f + 28.062f);
        bossNum = lv / 5 + 1;
        StartCoroutine(inicapos());
    }

    IEnumerator inicapos()
    {
        var t = Camera.main.transform;
        var dir = Vector3.Normalize(t.position - capos);
        t.position += dir * 2.5f;
        while (Vector3.Distance(t.position, capos) > 0.1f)
        {
            t.position += (capos - t.position).normalized * Time.deltaTime * 3;
            yield return new WaitForEndOfFrame();
        }
        t.position = capos;
        yield return null;
        GameUI.instance.ShowSwJd(this);
        GameUI.instance.ShowStage(GlobelControl.instance.chooseStage+1);
        GameUI.instance.joy.line.position = target.transform.position + Vector3.up;
        GameUI.instance.Ready(() =>
        {
            target.Ini();
            cansw = swenimy = true;
            killnum = 0;
        });
    }

    bool cansw;
    bool swenimy;

    private Enimy SwEnimy(Transform pa)
    {
        Transform[] es;
        if (pa.name == "shui")
        {
            es = GameControll.instance.shuienimies;
        }
        else
        {
            es = GameControll.instance.enimies;
        }
        var p = Random.Range(0, es.Length);
        var a = Instantiate(es[p], pa.position, transform.rotation, pa);
        var e = a.GetComponent<Enimy>();
        return e;
    }

    private void Update()
    {
        if (cansw && !pause)
        {
            if (swenimy)
            {
                _swtime -= Time.deltaTime;
                if (_swtime < 0)
                {
                    _swtime = Random.Range(swtime.x, swtime.y);
                    int n = Random.Range(0, swpos.Length);
                    int max = Random.Range(1, 3);
                    for (int i = 1; i < max; i++)
                    {
                        Transform pa = swpos[n];
                        var e = SwEnimy(pa);
                        int lv = GlobelControl.instance.chooseStage;
                        e.data.hp = (int)(0.1836f * lv + 0.8164f);
                        e.data.speed = 2 + Random.Range(-0.5f, 0.5f);
                        e.data.att = 1;
                        e.ini(target.transform);
                        ess.Add(e);
                        n++;
                        n = n >= swpos.Length ? 0 : n;
                        _enimynum++;
                        if (_enimynum >= enimynum)
                        {
                            swenimy = false;
                        }
                    }
                }
            }
            else
            {
                _swtime -= Time.deltaTime;
                if (_swtime < 0)
                {
                    _swtime = Random.Range(swtime.x, swtime.y);
                    int n = Random.Range(0, swpos.Length);
                    int max = Random.Range(1, 3);
                    for (int i = 1; i < max; i++)
                    {
                        Transform pa = swpos[n];
                        var e = SwEnimy(pa);
                        e.transform.localScale = e.transform.localScale * 1.5f;
                        int lv = GlobelControl.instance.chooseStage;
                        e.data.hp = (int)(0.3469F * lv + 2.6531F);
                        e.data.speed = 1.75f + Random.Range(-0.5f, 0.5f);
                        e.data.att = 1+Random.Range(1,3);
                        e.ini(target.transform);
                        ess.Add(e);
                        n++;
                        n = n >= swpos.Length ? 0 : n;
                        _bossNum++;
                        if (_bossNum >= bossNum)
                        {
                            cansw = false;
                        }
                    }
                }
            }
        }
    }

    public void OnenimyDie(Enimy e)
    {
        ess.Remove(e);
        killnum++;
        if (killnum >= (enimynum + bossNum))
        {
            Debug.Log(killnum + " " + enimynum + " " + bossNum);
            GlobelControl.instance.panelControl.OpenPanel<WinPanel>();
            ess.Clear();
            //准备win
        }
    }

}
