using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Ini());
    }

    IEnumerator Ini()
    {
        yield return new WaitUntil(()=>GlobelControl.instance.lan.isok);
        Debug.Log(GlobelControl.instance.panelControl + " " + GetComponent<Canvas>().transform);
        GlobelControl.instance.panelControl.pa = GetComponent<Canvas>().transform;
        GlobelControl.instance.panelControl.OpenPanel<HomeUI>();
    }

    private void OnDestroy()
    {
        if (GlobelControl.instance)
        {
            GlobelControl.instance.panelControl.Clear();
        }
    }
}
