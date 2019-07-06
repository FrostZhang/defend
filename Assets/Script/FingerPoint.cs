using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FingerPoint : MonoBehaviour
{

    public int att;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enimy"))
        {
            var e = other.gameObject.GetComponent<Enimy>();
            e.Hurt(att);
        }
        else
        {
            Debug.Log(other.gameObject.layer);
        }
    }

    public void Ini(float are, int att)
    {
        transform.localScale = new Vector3(are, are, are);
        this.att = att;
    }
}
