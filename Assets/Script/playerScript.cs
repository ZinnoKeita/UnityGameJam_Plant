using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
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

    public int HP = 600;
    public int MaxHP = 600;

    public int Score = 0;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.Instance.isGameOver
            || GameManagerScript.Instance.isGameStart == false)
        {
            return;
        }


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

        staminaText.text = "Stamina:" + stamina.ToString("F1");
        HPText.text = "HP:" + HP.ToString();
        ScoreText.text = "Score:" + Score.ToString();

        Damage(1);
        ScoreUp(1);
    }

    public void Damage(int damage)
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
            GameManagerScript.Instance.GameOver();
        }
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
}
