using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;
    private int oldscore = 0;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        if (GameManager.instance != null)
        {
            scoreText.text = "Score " + GameManager.instance.score;
        }
        else
        {
            Debug.Log("ゲームマネージャー設置し忘れ");
            Destroy(this);
        }
    }

    void Update()
    {
        if (oldscore != GameManager.instance.score)
        {
            scoreText.text = "Score " + GameManager.instance.score;
            oldscore = GameManager.instance.score;
        }
    }
}
