using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : ItemBase2D
{
    [SerializeField, Header("加算スコア")] int _score = 20;
    /// <summary>
    /// スコア取得時の加算
    /// </summary>
    public override void Activate()
    {
        FindObjectOfType<Score>().AddScore(_score);
    }
}
