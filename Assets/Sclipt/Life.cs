using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Life : MonoBehaviour
{
    private TextMeshProUGUI lifeText = null;
    private int oldLifeNum = 0;

    void Start()
    {
        lifeText = GetComponent<TextMeshProUGUI>();
        if (GameManager.instance != null)
        {
            lifeText.text = "× " + GameManager.instance.lifeNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー設置し忘れ");
            Destroy(this);
        }
    }

    void Update()
    {
        if (oldLifeNum != GameManager.instance.lifeNum)
        {
            lifeText.text = "× " + GameManager.instance.lifeNum;
            oldLifeNum = GameManager.instance.lifeNum;
        }
    }
}
