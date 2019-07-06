using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel
{

    public Transform pa;

    public Stack<Component> cps;

    public GamePanel(Transform root)
    {
        cps = new Stack<Component>();
        pa = root;
    }

    /// <summary>
    /// 列队开启 将临时关闭本级 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T OpenPanel<T>() where T : Component
    {
        T t;
        if (cps.Count > 0)
        {
            cps.Peek().gameObject.SetActive(false);
        }
        t = pa.GetComponent<T>();
        if (t)
        {
            t.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log(typeof(T).Name);
            t = Resources.Load<T>(typeof(T).Name);
            t = GameObject.Instantiate<T>(t, pa);
        }
        cps.Push(t);
        return t;
    }
    /// <summary>
    /// 列队关闭 将关闭本级 并打开上一级
    /// </summary>
    public void Close()
    {
        GameObject.Destroy(cps.Pop().gameObject);
        if (cps.Count>0)
        {
            var g = cps.Peek();
            if (g)
            {
                g.gameObject.SetActive(true);
            }
        }
    }
}
