using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Serializable]
    public class FighterObject
    {
        public string name;
        public GameObject obj;
    }

    [SerializeField]
    public FighterObject[] fighters;


    public bool fightInProgress;
    public GameObject hitParticle;
    private AudioManager audioManager;

    public Fighter player, enemy;

    public GameObject fightUI;
    public MenuUIHandler menuHandler;
    
    private int clockSeconds;

    public GameObject fightTextObject;

    public TextMeshProUGUI clockText;

    private int playerWins;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerCounter;
    public Slider playerHealth;
    public Slider playerStamina;

    private int enemyWins;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyCounter;
    public Slider enemyHealth;
    public Slider enemyStamina;

    private Vector3 startingPositionPlayer = new Vector3(13f, 0f, -1f);
    private Quaternion startingRotationPlayer = Quaternion.Euler(0f, 90f, 0f);

    private Vector3 startingPositionEnemy = new Vector3(17f, 0f, -1f);
    private Quaternion startingRotationEnemy = Quaternion.Euler(0f, -90f, 0f);

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Start()
    {
        audioManager.Play("Ambient");
    }

    void FixedUpdate()
    {
        if (fightInProgress)
        {
            HealthUpdate();
            StaminaUpdate();
        }
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

    public void EndFight(string tag)
    {
        fightInProgress = false;
        HealthUpdate();
        StaminaUpdate();

        if (tag == "Player")
        {
            audioManager.Play("GameOver");
            fightTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over";
            fightTextObject.GetComponent<CanvasGroup>().alpha = 1;
            enemyCounter.text = (++enemyWins).ToString();
            enemy.Celebration();
        }
        else
        {
            audioManager.Play("Victory");
            fightTextObject.GetComponent<TextMeshProUGUI>().text = "Victory";
            fightTextObject.GetComponent<CanvasGroup>().alpha = 1;
            playerCounter.text = (++playerWins).ToString();
            player.Celebration();
        }

        enemy.Freeze();
        player.Freeze();

        if (enemyWins == 1)
        {
            menuHandler.FightLost();
        } else if (playerWins == 1)
        {
            menuHandler.FightWin();
        } else
        {
            StartCoroutine(NewRoundCoroutine());
        }
    }

    public void StartFight()
    {
        fightUI.SetActive(true);
        fightInProgress = true;
        StartCoroutine(CountdowCoroutine());
    }

    public void FightEndedCleanup()
    {
        fightUI.SetActive(false);
        audioManager.Play("Ambient");
        audioManager.Stop("Combat");
        Destroy(player.gameObject);
        Destroy(enemy.gameObject);
    }

    public void FighterSelected(int index)
    {
        audioManager.Stop("Ambient");
        audioManager.Play("Combat");

        player = Instantiate(fighters[index].obj, startingPositionPlayer, startingRotationPlayer).GetComponent<Fighter>();
        playerName.text = fighters[index].name;
        player.tag = "Player";
        playerCounter.text = (playerWins = 0).ToString();

        enemy = Instantiate(fighters[0].obj, startingPositionEnemy, startingRotationEnemy).GetComponent<Fighter>();
        enemyName.text = fighters[0].name;
        enemyCounter.text = (enemyWins = 0).ToString();
    }

    IEnumerator NewRoundCoroutine()
    {
        yield return new WaitForSeconds(4);
        
        player.ResetStats();
        player.transform.position = startingPositionPlayer;

        enemy.ResetStats();
        enemy.transform.position = startingPositionEnemy;

        StartFight();
    }
    IEnumerator CountdowCoroutine()
    {
        clockSeconds = 30;
        clockText.text = clockSeconds.ToString();

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

        bool endedByTime = false;

        while (fightInProgress)
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
                endedByTime = true;
                break;
            }

            yield return new WaitForSeconds(1);
        }

        if (endedByTime)
        {
            if (player.health > enemy.health)
            {
                EndFight("Enemy");
            }
            else
            {
                EndFight("Player");
            }
        }
    }
}
