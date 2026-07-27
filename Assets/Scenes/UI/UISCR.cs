using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISCR : MonoBehaviour
{
    [Header("---UIの要素---")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Image hpBarImage;
    public TextMeshProUGUI hpText;
    public Image staminaBatImage;

    [Header("---ポップアップ設定---")]
    public GameObject grazePopupPrefab;
    public Transform canvasTransform;

    private float NowTime = 0.0f;//時間
    private int Score = 0;
    private float NowHP = 500.0f;//今のHP
    private float MaxHP = 500.0f;//さいだいHP 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NowTime += Time.deltaTime;
        UpdateTimer(NowTime);

        //テスト用
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("スペースキーが押された");
            AddScoreAndGraze(100);
        }

    }
    public void AddScoreAndGraze(int addscor)
    {
        Score += addscor;

        if (scoreText != null)
        {
            scoreText.text = "得点" + Score.ToString("00000");
        }
        else
        {
            Debug.LogWarning("scoreTextがアタッチされていません！");
        }

        if (grazePopupPrefab != null && canvasTransform != null)
        {
            GameObject popup = Instantiate(grazePopupPrefab, canvasTransform);
            //簡易的に中央に配置している
            popup.transform.localPosition = new Vector3(0, 0, 0);
            Destroy(popup, 1.0f);
        }

    }
    private void UpdateTimer(float timeSeconds)
    {
        int min = Mathf.FloorToInt(timeSeconds / 60);
        int sec = Mathf.FloorToInt(timeSeconds % 60);
        int millsec= Mathf .FloorToInt((timeSeconds * 100)%100);

        timerText.text = $"{min:00}:{sec:00}.{millsec:00}";

    }
     public void ChangeHP(float amount)
     {
        NowHP = Mathf.Clamp(NowHP + amount, 0, MaxHP);
        hpBarImage.fillAmount = NowHP / MaxHP;
        hpText.text = $"HP {NowHP}/{MaxHP}";
     }
    }
