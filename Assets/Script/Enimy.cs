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
        ai = GetComponent<NavMeshAgent>();
    }
    public void ini(Transform trg)
    {
        target = trg;
        StartCoroutine(_ini());
    }
    IEnumerator _ini()
    {
        yield return null;
        if (ai)
        {
            ai.speed = data.speed;
            ai.destination = target.position;
        }
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
            Kill();
            //敌人死亡  获得收益
            int c = Random.Range(10, 35) + GameControll.instance.activeSw.target.jddata.level * 2;
            GameUI.instance.ShowGetCoin(c, transform.position);
            GameControll.instance.GetCoin(c);
        }
    }

    public void Kill()
    {
        if (die)
        {
            return;
        }
        die = true;
        if (ai)
            ai.isStopped = true;
        var efc = Instantiate(dieEff, transform.position, Quaternion.identity);
        var music = efc.GetComponent<AudioSource>();
        if (music)
        {
            music.mute = !GlobelControl.instance.music;
        }
        GameControll.instance.activeSw.OnenimyDie(this);
        Destroy(gameObject, 0.25f);
    }

    void Update()
    {
        if (!canmove)
        {
            return;
        }
    }
}
