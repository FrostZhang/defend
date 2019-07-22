using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour {
    public int languageid;
    public string text;

    private Text tt;
	void Start ()
    {
        Uptext();
        GlobelControl.instance.Regist(this, true);
    }

    public void Uptext()
    {
        if (!tt)
        {
            tt = GetComponent<Text>();
        }
        SystemLanguage lan;
        if (GlobelControl.instance.cuslanguage != SystemLanguage.Unknown)
        {
            lan = GlobelControl.instance.cuslanguage;
        }
        else
        {
            lan = Application.systemLanguage;
        }
        switch (lan)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                tt.font = GlobelControl.instance.gfs[0].font;
                tt.text = Language.instance.GetLan(languageid.ToString(), "CN");
                break;
            case SystemLanguage.ChineseTraditional:
                tt.font = GlobelControl.instance.gfs[0].font;
                tt.text = Language.instance.GetLan(languageid.ToString(), "TW");
                break;
            case SystemLanguage.German:
                tt.font = GlobelControl.instance.gfs[1].font;
                tt.text = Language.instance.GetLan(languageid.ToString(), "DE");
                break;
            default:
                tt.font = GlobelControl.instance.gfs[1].font;
                tt.text = Language.instance.GetLan(languageid.ToString(), "EN");
                break;
        }
    }

    private void OnDestroy()
    {
        GlobelControl.instance.Regist(this, false);

    }
}
