using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageResult : MonoBehaviour
{
    [Header("拡大縮小のアニメーション")] public AnimationCurve curve;
    [Header("リザルト移行")] public StageCtrl ctrl;

    private bool comp;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comp)
        {
            if (timer < 1.0f)
            {
                transform.localScale = Vector3.one * curve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one;
                SceneManager.LoadScene("Stage3");
                comp = true;
            }
        }
    }
}
