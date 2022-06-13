using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [Header("スコア")] public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("リスポーンポイント")] public int continueNum;
    [Header("現在の残機")] public int lifeNum;
    [Header("デフォルトの残機")] public int defaultHeartNum;
    [HideInInspector] public bool isGameOver;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機を1増やす
    /// </summary>
    public void AddLifeNum()
    {
        if (lifeNum < 99)
        {
            ++lifeNum;
        }
    }

    /// <summary>
    /// 残機を1減らす
    /// </summary>
    public void SubLifeNum()
    {
        if (lifeNum > 0)
        {
            --lifeNum;
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    /// 最初から始める時の処理
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        lifeNum = defaultHeartNum;
        score = 0;
        stageNum = 1;
        continueNum = 0;
    }
    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySE(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていない！");
        }
    }
}
