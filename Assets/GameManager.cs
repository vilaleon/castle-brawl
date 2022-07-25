using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public bool fightEnded;
    public GameObject hitParticle;
    private AudioManager audioManager;

    public static Fighter player, enemy;

    public TextMeshProUGUI playerName;
    public Slider playerHealth;
    public Slider playerStamina;

    public TextMeshProUGUI enemyName;
    public Slider enemyHealth;
    public Slider enemyStamina;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();

        playerName.text = player.name;
        enemyName.text = enemy.name;
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
}
