using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;

public class Language
{
    public Dictionary<string, Dictionary<string, string>> language;
    public static Language instance;

    public bool isok;
    public void Ini()
    {
        instance = this;
        string path = "file:///" + Application.streamingAssetsPath + "/Language.csv";
#if UNITY_STANDALONE
        path =Application.streamingAssetsPath + "/Language.csv";
#elif UNITY_EDITOR
        path = Application.streamingAssetsPath + "/Language.csv";
#elif UNITY_IPHONE
    path = Application.streamingAssetsPath + "/Language.csv";
#elif UNITY_ANDROID
    path = Application.streamingAssetsPath + "/Language.csv";
#endif
        Debug.Log("读取语言文件:"+path);

       GlobelControl.instance.StartCoroutine( LoadCsvFile(path));
    }

    public string GetLan(string id, string contry)
    {
        if (language.ContainsKey(id))
        {
            if (language[id].ContainsKey(contry))
            {
                return language[id][contry];
            }
        }
        Debug.Log("语言出错" + id + " " + contry);
        return string.Empty;
    }

    public void GetLan(Text tt, string id)
    {
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
                tt.text = GetLan(id, "CN");
                break;
            case SystemLanguage.ChineseTraditional:
                tt.font = GlobelControl.instance.gfs[0].font;
                tt.text = GetLan(id, "TW");
                break;
            case SystemLanguage.German:
                tt.font = GlobelControl.instance.gfs[1].font;
                tt.text = GetLan(id, "DE");
                break;
            default:
                tt.font = GlobelControl.instance.gfs[1].font;
                tt.text = GetLan(id, "EN");
                break;
        }
    }

    public string GetLan( string id,out Font font)
    {
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
                font = GlobelControl.instance.gfs[0].font;
                return GetLan(id, "CN");
            case SystemLanguage.ChineseTraditional:
                font = GlobelControl.instance.gfs[0].font;
                return GetLan(id, "TW");
            case SystemLanguage.German:
                font = GlobelControl.instance.gfs[1].font;
                return GetLan(id, "DE");
            default:
                font = GlobelControl.instance.gfs[1].font;
                return GetLan(id, "EN");
        }
    }

    IEnumerator LoadCsvFile(string filePath)
    {
        Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
        string[] fileData;
#if UNITY_EDITOR || UNITY_STANDALONE
        //fileData =System.IO.File.ReadAllLines(filePath);
        WWW www = new WWW(filePath);
        while (!www.isDone)
            yield return null;
        fileData = www.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
#elif   UNITY_ANDROID && !UNITY_EDITOR
        WWW www = new WWW(filePath);
        while (!www.isDone)
            yield return null;
        fileData = www.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
#endif
        /* CSV文件的第一行为Key字段，第二行开始是数据。第一个字段一定是ID。 */
        string[] keys = fileData[0].Split(',');
        for (int i = 1; i < fileData.Length; i++)
        {
            string[] line = fileData[i].Split(',');
            /* 以ID为key值，创建一个新的集合，用于保存当前行的数据 */
            string ID = line[0];
            result[ID] = new Dictionary<string, string>();
            for (int j = 0; j < line.Length; j++)
            {
                /* 每一行的数据存储规则：Key字段-Value值 */
                result[ID][keys[j]] = line[j];
            }
        }
        language = result;
        yield return null;
        isok = true;
    }
}
