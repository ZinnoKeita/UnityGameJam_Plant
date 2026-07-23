using UnityEngine;

public class Cube : MonoBehaviour
{

    public float moveSpeed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // キャラクターの「前方（ローカル座標のZ軸）」に向かって進む
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
