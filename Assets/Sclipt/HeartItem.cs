using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : ScoreItem
{
    [Header("加算するライフ")] public int myLife;
    void Update()
    {
        if (playerCheck.isOn) //プレイヤーが判定に入ったら
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.PlaySE(getSE);
                GameManager.instance.lifeNum += myLife;
                Destroy(this.gameObject);
            }
        }
    }
}
