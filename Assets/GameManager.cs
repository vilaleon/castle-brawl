using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public bool fightEnded;
    public GameObject hitParticle;
    private AudioManager audioManager;

    public static Fighter player, enemy;

    public TextMeshProUGUI clockText;
    private int clockSeconds = 60;


    public GameObject fightTextObject;

    public TextMeshProUGUI playerName;
    public Slider playerHealth;
    public Slider playerStamina;

    public TextMeshProUGUI enemyName;
    public Slider enemyHealth;
    public Slider enemyStamina;

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Start()
    {
        playerName.text = player.name;
        enemyName.text = enemy.name;

        player.Freeze();
        enemy.Freeze();

        StartCoroutine(CountdowCoroutine());
    }


    void FixedUpdate()
    {
        HealthUpdate();
        StaminaUpdate();
    }

    public void HealthUpdate()
    {
        playerHealth.value = player.health;
        enemyHealth.value = enemy.health;
    }

    public void StaminaUpdate()
    {
        playerStamina.value = player.stamina;
        enemyStamina.value = enemy.stamina;
    }

    public void AttackRegistered()
    {
    }

    public void HitRegistered(Vector3 contactPoint)
    {
        Instantiate(hitParticle, contactPoint, hitParticle.transform.rotation);
        audioManager.PlayRandomWithTag("hit");
    }

    public void BlockRegistered(Vector3 contactPoint)
    {
        audioManager.PlayRandomWithTag("block");
    }

    public void FightEnd(string tag)
    {
        fightEnded = true;

        if (tag == "Player")
        {
            audioManager.Play("GameOver");
            enemy.Celebration();
        }
        else
        {
            audioManager.Play("Victory");
            player.Celebration();
        }

        enemy.Freeze();
        player.Freeze();
    }

    IEnumerator CountdowCoroutine()
    {
        audioManager.Play("Fight");

        fightTextObject.GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1);

        fightTextObject.GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1);

        fightTextObject.GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1);

        fightTextObject.GetComponent<TextMeshProUGUI>().text = "Fight!";
        fightTextObject.GetComponent<Animation>().Play("FadeOut");

        player.Unfreeze();
        enemy.Unfreeze();

        while (!fightEnded)
        {
            clockSeconds = clockSeconds - 1;
            clockText.text = clockSeconds.ToString();

            if (clockSeconds == 3)
            {
                audioManager.Play("Countdown");
                fightTextObject.GetComponent<CanvasGroup>().alpha = 1;
                fightTextObject.GetComponent<TextMeshProUGUI>().text = "3";
            }

            if (clockSeconds == 2)
            {
                fightTextObject.GetComponent<TextMeshProUGUI>().text = "2";
            }

            if (clockSeconds == 1)
            {
                fightTextObject.GetComponent<TextMeshProUGUI>().text = "1";
            }

            if (clockSeconds == 0)
            {
                fightTextObject.GetComponent<Animation>().Play("FadeOut");
                fightTextObject.GetComponent<TextMeshProUGUI>().text = "Time's Up!";

                if (player.health > enemy.health)
                {
                    FightEnd("Enemy");
                    fightTextObject.GetComponent<TextMeshProUGUI>().text = "Victory";
                }
                else
                {
                    FightEnd("Player");
                    fightTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over";
                }
            }

            if (clockSeconds == 0)
            {
                fightTextObject.GetComponent<Animation>().Play("FadeOut");
                fightTextObject.GetComponent<TextMeshProUGUI>().text = "Time's Up!";

                if (player.health > enemy.health)
                {
                    FightEnd("Enemy");
                    fightTextObject.GetComponent<TextMeshProUGUI>().text = "Victory";
                }
                else
                {
                    FightEnd("Player");
                    fightTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over";
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
}
