using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : ItemBase2D
{
    [SerializeField, Header("加算するライフ")] int _myLife = 1;

    public override void Activate()
    {
        FindObjectOfType<GameManager>().AddLifeNum();
    }
}
