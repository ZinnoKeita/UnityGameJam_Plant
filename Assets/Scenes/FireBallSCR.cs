//
//火の玉



using UnityEngine;


public class FireBallSCR : MonoBehaviour
{
    [Header("--- クローンの設定 ---")]
    public float speed = 5.0f;
    [Header("--- 本体の設定の設定 ---")]
    public GameObject FireBall;

    public Transform playerTransform;
    private bool isClone = false;

    void Start()
    {
        //画面から箱を探す
        GameObject player = GameObject.Find("Cube");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        transform.LookAt(playerTransform);

        if (!isClone)
        {
            // Scratchの「隠す」の再現（本体は画面に見えないようにする）
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = false;

        }
        else
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = true;

            // Scratchの「Sprite1 へ向ける」の3D再現
            if (playerTransform != null)
            {
                transform.LookAt(playerTransform);
            }
        }
    }


    public void Update()
    {
        if (!isClone)
        {
            Vector3 spawnPosition = transform.position; // 本体と同じ位置に生成

            GameObject newClone = Instantiate(gameObject, spawnPosition, Quaternion.identity);

            // 生成されたオブジェクトに「お前はクローンだよ」と教える
            FireBallSCR cloneScript = newClone.GetComponent<FireBallSCR>();
            if (cloneScript != null)
            {
                cloneScript.isClone = true;
            }

            return; // 本体の処理はここまで
        }
        if (playerTransform != null)
        {
            // 1. 自分からプレイヤー（Sprite1）への「正しい方向（ベクトル）」を直接計算する
            Vector3 direction = playerTransform.position - transform.position;
            // 2. 高さを固定する（箱が地面にめり込んだり浮いたりするのを防ぐ）
            direction.y = 0;
            // 3. 方向の長さを1に綺麗に整える（正規化）
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if (transform.position.x > 230f || transform.position.x < -200f ||transform.position.z > 150f || transform.position.z < -200f)
        {
            Destroy(gameObject);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!isClone) return;

        if (collision.gameObject.name == "Cube")
        {
            Destroy(gameObject);
        }
    }
 }
