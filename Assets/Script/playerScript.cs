using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;


public class Player : MonoBehaviour
{
    public static Player instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float MoveSpeed = 5.0f;

    [SerializeField] private float maxStamina = 5.0f;
    [SerializeField] private float stamina = 5.0f;
    [SerializeField] private float staminaRecovery = 1.5f;
    [SerializeField] private float staminaConsumption = 2.0f;
    [SerializeField] private float restamina = 1.5f;

    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text HealItemText;

    [SerializeField] private int HP = 600;
    [SerializeField] private int MaxHP = 600;

    [SerializeField] private int HealItemCount = 3;
    [SerializeField] private int HealAmount = 150;

    [SerializeField] private GameObject PlayerCanvas;

    public int Score = 0;

    bool isInvincible = false;
    [SerializeField] private float invincibleTime = 0.5f;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.Instance.isGameOver
            || GameManagerScript.Instance.isGameStart == false)
        {
            PlayerCanvas.SetActive(false);
            return;
        }
        PlayerCanvas.SetActive(true);

        Vector3 move = Vector3.zero;

        if (Keyboard.current.shiftKey.isPressed && stamina >= 0.0f)
        {
            MoveSpeed = 10.0f;
            stamina -= staminaConsumption * Time.deltaTime;
            restamina = 1.5f;
        }
        else
        {
            MoveSpeed = 5.0f;
            restamina -= Time.deltaTime;
            if (restamina <= 0.0f&&stamina <= maxStamina)
            {
                stamina += staminaRecovery * Time.deltaTime;
            }
        }


        if (Keyboard.current.wKey.isPressed)
        {
            move += Vector3.forward;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            move += Vector3.back;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            move += Vector3.left;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            move += Vector3.right;
        }
        transform.position += move.normalized * MoveSpeed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime);
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            UseHealItem();
        }

        staminaText.text = "Stamina:" + stamina.ToString("F1");
        HPText.text = "HP:" + HP.ToString();
        ScoreText.text = "Score:" + Score.ToString();
        HealItemText.text = "Heal ×" + HealItemCount;

    }

    public void Damage(int damage)
    {
        if (isInvincible) return;

        HP -= damage;

        StartCoroutine(Invincible());

        if (HP <= 0)
        {
            HP = 0;
            HPText.text = "HP:" + HP.ToString();
            GameManagerScript.Instance.GameOver();
            return;
        }
        HPText.text = "HP:" + HP.ToString();

    }

    public void UseHealItem()
    {
        if (HealItemCount <= 0)
        {
            return;
        }

        if (HP >= MaxHP)
        {
            return;
        }

        Heel(HealAmount);

        HealItemCount--;
    }


    public void Heel(int heel)
    {
        HP += heel;
        if (HP >= MaxHP)
        {
            HP = MaxHP;
        }
    }

    public void ScoreUp(int score)
    {
        Score += score;
    }

    IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }
}
