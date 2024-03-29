﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownFloor : MonoBehaviour
{
    [Header("スプライトがあるオブジェクト")] public GameObject spriteOb;
    [Header("振動幅")] public float vibrationWidth = 0.05f;
    [Header("振動速度")] public float vibrationSpeed = 30.0f;
    [Header("落ちるまでの時間")] public float fallTime = 1.0f;
    [Header("落ちていく速度")] public float fallSpeed = 10.0f;
    [Header("落ちてから戻るまでの時間")] public float returnTime = 5.0f;
    [Header("振動アニメーション")] public AnimationCurve curve;

    private bool isOn;
    private bool isFall;
    private bool isReturn;
    private Vector3 spriteDefaultPos;
    private Vector3 floorDefaultPos;
    private Vector2 fallVelocity;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private ObjectCollision oc;
    private SpriteRenderer sr;
    private float timer = 0.0f;
    private float fallingTimer = 0.0f;
    private float returnTimer = 0.0f;
    private float blinkTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        oc = GetComponent<ObjectCollision>();
        if (spriteOb != null && oc != null && col != null && rb != null)
        {
            spriteDefaultPos = spriteOb.transform.position;
            fallVelocity = new Vector2(0, -fallSpeed);
            floorDefaultPos = gameObject.transform.position;
            sr = spriteOb.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.Log("fallDownFloor インスペクターに設定し忘れがあります");
                Destroy(this);
            }
        }
        else
        {
            Debug.Log("fallDownFloor インスペクターに設定し忘れがあります");
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //一度乗るとフラグをオンにする
        if (oc.playerStepOn)
        {
            isOn = true;
            oc.playerStepOn = false;
        }

        //振動する
        if (isOn && isFall)
        {
            float x = curve.Evaluate(timer * vibrationSpeed) * vibrationWidth;
            spriteOb.transform.position = spriteDefaultPos + new Vector3(x, 0, 0);

            //一定時間たったら落ちる
            if (timer > fallTime)
            {
                isFall = true;
            }
            timer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //落下中
        if (isFall)
        {
            rb.velocity = fallVelocity;

            //一定時間経つと元の位置に戻る
            if (fallingTimer > fallTime)
            {
                isReturn = true;
                transform.position = floorDefaultPos;
                rb.velocity = Vector2.zero;
                isFall = false;
                timer = 0.0f;
                fallingTimer = 0.0f;
            }
            else
            {
                fallingTimer += Time.deltaTime;
                isOn = false;
            }
        }
    }
}
