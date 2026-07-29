using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    private CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float walkSpeed = 75.0f;
    [SerializeField] private float runSpeed = 150.0f;

    [SerializeField] private float waterSpeed = 0.01f;

    private bool isInWater = false;

    private float moveSpeed;

    [SerializeField] private float maxStamina = 5.0f;
    [SerializeField] private float stamina = 5.0f;
    [SerializeField] private float staminaRecovery = 1.5f;
    [SerializeField] private float staminaConsumption = 2.0f;
    [SerializeField] private float restamina = 1.5f;
    

    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text HealItemText;

    [SerializeField] private Image hpBarImage;
    [SerializeField] private Image staminaBatImage;

    [SerializeField] private int HP = 500;
    [SerializeField] private int MaxHP = 500;

    [SerializeField] private int HealItemCount = 3;
    [SerializeField] private int HealAmount = 150;

    [SerializeField] private GameObject PlayerCanvas;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSE;
    [SerializeField] private AudioClip healSE;

    public int Score = 0;

    bool isInvincible = false;
    [SerializeField] private float invincibleTime = 0.5f;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
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
            moveSpeed = runSpeed;
            stamina -= staminaConsumption * Time.deltaTime;
            restamina = 1.5f;
        }
        else
        {
            moveSpeed = walkSpeed;
            restamina -= Time.deltaTime;
            if (restamina <= 0.0f&&stamina <= maxStamina)
            {
                stamina += staminaRecovery * Time.deltaTime;
            }
        }
        if (isInWater)
        {
            moveSpeed *= waterSpeed;
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
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

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

        //staminaText.text = "Stamina:" + stamina.ToString("F1");
        
        ScoreText.text = "Score:" + Score.ToString();
        HealItemText.text = "HealItem ×" + HealItemCount;

        //HPText.text = "HP " + HP.ToString() + "/" + MaxHP.ToString();
        
        hpBarImage.fillAmount = (float)HP / MaxHP;
        HPText.text = $"HP {HP}/{MaxHP}";
        staminaBatImage.fillAmount = stamina / maxStamina;

    }

    public void Damage(int damage)
    {
        if (isInvincible) return;

        
        HP -= damage;

        StartCoroutine(Invincible());

        if (HP <= 0)
        {
            HP = 0;
            HPText.text = $"HP {HP}/{MaxHP}";
            GameManagerScript.Instance.GameOver();
            return;
        }
        //hpBarImage.fillAmount = (float)HP / MaxHP;
        HPText.text = $"HP {HP}/{MaxHP}";
        audioSource.PlayOneShot(hitSE);
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

        //hpBarImage.fillAmount = (float)HP / MaxHP;
        HPText.text = $"HP {HP}/{MaxHP}";


        Heel(HealAmount);

        HealItemCount--;
    }


    public void Heel(int heel)
    {
        audioSource.PlayOneShot(healSE);

        HP += heel;
        if (HP >= MaxHP)
        {
            HP = MaxHP;
        }
    }

    public void ScoreUp(int score)
    {
        if (GameManagerScript.Instance.isGameOver)
        {
            return;
        }
        Score += score;
    }

    IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}
