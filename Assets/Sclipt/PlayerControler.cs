using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    #region//インスペクターで設定
    [Header("移動速度")] public float speed;//速度
    [Header("重力")] public float gravity;//重力
    [Header("ジャンプ速度")] public float jumpspeed;//ジャンプ速度
    [Header("ジャンプ高度")] public float jumpHeight;//ジャンプ高度
    [Header("ジャンプ制限")] public float jumpLimitTime;//ジャンプ制限時間
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("掴みコライダー")] public Grab grabSencer;
    [Header("設置判定")] public GroundSencer ground;//設置判定
    [Header("頭をぶつけた判定")] public GroundSencer head;//頭をぶつけた判定
    [Header("ダッシュスピード処理")] public AnimationCurve dushCurve;
    [Header("ジャンプスピード処理")] public AnimationCurve jumpCurve;
    [Header("ジャンプする時のSE")] public AudioClip jumpSE;
    [Header("やられた時のSE")] public AudioClip downSE;
    [Header("復帰時のSE")] public AudioClip continueSE;
    #endregion

    [SerializeField] private BoxCollider2D grabcollision;

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private Animator anim = null;
    private CapsuleCollider2D capcol = null;
    private SpriteRenderer sr = null;
    private MoveObject moveOb = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isDown = false;
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool gSencer = false;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private float jumpPos = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float jumpTime = 0.0f;
    private float dashTime = 0.0f;
    private float beforeKey = 0.0f;
    private string enemyTag = "Enemy";
    private string deadAreaTag = "DeadArea";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";

    #endregion

    [SerializeField] StageCtrl _stageCtrl;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        gSencer = GetComponent<Grab>(); 
    }

    private void Update()
    {
        if (isContinue)
        {
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            else
            {
                sr.enabled = true;
            }
        }
        if (continueTime > 2.0f)
        {
            isContinue = false;
            blinkTime = 0.0f;
            continueTime = 0.0f;
            sr.enabled = true;
        }
        else
        {
            blinkTime += Time.deltaTime;
            continueTime += Time.deltaTime;
        }



        if (Input.GetButton("Fire1"))
        {
            StartCoroutine(grab());
        }
    }
    private IEnumerator grab()
    {
        grabcollision.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        grabcollision.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDown && !GameManager.instance.isGameOver)
        {
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //各種座標軸の速度を求める
            float ySpeed = GetYspeed();
            float xSpeed = GetXSpeed();

            //アニメーションを適用
            SetAnimation();

            //移動速度を設定
            Vector2 addVelocity = Vector2.zero;
            if (moveOb != null)
            {
                addVelocity = moveOb.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;
        }
        else
        {
            rb.velocity = new Vector2(0, -gravity);
        }


    }
    /// <summary>
    /// Y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYspeed()
    {
        float JumpKey = Input.GetAxis("Jump");
        float ySpeed = -gravity;

        if (isOtherJump)
        {
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;

            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead)
            {
                ySpeed = jumpspeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime += 0.0f;
            }
        }
        else if (isGround)
        {
            if (JumpKey > 0)
            {
                if (!isJump)
                {
                    GameManager.instance.PlaySE(jumpSE);
                }
                ySpeed = jumpspeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }

        }
        else if (isJump)
        {
            bool pushUpKey = JumpKey > 0;
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            bool canTime = jumpLimitTime > jumpTime;
            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpspeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        //アニメーションカーブを速度に適用
        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }

        return ySpeed;
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        float HorizontalKey = Input.GetAxis("Horizontal");

        float xSpeed = 0.0f;

        if (HorizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = speed;
        }

        else if (HorizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = -speed;
        }

        else
        {
            isRun = false;
            dashTime = 0.0f;
            xSpeed = 0.0f;
        }

        //前回の入力からダッシュの反転を判断して速度を変える
        if (HorizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (HorizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = HorizontalKey;

        //アニメーションカーブを速度に適用
        xSpeed *= dushCurve.Evaluate(dashTime);

        return xSpeed;
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting()
    {
        if (GameManager.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }
    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if (isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("K_O_"))
            {
                if (currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ContinuePlayer()
    {
        GameManager.instance.PlaySE(continueSE);
        isDown = false;
        anim.Play("stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    private void ReceiveDamage(bool downAnim)
    {
        if (isDown)
        {
            return;
        }
        else
        {
            if (downAnim)
            {
                anim.Play("K_O_");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            GameManager.instance.PlaySE(downSE);
            GameManager.instance.SubLifeNum();

        }
    }
    #region
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);

        if (enemy || moveFloor || fallFloor)
        {
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.point.y < judgePos)
                {
                    if (enemy || fallFloor)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            if (enemy)
                            {
                                otherJumpHeight = o.boundHeight;
                                o.playerStepOn = true;
                                jumpPos = transform.position.y;
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor)
                            {
                                o.playerStepOn = true;
                            }
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが付いてない！");
                        }
                    }
                    else if (moveFloor)
                    {
                        moveOb = collision.gameObject.GetComponent<MoveObject>();
                    }
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == moveFloorTag)
        {
            moveOb = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
            Debug.Log("死亡判定内");
        }

        if (collision.tag == enemyTag)
        {
            grabSencer.isGrab = true;
            collision.GetComponent<Enemy_zako1>().Grab();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == enemyTag)
        {
            grabSencer.isGrab = false;
        }
    }
    #endregion
}
