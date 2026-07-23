using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public Transform startPoint;
    public Transform goalPoint;

    public float moveTime = 3f;

    float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < moveTime)
        {
            timer += Time.deltaTime;

            float t = timer / moveTime;

            transform.position =
                Vector3.Lerp(startPoint.position, goalPoint.position, t);

            transform.rotation =
                Quaternion.Lerp(startPoint.rotation, goalPoint.rotation, t);
        }
    }
}
