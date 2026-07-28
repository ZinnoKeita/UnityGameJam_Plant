// 火の玉
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBallSCR : MonoBehaviour
{
    [Header("--- クローンの設定 ---")]
    public float speed = 5.0f;

    [Header("--- 本体の設定 ---")]
    public GameObject FireBall; // インスペクターでプレハブまたは自分自身を設定

    // 「決まった位置」のリスト
    //インスペクタービューで調整
    //public Vector3[] startPositions = new Vector3[]
    //{
    //    new Vector3(0.0f, 0.0f, 1.0f),
    //    new Vector3(2.0f, 0.0f, 2.0f),
    //    new Vector3(2.0f, 0.0f, 0.0f)
    //};

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int Damage = 50;




    public Transform playerTransform;
    private bool isClone = false;

    // 連射速度を制御するタイマー
    private float spawnTimer = 0.0f;
    public float spawnInterval = 2.0f;

    private float G_time = 0.0f;
    private int SpeedUp = 0;

    private Vector3 moveDirection;
    void Start()
    {
        // 画面からプレイヤー（Cube）を探す
        GameObject player = GameObject.FindWithTag("Player");
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

            if (playerTransform != null)
            {
                moveDirection = playerTransform.position - transform.position;
                // 高さを固定（箱が地面にめり込んだり浮いたりするのを防ぐ）
                moveDirection.y = 0;
                // 方向の長さを1に整える（正規化）
                moveDirection.Normalize();
            }
        }
    }

    void Update()
    {
        if (GameManagerScript.Instance.isGameOver)
        {
            Destroy(gameObject);
            return;
        }
        if (GameManagerScript.Instance.isGameStart == false)
        {
            return;
        }

        // 【本体の処理】複数の決まった位置から１つ選んで出す
        if (!isClone)
        {
            G_time += Time.deltaTime;
            if (G_time >= 15.0f && SpeedUp == 0)
            {
                spawnInterval = 1.0f;
                SpeedUp = 1;
            }
            else if (G_time >= 60.0f && SpeedUp == 1)
            {
                spawnInterval = 0.5f;
                SpeedUp = 2;
            }
            else if (G_time >= 90.0f && SpeedUp == 2)
            {
                spawnInterval = 0.25f;
                SpeedUp = 3;
            }
            else if (G_time >= 120.0f && SpeedUp == 3)
            {
                spawnInterval = 0.125f;
                SpeedUp = 4;
            }else if(G_time >= 300.0f && SpeedUp == 4)
            {
                spawnInterval = 0.1f;
                SpeedUp = 5;
            }

            spawnTimer += Time.deltaTime; // タイマーを進める
            if (spawnTimer >= spawnInterval) // 〇秒経ったら
            {
                //int randomIndex = Random.Range(0, startPositions.Length);
                //// 設定した位置(インスペクタービュー)からそれぞれクローンを生成


                //// 本体の位置をrandomIndexで指定
                //Vector3 spawnPos = transform.position + startPositions[randomIndex];

                //GameObject clone = Instantiate(FireBall, spawnPos, Quaternion.identity);

                int index = Random.Range(0, spawnPoints.Length);

                GameObject clone = Instantiate(
                    FireBall,
                    spawnPoints[index].position,
                    Quaternion.identity);

                FireBallSCR cloneScript = clone.GetComponent<FireBallSCR>();
                if (cloneScript != null)
                {
                    cloneScript.isClone = true;
                    cloneScript.playerTransform = this.playerTransform;
                }


                spawnTimer = 0.0f; // タイマーリセット
            }
            return; // 本体の処理はここまで
        }

        // 【クローンの処理】プレイヤーに向かって進み続ける
        if (moveDirection != Vector3.zero)
        {
            // 計算済みの方向へスピードの分だけ進む
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }

        // 画面端にいったら消去
        if (transform.position.x > 1000.0f || transform.position.x < -1000.0f || transform.position.z > 1000.0f || transform.position.z < -1000.0f)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        CheckHit(other.gameObject);
    }

    // Is Trigger チェックなし（物理衝突）のとき
    public void OnCollisionEnter(Collision collision)
    {
        CheckHit(collision.gameObject);
    }

    // 当たり判定の共通処理
    private void CheckHit(GameObject hitObject)
    {
        

        if (!isClone) return;

            // 名前に Cube が含まれている（Cube, Cube(Clone) など）または Player タグの場合
            if (hitObject.name.Contains("Cube") || hitObject.CompareTag("Player"))
            {
                Player player = hitObject.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage(Damage);
                }
                Destroy(gameObject);
            }
    }
}