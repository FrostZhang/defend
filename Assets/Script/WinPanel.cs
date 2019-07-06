using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel : MonoBehaviour {
    public Button j1, j2;
    public Button next;
    //public PlayUnityAd ad;
    public AudioClip winm;

    void Start () {
        GameControll.instance.PlayMusic(winm, Camera.main.transform.position);
        next.gameObject.SetActive(false);
        j1.gameObject.SetActive(true);
        j2.gameObject.SetActive(true);
        j1.onClick.AddListener(() =>
        {
            Shownext();
            GameControll.instance.GetCoin(GameControll.instance.data.stagelevel * 100);
        });
        j2.onClick.AddListener(() =>
        {
            AdScript.instance.ShowRewardAd((x,y,b) =>
            {
                if (b)
                {
                    Shownext();
                    GameControll.instance.GetCoin(GameControll.instance.data.stagelevel * 200);
                }
            });
        });
        next.onClick.AddListener(() =>
        {
            GameControll.instance.data.stagelevel++;
            GameControll.instance.SaveCusd();
            GameUI.instance.panelControl.Close();
            GameControll.instance.ChooseStage(GameControll.instance.data.stagelevel);
            GameUI.instance.lvupbtn.gameObject.SetActive(false);
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
        next.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f).SetLoops(-1,LoopType.Yoyo);
    }
}
