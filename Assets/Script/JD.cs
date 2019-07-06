using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JD : MonoBehaviour
{
    public GameData.JdData jddata;
    [SerializeField]
    private int hp;
    private int hpmax;
    Tween anim;
    public Transform lvUppos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enimy"))
        {
            var e = other.gameObject.GetComponent<Enimy>();
            hp -= e.data.att;
            if (hp > 0)
            {
                GameUI.instance.jdhp.value = hp;
                if (anim == null || !anim.IsPlaying())
                {
                    anim = transform.GetChild(0).DOScale(1.15f, 0.35f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        transform.GetChild(0).localScale = Vector3.one;
                    });
                }
                Instantiate(e.dieEff, e.transform.position, Quaternion.identity);
            }
            else
            {
                //守护失败
                hp = 0;
                GameUI.instance.jdhp.value = hp;
                GameUI.instance.panelControl.OpenPanel<GameOver>();
            }
            e.Kill();
        }
    }

    public void Ini()
    {
        ShowHpText();
        StartCoroutine(inicapos());
    }

    private void ShowHpText()
    {
        hpmax = hp = jddata.level * 2 + 8;
        GameUI.instance.jdhp.maxValue = hpmax;
        GameUI.instance.jdhp.value = hp;
    }

    IEnumerator inicapos()
    {
        yield return null;
        GameUI.instance.lvupbtn.position = Camera.main.WorldToScreenPoint(lvUppos.position);
    }

    public bool LevelUp()
    {
        var lv = jddata.level;
        int pay = lv * 1018 - 868;
        if (GameControll.instance.data.money >= pay)
        {
            GameControll.instance.data.money -= pay;
            jddata.level++;
            var i = jddata.level * 2 + 8;
            var up = i - hpmax;
            hpmax = i;
            hp += up;
            GameUI.instance.jdhp.maxValue = hpmax;
            GameUI.instance.jdhp.value = hp;
            GameControll.instance.SaveJDdatas();
            return true;
        }
        else
        {
            return false;
        }
    }
}
