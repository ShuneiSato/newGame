using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    [SerializeField] float _scoreChangeInterval = 1.0f;
    private TextMeshProUGUI scoreText = default;
    int _maxScore = 9999;
    private int _score = 0;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// スコア加算の処理
    /// 各所で呼び出しするとき加算量を調整する
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        int tempScore = _score;
        _score = Mathf.Min(_score + score, _maxScore);
        Debug.Log("呼び出された");

        DOTween.To(() => tempScore,
            x =>
            {
                tempScore = x;
                scoreText.text = tempScore.ToString("0000");
            },
            _score,
            _scoreChangeInterval).
            OnComplete(() => scoreText.text = _score.ToString("0000"));
    }
}
