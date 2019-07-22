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
    [SerializeField]
    float _chuan;
    private void Awake()
    {
        tr = transform;
        bc = tr.GetComponent<BoxCollider>();
        tempEnimys = new List<Transform>();
    }

    int jdlv;
    private void OnEnable()
    {
        tr.GetComponent<AudioSource>().mute = !GlobelControl.instance.music;
        jdlv = GameControll.instance.activeSw.target.jddata.level;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enimy"))
        {
            var e = other.gameObject.GetComponent<Enimy>();
            e.Hurt((int)((att + jdlv * 0.01f) * 5));
            if (_chuan > 0)
            {
                if (Random.Range(0, 1f) < _chuan)
                {
                    FindEnimy();
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

    List<Transform> tempEnimys;

    private void FindEnimy()
    {
        var es = GameControll.instance.activeSw.ess;
        if (es.Count > 0)
        {
            int begin = 0;
            Transform target = es[begin].transform;
            while (tempEnimys.Contains(target))
            {
                begin++;
                if (begin >= es.Count)
                {
                    target = null;
                }
                else
                    target = es[begin].transform;
            }
            if (target == null)
            {
                tempEnimys.Clear();
                begin = 0;
                target = es[begin].transform;
            }
            float dis = Vector3.SqrMagnitude(target.position - tr.position);
            for (int i = begin; i < es.Count; i++)
            {
                if (!tempEnimys.Contains(es[i].transform))
                {
                    float d = Vector3.SqrMagnitude(es[i].transform.position - tr.position);
                    if (d < dis)
                    {
                        target = es[i].transform;
                        dis = d;
                    }
                }
            }
            tempEnimys.Add(target);
            var t1 = new Vector3(target.position.x, 0, target.position.z);
            var t2 = new Vector3(tr.position.x, 0, tr.position.z);
            dir = Vector3.Normalize(t1 - t2);
            _chuan -= 0.1f;
        }
        else
        {
            Pool.Instance.Recover("bul" + tr.name, tr);
        }
    }

    public void Ini(Vector3 dir)
    {
        this.dir = dir;
        life = 2f;
        canfly = true;
        _chuan = chuan + (jdlv / 5) * 0.1f;
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
                tr.position += dir * Time.deltaTime * 25f;
            }
        }

    }


}
