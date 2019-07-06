using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public Button retry, quit;
    public AudioClip lose;
    // Use this for initialization
    void Start()
    {
        GameControll.instance.PlayMusic(lose, Camera.main.transform.position);
        GameControll.instance.Pause(true);
        retry.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Stage");
        });
        quit.onClick.AddListener(() => { SceneManager.LoadScene("Home"); });
    }
}
