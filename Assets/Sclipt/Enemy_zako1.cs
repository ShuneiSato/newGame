using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_zako1 : MonoBehaviour
{
    #region//インスペクター
    [Header("スコア加算")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動するか")] public bool nonVisibleAct;
    [Header("掴みコライダー")] public Grab grabSencer;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("ダメージ時SE")] public AudioClip damegeSE;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    private bool gSencer = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
        gSencer = GetComponent<Grab>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!oc.playerStepOn)
        {
            if (sr.isVisible | nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            else
            {
                rb.Sleep();
            }
        }
        else if (oc.playerStepOn)
        {
            Debug.Log("踏んだ");
                if (!isDead)
                {
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.PlaySE(damegeSE);
                        GameManager.instance.score += myScore;
                    }
                    anim.Play("Enemy_zako_dead");
                    rb.velocity = new Vector2(0, -gravity);
                    isDead = true;
                    Destroy(gameObject, 0.3f);
                }
                else
                {
                    transform.Rotate(new Vector3(0, 0, -30));
                }
        }
        if (grabSencer.isGrab)
        {

        }
    }

    public void Grab()
    {
        Debug.Log("判定入った");
        if (!isDead)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.PlaySE(damegeSE);
                GameManager.instance.score += myScore;
            }
            anim.Play("Enemy_zako_dead");
            rb.velocity = new Vector2(0, -gravity);
            isDead = true;
            gameObject.tag = "Dead";
            Destroy(gameObject, 0.3f);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -30));
        }
    }
}
