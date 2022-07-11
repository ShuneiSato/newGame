using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAsobi : MonoBehaviour
{
    [Header("ボタンSE")] public AudioClip ButtonSE;
    public GameObject canvas;


    private void Start()
    {
        canvas.SetActive(false);
    }
    public void OnClick()
    {
        canvas.SetActive(true);
        GameManager.instance.PlaySE(ButtonSE);
    }
    public void OnClick2()
    {
        canvas.SetActive(false);
        GameManager.instance.PlaySE(ButtonSE);
    }
}
