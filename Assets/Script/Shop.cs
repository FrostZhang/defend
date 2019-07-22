using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button re;
    public Transform pa;
    private Transform preb;
    public Text coin;
    // Use this for initialization
    void Start()
    {
        re.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.Close();
        });
        preb = pa.GetChild(0);
        preb.gameObject.SetActive(false);
        Ini();
        coin.text = GlobelControl.instance.cusdata.money.ToString();
    }

    private void Ini()
    {
        var guns = GlobelControl.instance.guns;
        for (int i = 0; i < 7; i++)
        {
            var fp = guns[i].GetComponent<FingerPoint>();
            var t = Instantiate(preb, pa);
            t.gameObject.SetActive(true);
            t.name = i.ToString();
            t.Find("wepa/we").GetComponent<Image>().sprite = Resources.Load<Sprite>("weapon/" + (i + 1).ToString());
            var a = t.Find("att/Slider").GetComponent<Slider>();
            a.value = fp.att;
            var b = t.Find("len/Slider").GetComponent<Slider>();
            b.value = fp.leng;
            var c = t.Find("chuan/Slider").GetComponent<Slider>();
            c.value = fp.chuan;
            var d = t.Find("range/Slider").GetComponent<Slider>();
            d.value = fp.fanwei;
            var to = t.GetComponent<Toggle>();
            var n = i;
            if (GlobelControl.instance.cusdata.guns.Contains(n))
            {
                CanClick(t, to, n);
                if (GlobelControl.instance.cusdata.gun==n)
                {
                    to.isOn = true;
                }
            }
            else
            {
                int pay = n < 3 ? 5000 : 8000;
                t.Find("pay").GetComponent<Text>().text = pay.ToString();
                to.interactable = false;
                t.Find("buy").GetComponent<Button>().onClick.AddListener(() =>
                {
                    pay = n < 3 ? 5000 : 8000;
                    if (GlobelControl.instance.cusdata.money >= pay)
                    {
                        to.interactable= true;
                        GlobelControl.instance.cusdata.money -= pay;
                        GlobelControl.instance.BuyGun(n);
                        CanClick(t, to, n);
                        to.isOn = true;
                    }
                });
            }

        }
    }

    private static void CanClick(Transform t, Toggle to, int n)
    {
        to.onValueChanged.AddListener((_) =>
        {
            if (_)
            {
                GlobelControl.instance.cusdata.gun = n;
                GlobelControl.instance.ChooseGun(n);
            }
        });
        Language.instance.GetLan(t.Find("pay").GetComponent<Text>(), "1026");
        t.Find("buy").gameObject.SetActive(false);
    }
}
