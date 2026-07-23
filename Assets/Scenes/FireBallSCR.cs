// 火の玉
using UnityEngine;

public class FireBallSCR : MonoBehaviour
{
    [Header("--- クローンの設定 ---")]
    public float speed = 5.0f;

    [Header("--- 本体の設定 ---")]
    public GameObject FireBall; // インスペクターでプレハブまたは自分自身を設定

    // 「決まった位置」のリスト
    public Vector3[] startPositions = new Vector3[]
    {
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(2.0f, 0.0f, 2.0f),
        new Vector3(2.0f, 0.0f, 0.0f)
    };

    public Transform playerTransform;
    private bool isClone = false;

    // 連射速度を制御するタイマー
    private float spawnTimer = 0.0f;
    public float spawnInterval = 0.1f;

    void Start()
    {
        // 画面からプレイヤー（Cube）を探す
        GameObject player = GameObject.Find("Cube");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // もし自分が「本体」なら
        if (!isClone)
        {
            // Scratchの「隠す」の再現（本体は画面に見えないようにする）
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = false;
        }
        // もし自分が「クローン」なら
        else
        {
            // クローンは画面に表示する（Scratchの「表示する」）
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = true;

            // Scratchの「Sprite1 へ向ける」の3D再現
            if (playerTransform != null)
            {
                transform.LookAt(playerTransform);
            }
        }
    }

    void Update()
    {
        // 【本体の処理】複数の決まった位置から常に同時に出す
        if (!isClone)
        {
            spawnTimer += Time.deltaTime; // タイマーを進める
            if (spawnTimer >= spawnInterval) // 0.1秒経ったら
            {
                // 設定した位置（3箇所）からそれぞれクローンを生成
                for (int i = 0; i < startPositions.Length; i++)
                {
                    // 本体の位置（transform.position）にずらし分（startPositions[i]）を足す
                    Vector3 spawnPos = transform.position + startPositions[i];

                    GameObject clone = Instantiate(FireBall, spawnPos, Quaternion.identity);

                    FireBallSCR cloneScript = clone.GetComponent<FireBallSCR>();
                    if (cloneScript != null)
                    {
                        cloneScript.isClone = true;
                        cloneScript.playerTransform = this.playerTransform;
                    }
                }

                spawnTimer = 0.0f; // タイマーリセット
            }
            return; // 本体の処理はここまで
        }

        // 【クローンの処理】プレイヤーに向かって進み続ける
        if (playerTransform != null)
        {
            // 自分からプレイヤー（Cube）への「正しい方向（ベクトル）」を計算
            Vector3 direction = playerTransform.position - transform.position;
            //  高さを固定（箱が地面にめり込んだり浮いたりするのを防ぐ）
            direction.y = 0;
            //  方向の長さを1に整える（正規化）
            direction.Normalize();

            // 計算した方向へスピードの分だけ進む
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        // 画面端にいったら消去
        if (transform.position.x > 40.0f || transform.position.x < -40.0f || transform.position.z > 40.0f || transform.position.z < -40.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!isClone) return;

        // ぶつかった相手の名前が「Cube」だったらクローンを削除
        if (collision.gameObject.name == "Cube")
        {
            Destroy(gameObject);
        }
    }
}