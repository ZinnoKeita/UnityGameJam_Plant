using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    [SerializeField] public TMP_Text CenterText;
    [SerializeField] private TMP_Text surviveTimeText;
    [SerializeField] private GameObject GameCanvas;

    float countTime = 1.0f;
    public int CountDown = 3;


    public bool isGameOver = false;
    public bool isGameStart = false;
    public bool isResult = false;

    float surviveTime = 0f;

    int scorePerSecond = 100;
    float scoreTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDown>=-1)
        {
            countTime -= Time.deltaTime;

            if (countTime <= 0)
            {
                CountDown--;
                countTime = 1.0f;
            }
            if (CountDown > 0)
            {
                CenterText.text = CountDown.ToString();
            }
            else if (CountDown <= -1)
            {
                CenterText.text = " ";
            }
            else if (CountDown <= 0)
            {
                isGameStart = true;
                CenterText.text = "GO!";
            }
        }


        if (isGameOver)
        {
            GameCanvas.SetActive(false);

            if (!isResult)
            {
                CenterText.text = "GameOver";
                isResult = true;
            }

            if (isResult && Keyboard.current.jKey.wasPressedThisFrame)
            {
                Restart();
            }
        }

        if (!isGameOver && isGameStart)
        {

            GameCanvas.SetActive(true);
            surviveTime += Time.deltaTime;


            int minutes = (int)(surviveTime / 60);
            int seconds = (int)(surviveTime % 60);

            surviveTimeText.text = $"{minutes:00}:{seconds:00}";
        }
        

        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            Player.instance.ScoreUp(scorePerSecond);
            scoreTimer = 0f;
        }

        if (surviveTime >= 300)
        {
            scorePerSecond = 1500;
        }else if(surviveTime >= 120)
        {
            scorePerSecond = 1000;
        }else if(surviveTime >= 90)
        {
            scorePerSecond = 500;
        }else if(surviveTime >= 60)
        {
            scorePerSecond = 300;
        }else if(surviveTime >= 15)
        {
            scorePerSecond = 150;
        }

        
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("Game Over!");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
