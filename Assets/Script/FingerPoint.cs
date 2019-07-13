using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FingerPoint : MonoBehaviour
{
    public float att;
    public float chuan;
    public float leng;
    public float fanwei
    {
        get
        {
            if (bc)
            {
                return bc.size.y;
            }
            else
            {
                return GetComponent<BoxCollider>().size.x;
            }
        }
    }

    public Vector3 dir;
    public float life;

    BoxCollider bc;
    private Transform tr;
    private void Awake()
    {
        tr = transform;
        bc = tr.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enimy"))
        {
            var e = other.gameObject.GetComponent<Enimy>();
            e.Hurt((int)(att * 5));
            if (chuan > 0)
            {
                if (Random.Range(0, 1) < chuan)
                {
                    var es = GameControll.instance.activeSw.ess;
                    if (es.Count > 0)
                    {
                        Transform target = es[0].transform;
                        float dis = Vector3.SqrMagnitude(target.position - tr.position);
                        for (int i = 1; i < es.Count; i++)
                        {
                            float d = Vector3.SqrMagnitude(es[i].transform.position - tr.position);
                            if (d < dis)
                            {
                                target = es[i].transform;
                                dis = d;
                            }
                        }
                        dir = Vector3.Normalize(target.position - tr.position);
                        chuan -= 0.1f;
                    }
                    else
                    {
                        Pool.Instance.Recover("bul" + tr.name, tr);
                    }
                }
                else
                {
                    Pool.Instance.Recover("bul" + tr.name, tr);
                }
            }
            else
            {
                Pool.Instance.Recover("bul" + tr.name, tr);
            }
        }
        else
        {
            Debug.Log(other.gameObject.layer);
        }
    }

    public void Ini(Vector3 dir)
    {
        this.dir = dir;
        life = 2f;
        canfly = true;

        //test
        var es = GameControll.instance.activeSw.ess;
        if (es.Count > 0)
        {
            Transform target = es[0].transform;
            float dis = Vector3.SqrMagnitude(target.position - tr.position);
            for (int i = 1; i < es.Count; i++)
            {
                float d = Vector3.SqrMagnitude(es[i].transform.position - tr.position);
                if (d < dis)
                {
                    target = es[i].transform;
                    dis = d;
                }
            }
            dir = Vector3.Normalize(target.position - tr.position);
            dir.y = target.position.y;
            this.dir = dir;
        }
    }

    bool canfly;
    private void Update()
    {
        if (canfly)
        {
            life -= Time.deltaTime;
            if (life < 0)
            {
                Pool.Instance.Recover("bul" + tr.name, tr);
                canfly = false;
            }
            else
            {
                tr.position += dir * Time.deltaTime * 15f;
            }
        }
 
    }


}
