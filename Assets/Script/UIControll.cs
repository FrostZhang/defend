using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControll : LayoutGroup
{
    Camera ca;
    float ratio;

    protected override void Awake()
    {
        base.Awake();
        ca = Camera.main;
        ratio = 16f /10;
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {
        //var bili = (float)Screen.width / Screen.height;
        //if (bili < ratio)
        //{
        //    ca.fieldOfView = 60 * (bili / ratio);
        //}
        //else if (bili > ratio)
        //{
        //    ca.fieldOfView = 60 * (ratio / bili);
        //}
        //else
        //{
        //    ca.fieldOfView = 60;
        //}
        //Debug.Log(bili + " " + ratio);
    }

    public override void SetLayoutVertical()
    {

    }


}
