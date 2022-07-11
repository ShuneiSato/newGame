using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerOb;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverOb;
    [Header("フェード")] public Fadeimage fade;
    [Header("ゲームオーバー時のSE")] public AudioClip gameOverSE;
    [Header("リトライ時のSE")] public AudioClip retrySE;
    [Header("ステージクリアSE")] public AudioClip stageClearSE;
    [Header("ステージクリア")] public GameObject stageClearOb;
    [Header("ステージクリア判定")] public PlayerTriggerCheck stageClearTrigger;



    private PlayerControler p;
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playerOb != null && continuePoint != null && continuePoint.Length > 0 && gameOverOb != null && fade != null)
        {
            gameOverOb.SetActive(false);
            stageClearOb.SetActive(false);
            playerOb.transform.position = continuePoint[0].transform.position;

            p = playerOb.GetComponent<PlayerControler>();
            if (p == null)
            {
                Debug.Log("プレイヤー以外がアタッチされている！");
            }
        }
        else
        {
            Debug.Log("復活地点未設定！");
        }
    }

    //// Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver && !doGameOver)
        {
            gameOverOb.SetActive(true);
            GameManager.instance.PlaySE(gameOverSE);
            doGameOver = true;
        }
        else if (p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if (continuePoint.Length > GameManager.instance.continueNum)
            {
                playerOb.transform.position = continuePoint[GameManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定数が足りてない！");
            }
        }
        else if (stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }
        //ステージを切り替える
        if (fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeInComplete())
            {
                if (retryGame)
                {
                    GameManager.instance.RetryGame();
                }
                else
                {
                    GameManager.instance.stageNum = nextStageNum;
                }
                GameManager.instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }
    }



    /// <summary>
    /// 最初のステージに戻る
    /// </summary>
    public void Retry()
    {
        GameManager.instance.PlaySE(retrySE);
        ChangeScene(1); //最初のステージに戻るので1
        retryGame = true;
    }

    /// <summary>
    /// ステージを切り替える
    /// </summary>
    /// <param name="num"></param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }

    /// <summary>
    /// ステージクリア時
    /// </summary>
    public void StageClear()
    {
        GameManager.instance.isStageClear = true;
        stageClearOb.SetActive(true);
        GameManager.instance.PlaySE(stageClearSE);
    }
}
