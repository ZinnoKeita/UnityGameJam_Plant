using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    [SerializeField] public TMP_Text CenterText;
    float countTime = 1.0f;
    public int CountDown = 3;


    public bool isGameOver = false;
    public bool isGameStart = false;
    public bool isResult = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

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
