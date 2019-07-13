using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private static Pool instance;

    public static Pool Instance
    {
        get
        {
            if (instance == null)
            {
                items = new SortedDictionary<string, Stack<Transform>>();
                instance = new Pool();
            }
            return instance;
        }

    }

    public static SortedDictionary<string, Stack<Transform>> items;

    public Transform Get(string poolname, Vector3 pos, Quaternion ro, Transform pa, string path = null)
    {
        if (string.IsNullOrEmpty(poolname))
        {
            return null;
        }
        if (items.ContainsKey(poolname))
        {
            if (items[poolname].Count > 1)
            {
                var t = items[poolname].Pop();
                t.SetPositionAndRotation(pos, ro);
                t.gameObject.SetActive(true);
                t.SetParent(pa);
                return t;
            }
            else
            {
                var t = Object.Instantiate(items[poolname].Peek(), pos, ro, pa);
                t.name = items[poolname].Peek().name;
                return t;
            }
        }
        else if (!string.IsNullOrEmpty(path))
        {
            var t = Resources.Load<Transform>(path);
            if (t)
            {
                Setpool(poolname, t);
                var t2 = Object.Instantiate(t, pos, ro, pa);
                t2.name = t.name;
                return t2;
            }
        }
        return null;
    }

    public void Setpool(string poolname, Transform item)
    {
        if (items.ContainsKey(poolname))
        {
            Debug.Log("Already have poolitem " + poolname);
            return;
        }
        var a = new Stack<Transform>();
        a.Push(item);
        items.Add(poolname, a);
    }

    public void Setpool(string poolname, string path)
    {
        if (items.ContainsKey(poolname))
        {
            Debug.Log("Already have poolitem " + poolname);
            return;
        }
        if (!string.IsNullOrEmpty(path))
        {
            var t = Resources.Load<Transform>(path);
            if (t)
            {
                Setpool(poolname, t);
            }
        }
    }

    public void Recover(string poolname, Transform item)
    {
        if (items.ContainsKey(poolname))
        {
            item.gameObject.SetActive(false);
            items[poolname].Push(item);
        }
    }
}
