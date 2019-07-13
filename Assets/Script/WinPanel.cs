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
    void Start()
    {
        Jiesuan();
        GlobelControl.instance.CanLvup();
        GameControll.instance.PlayMusic(winm, Camera.main.transform.position);
        Linqu();
        Next();
    }

    private void Jiesuan()
    {
        var jiesuan = GameControll.instance.activeSw.target.Hurt;
        int n;
        if (jiesuan > 0.9f)
        {
            n = 3;
        }
        else if (jiesuan > 0.5f)
        {
            n = 2;
        }
        else
        {
            n = 1;
        }
        GlobelControl.instance.SetStar(n);
        for (int i = 0; i < n; i++)
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
            Shownext();
            GameControll.instance.GetCoin((GlobelControl.instance.chooseStage+1) * 100);
        });
        j2.onClick.AddListener(() =>
        {
            AdScript.instance.ShowRewardAd((x, y, b) =>
            {
                if (b)
                {
                    Shownext();
                    GameControll.instance.GetCoin((GlobelControl.instance.chooseStage + 1) * 200);
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

    IEnumerator showcoin(int num)
    {
        for (int i = 0; i < num; i++)
        {
            yield return null;
        }
    }

    private void tweenNext()
    {
        next.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
