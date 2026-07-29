using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    [SerializeField] public GameObject GameCanvas;
    [SerializeField] public TMP_Text CenterText;
    [SerializeField] private TMP_Text surviveTimeText;
    
    [SerializeField] private GameObject TitleCanvas;
    [SerializeField] private TMP_Text StartText;
    [SerializeField] private TMP_Text TitleText;

    [SerializeField] public GameObject ResultCanvas;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text TimeText;
    [SerializeField] private TMP_Text TitleGoText;

    float countTime = 1.0f;
    public int CountDown = 3;


    public bool isGameOver = false;
    public bool isGameStart = false;
    public bool isResult = false;
    public bool isGameTitle = true;

    float surviveTime = 0f;

    int scorePerSecond = 100;
    float scoreTimer = 0f;

    int minutes;
    int seconds;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameoverSE;
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip inGameBGM;

    string cheatCode = "";
    public bool aaaaa = false;


    void Awake()
    {
        Instance = this;
        GameCanvas.SetActive(false);
        TitleCanvas.SetActive(false);
        ResultCanvas.SetActive(false);
    }

    void Start()
    {
        StartText.text = "Press the A button or Enterkey";
        TitleText.text ="Plant Escape";

        audioSource.clip = titleBGM;
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.Play();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameTitle)
        {
            TitleCanvas.SetActive(true);

            if (Keyboard.current.enterKey.wasPressedThisFrame||
                (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
            {
                audioSource.Stop();

                audioSource.clip = inGameBGM;
                audioSource.volume = 0.3f;
                audioSource.loop = true;
                

                isGameTitle = false;
            }

            //if (Keyboard.current.escapeKey.wasPressedThisFrame)
            //{
            //    Application.Quit();
            //}

            if (Keyboard.current.escapeKey.wasPressedThisFrame||
                (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
            }



            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                cheatCode += "Y";
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                cheatCode += "H";
            }

            if (Keyboard.current.jKey.wasPressedThisFrame)
            {
                cheatCode += "J";
            }

            if (cheatCode.Contains("YHJ"))
            {
                aaaaa = !aaaaa;
                cheatCode = "";
            }


            return;
        }
        TitleCanvas.SetActive(false);
        GameCanvas.SetActive(true);
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
            else if (CountDown == -1)
            {
                
                isGameStart = true;
                GameCanvas.SetActive(false);
            }
            else if (CountDown == 0)
            {
                
                CenterText.text = "GO!";
                audioSource.Play();

            }
        }


        if (isGameOver)
        {
            GameCanvas.SetActive(true);
            

            if (!isResult)
            {
                CenterText.text = "GameOver";
                isResult = true;
            }
        }

        if (isResult)
        {
           
            ScoreText.text = "スコア:" + Player.instance.Score;
            TimeText.text = "生存時間:" +$"{minutes:00}:{seconds:00}";
            TitleGoText.text = "Press the A button or Enterkey";

            if (Keyboard.current.enterKey.wasPressedThisFrame||
                (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
            {
                Restart();
            }
        }


        if (!isGameOver && isGameStart)
        {
            

            GameCanvas.SetActive(false);
            surviveTime += Time.deltaTime;


            minutes = (int)(surviveTime / 60);
            seconds = (int)(surviveTime % 60);

            surviveTimeText.text = $"{minutes:00}:{seconds:00}";


            if (Keyboard.current.escapeKey.wasPressedThisFrame||
                (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
            {
                Restart();
            }
           
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
        }
        else if (surviveTime >= 120)
        {
            scorePerSecond = 1000;
        }
        else if (surviveTime >= 90)
        {
            scorePerSecond = 500;
        }
        else if (surviveTime >= 60)
        {
            scorePerSecond = 300;
        }
        else if (surviveTime >= 15)
        {
            scorePerSecond = 150;
        }


    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        audioSource.Stop();
        audioSource.PlayOneShot(gameoverSE);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
