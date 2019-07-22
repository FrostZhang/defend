using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;


    public Slider jdhp;
    public Text hptext;

    public Text coinT;
    public Text dyText;
    public Transform lvupbtn;
    public Text houselv;
    public CanvasGroup cg;
    public Text stage;
    public AudioClip readymusic;
    public Button pause;

    public Button fire;
    public Joy joy;
    public Transform[] ready;

    private void Awake()
    {
        instance = this;
        dyText.gameObject.SetActive(false);
        jdhp.onValueChanged.AddListener(OnHpChange);
        coinT.text = string.Empty;
        jdhp.maxValue = 0;
        jdhp.value = 0;
        houselv.text = "LV:";
    }

    void Start()
    {
        lvupbtn.GetComponent<Button>().onClick.AddListener(OnLvUp);
        pause.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.OpenPanel<PausePanel>();
        });
        Fire();
    }

    bool canfire;
    private void Fire()
    {
        fire.onClick.AddListener(() =>
        {
            if (canfire)
            {
                var t = Pool.Instance.Get("bul" + GlobelControl.instance.cusdata.gun,
        joy.line.position, Quaternion.identity, GameControll.instance.activeSw.transform);
                var bul = t.GetComponent<FingerPoint>();
                bul.Ini(Vector3.Normalize(joy.line.right));
                _fire = 1 - GlobelControl.instance.activegun.leng;
                canfire = false;
            }
        });
    }

    float _fire;
    private void Update()
    {
        if (canfire)
        {
            return;
        }
        _fire -= Time.deltaTime;
        if (_fire < 0)
        {
            canfire = true;
        }
    }

    private void OnHpChange(float arg0)
    {
        hptext.text = jdhp.value.ToString() + "/" + jdhp.maxValue.ToString();
    }

    private void OnDestroy()
    {
        instance = null;
        lvupbtn.GetComponent<Button>().onClick.RemoveAllListeners();
        jdhp.onValueChanged.RemoveAllListeners();
    }

    public void ShowSwJd(Sw sw)
    {
        Upcoin(GlobelControl.instance.cusdata.money, false);
        houselv.text = "LV: " + sw.target.jddata.level.ToString();
    }

    private void OnLvUp()
    {
        var jd = GameControll.instance.activeSw.target;
        bool b = jd.LevelUp();  //检测是否升级成功
        if (b)
        {
            Upcoin(GlobelControl.instance.cusdata.money, true);
            houselv.text = "LV: " + jd.jddata.level.ToString();
        }
        Showlv();
    }

    Tween lvtween;
    public void Showlv()
    {
        var jd = GameControll.instance.activeSw.target;
        var lv = jd.jddata.level;
        int pay = lv * 1018 - 868;
        if (GlobelControl.instance.cusdata.money >= pay)
        {
            lvupbtn.gameObject.SetActive(true);
            lvupbtn.Find("coin").GetComponent<Text>().text = pay.ToString();
            if (lvtween == null || !lvtween.IsPlaying())
            {
                lvtween = lvupbtn.DOScale(1.1f, 0.5f).SetLoops(-1);
            }
        }
        else
        {
            if (lvtween != null && lvtween.IsPlaying())
            {
                lvtween.Kill();
                lvupbtn.localScale = Vector3.one;
            }
            lvupbtn.gameObject.SetActive(false);
        }
    }

    public void Ready(Action act)
    {
        GameControll.instance.PlayMusic(readymusic, Camera.main.transform.position);
        //AudioSource.PlayClipAtPoint(readymusic, Camera.main.transform.position);
        ready[0].localScale = Vector3.zero;
        ready[0].DOScale(Vector2.one, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
         {
             ready[0].localScale = Vector3.zero;
             ready[1].localScale = Vector3.zero;
             ready[1].DOScale(Vector2.one, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
             {
                 if (act != null)
                 {
                     act.Invoke();
                 }
             });
         });
    }


    Tween coint;
    private int coin;
    public void Upcoin(int coin, bool tween)
    {
        if (tween)
        {
            if (coint != null && coint.IsPlaying())
            {
                coint.Kill();
            }
            coint = DOTween.To(() => this.coin, (_) =>
            {
                this.coin = _;
                coinT.text = _.ToString();
            }, coin, 1.5f);
        }
        else
        {
            this.coin = coin;
            coinT.text = coin.ToString();
        }
    }

    public void ShowGetCoin(int coin, Vector3 pos)
    {
        pos = Camera.main.WorldToScreenPoint(pos);
        var t = Instantiate(dyText, pos, Quaternion.identity, transform);
        t.gameObject.SetActive(true);
        t.text = "+" + coin.ToString();
        t.transform.DOMoveY(75, 1.5f).SetRelative();
        t.DOFade(0, 1).OnComplete(() => { Destroy(t.gameObject); }).SetDelay(0.5f);
    }

    public void ShowStage(int lv)
    {
        stage.text = lv.ToString();
        StartCoroutine(showstage());
    }

    IEnumerator showstage()
    {
        cg.alpha = 1;
        yield return new WaitForSeconds(5);
        cg.DOFade(0, 3);
    }
}
