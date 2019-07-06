using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enimy : MonoBehaviour
{
    public GameData.EnimyData data;
    public GameObject dieEff;
    private Animator anim;
    private bool canmove = true;
    NavMeshAgent ai;
    private Transform target;
    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }
    public void ini(Transform trg)
    {
        target = trg;
        StartCoroutine(_ini());
    }
    IEnumerator _ini()
    {
        yield return null;
        ai = GetComponent<NavMeshAgent>();
        ai.speed = data.speed;
        ai.destination = target.position;
    }

    public void Pause(bool b)
    {
        ai.isStopped = b;
        anim.SetBool("idel", b);
    }

    private bool die = false;
    public void Hurt(int demage)
    {
        if (die)
        {
            return;
        }
        data.hp -= demage;
        if (data.hp <= 0)
        {
            ai.isStopped = true;
            Kill();
            int c = 15 + GameControll.instance.activeSw.target.jddata.level * 5;
            GameUI.instance.ShowGetCoin(c, transform.position);
            GameControll.instance.GetCoin(c);
            Instantiate(GameControll.instance.pointEff, transform.position, Quaternion.identity);
        }
    }

    public void Kill()
    {
        if (die)
        {
            return;
        }
        GameControll.instance.activeSw.OnenimyDie(this);
        Destroy(gameObject, 0.25f);
        die = true;
    }

    void Update()
    {
        if (!canmove)
        {
            return;
        }
    }
}
