//
//クサカレールの本体

using UnityEngine;

public class karehaGachi : MonoBehaviour
{
    [SerializeField] private GameObject kusakaBottlePrefab;

    [Header("出現範囲の制限")]
    [SerializeField] private float minX = -10.0f;
    [SerializeField] private float maxX = 10.0f;
    [SerializeField] private float minZ = -10.0f;
    [SerializeField] private float maxZ = 10.0f;
    [SerializeField] private float spawnHeightY = 0.5f;

    [Header("スポーン間隔（秒単位）")]
    [SerializeField] private float spawnInterval = 15.0f;

    private bool isSettingDone = false;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.Instance.isGameStart)
        {
            return;
        }

        if (!isSettingDone)
        {
            if (GameManagerScript.Instance.aaaaa)
            {
                spawnInterval = 0.5f;
            }

            isSettingDone = true;
        }

        if (GameManagerScript.Instance.isGameOver)
        {
            return;
        }



        timer += Time.deltaTime;

        if(timer>=spawnInterval)
        {
            KusakareBottle();
            timer = 0.0f;

        }

    }

    void KusakareBottle()
    {
        if (kusakaBottlePrefab == null) return;


        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(randomX,spawnHeightY,randomZ);

        float randomYRotation = Random.Range(0.0f, 360.0f);
        Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

        //ボトル生成
        Instantiate(kusakaBottlePrefab, spawnPosition, randomRotation);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minX + maxX) / 2f, spawnHeightY, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(maxX - minX, 0.2f, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);
    }
}
