using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("打ち出す弾")]public GameObject bulletBall;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 20.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {

            GameObject clone = Instantiate(bulletBall, transform.position, Quaternion.identity);

            // クリックした座標の取得（スクリーン座標からワールド座標に変換）
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 向きの生成（Z成分の除去と正規化）
            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

            // 弾に速度を与える
            clone.GetComponent<Rigidbody2D>().velocity = shotForward * speed;

            GameObject obj = GameObject.Find("EnemyBall(Clone)");

            Destroy(obj);
     
        }
    }
}
