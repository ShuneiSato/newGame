using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("フェード")] public Fadeimage fade;
    [Header("ゲームスタート時のSE")] public AudioClip startSE;

    private bool firstpush = false;
    private bool goNextScene = false;

    public void PressStart()
    {
        Debug.Log("Press Start!");
        if (!firstpush)
        {
            fade.StartFadeOut();

            firstpush = true;
        }
    }

    private void Update()
    {
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("Stage1");
            goNextScene = true;
        }
    }
}
