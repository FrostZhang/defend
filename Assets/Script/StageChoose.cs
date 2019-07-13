using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageChoose : MonoBehaviour
{

    public Transform pa;

    public Button ok;
    public Button back;

    private Transform child;
    void Start()
    {
        child = pa.GetChild(0);
        child.gameObject.SetActive(false);

        Ini();
        ok.onClick.AddListener(() =>
        {
            if (GlobelControl.instance.chooseStage != -1)
            {
                SceneManager.LoadScene("Stage");
            }
        });
        back.onClick.AddListener(() =>
        {
            GlobelControl.instance.panelControl.Close();
        });
    }

    private void Ini()
    {
        GlobelControl.instance.chooseStage = -1;
        var sd = GlobelControl.instance.stagedata.fs;
        for (int i = 0; i < 50; i++)
        {
            var m = i + 1;
            var stage = Instantiate(child, pa);
            stage.gameObject.SetActive(true);
            stage.name = m.ToString();
            var t = stage.GetComponent<Toggle>();
            if (i < sd.Count)
            {
                for (int n = 0; n < sd[i]; n++)
                {
                    stage.Find("s" + n.ToString()).gameObject.SetActive(true);
                }
                t.interactable = true;
            }
            else if(i == sd.Count)
            {
                t.interactable = true;
            }
            else
            {
                t.interactable = false;
            }
            stage.Find("id").GetComponent<Text>().text = m.ToString();
            t.onValueChanged.AddListener(_ =>
            {
                if (_)
                {
                    GlobelControl.instance.chooseStage = m - 1;
                }
            });
        }
    }
}
