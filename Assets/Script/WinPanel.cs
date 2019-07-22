using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel : MonoBehaviour
{
    public Button j1, j2;
    public Button next;
    public AudioClip winm;
    public Transform[] stars;

    public Transform eff;
    public Transform efftarget;
    public float buc=1;
    private int star;
    void Start()
    {
        efftarget = GameUI.instance.coinT.transform;
        Jiesuan();
        GlobelControl.instance.CanLvup();
        GameControll.instance.PlayMusic(winm, Camera.main.transform.position);
        Linqu();
        Next();
    }

    private void Jiesuan()
    {
        var jiesuan = GameControll.instance.activeSw.target.Hurt;
        if (jiesuan > 0.9f)
        {
            star = 3;
        }
        else if (jiesuan > 0.5f)
        {
            star = 2;
        }
        else
        {
            star = 1;
        }
        GlobelControl.instance.SetStar(star);
        for (int i = 0; i < star; i++)
        {
            Transform t = stars[i];
            t.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            t.gameObject.SetActive(true);
            t.DOMove(t.position + new Vector3(-25f, 25f), 0.35f).From().SetDelay(i * 0.2f).OnComplete(() =>
            {
                t.DOScale(Vector3.one, 0.2f).SetLoops(5, LoopType.Yoyo);
            });
        }
    }

    private void Next()
    {
        next.gameObject.SetActive(false);
        next.onClick.AddListener(() =>
        {
            GlobelControl.instance.chooseStage++;
            GlobelControl.instance.SaveCusd();
            GlobelControl.instance.panelControl.Close();
            GameControll.instance.ChooseStage(GlobelControl.instance.chooseStage);
            GameUI.instance.lvupbtn.gameObject.SetActive(false);
        });
    }

    private void Linqu()
    {
        j1.gameObject.SetActive(true);
        j2.gameObject.SetActive(true);
        j1.onClick.AddListener(() =>
        {
            j1.onClick.RemoveAllListeners();
            j2.onClick.RemoveAllListeners();
            StartCoroutine(coinEff(20, j1.transform.position));
            GameControll.instance.GetCoin(star * 100 + (GlobelControl.instance.chooseStage+1) * (star*25));
        });
        j2.onClick.AddListener(() =>
        {
            AdScript.instance.ShowRewardAd((x, y, b) =>
            {
                if (b)
                {
                    j2.onClick.RemoveAllListeners();
                    j1.onClick.RemoveAllListeners();
                    StartCoroutine(coinEff(35, j2.transform.position));
                    GameControll.instance.GetCoin((star * 100 + (GlobelControl.instance.chooseStage + 1) * (star * 25)) *2);
                }
            });
        });
    }

    private void Shownext()
    {
        next.gameObject.SetActive(true);
        tweenNext();
        j1.gameObject.SetActive(false);
        j2.gameObject.SetActive(false);
    }

    private void tweenNext()
    {
        next.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    IEnumerator coinEff(int effnum,Vector2 tartgetpos)
    {
        var re = GetComponent<RectTransform>();
        float maxx = re.rect.size.x;
        maxx = Camera.main.ScreenToWorldPoint(new Vector3(maxx, 0, 0)).x;
        for (int i = 0; i < effnum; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 inipos = new Vector2(Random.Range(0, maxx), 0);
            //inipos = Camera.main.ScreenToWorldPoint(inipos);
            var ef = Instantiate(eff, transform);
            ef.GetComponent<RectTransform>().position = inipos;
            float centery = tartgetpos.y + Random.Range(50, 100);
            var li =  PhysicsUtil.GetParabolaInitVelocity(inipos, tartgetpos, -9.8f, centery, 0)* buc;
            StartCoroutine(moveeff(ef,tartgetpos, li));
        }
        yield return new WaitForSeconds(effnum * 0.1f);
        Shownext();
    }

    IEnumerator moveeff(Transform eff,Vector3 t1,Vector3 ver)
    {
        yield return null;
        float t=0;
        Vector3 inipos = eff.position;
        while (Vector3.Distance(eff.position,t1)>25)
        {
            t += Time.deltaTime*8;
            var next = PhysicsUtil.GetParabolaNextPosition(inipos, ver, -9.8f, t);
            eff.right = next - eff.position;
            eff.position = next;
            yield return new WaitForEndOfFrame();
            if (t>20f)
            {
                break;
            }
        }
        t = Vector3.Distance(eff.position, efftarget.position);
        while ( t> 10)
        {
            var dir = efftarget.position -  eff.position;
            eff.position += dir * Time.deltaTime*3;
            if (t<100)
            {
                var t2 = t * 0.01f;
                eff.localScale = new Vector3(t2, t2, t2);
            }
            yield return new WaitForEndOfFrame();
            t = Vector3.Distance(eff.position, efftarget.position);
        }
        Destroy(eff.gameObject);
    }

}
