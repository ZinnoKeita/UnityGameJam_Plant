using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public Transform lobbyView;
    public Transform gameView;
    public Transform resultView;

    public float moveTime = 3f;

    float starttimer = 0f;
    float endtimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = lobbyView.position;
        transform.rotation = lobbyView.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.Instance.isGameTitle)
        {
            return;
        }
        if (starttimer < moveTime)
        {
            starttimer += Time.deltaTime;

            float t = starttimer / moveTime;

            transform.position =
                Vector3.Lerp(lobbyView.position, gameView.position, t);

            transform.rotation =
                Quaternion.Lerp(lobbyView.rotation, gameView.rotation, t);
        }

        if (GameManagerScript.Instance.isResult)
        {
            if (endtimer < moveTime)
            {
                endtimer += Time.deltaTime;

                float t = endtimer / moveTime;

                transform.position =
                Vector3.Lerp(gameView.position, resultView.position, t);

                transform.rotation =
                Quaternion.Lerp(gameView.rotation, resultView.rotation, t);
            }
            if (endtimer >= 3.0f)
            {
                GameManagerScript.Instance.CenterText.text = " ";
                GameManagerScript.Instance.ResultCanvas.SetActive(true);
            }
        }
    }
}
