using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : NetworkBehaviour
{
    [Serializable]
    public class FighterObject
    {
        public string name;
        public GameObject obj;
    }

    [SerializeField]
    public FighterObject[] fighters;

    [SerializeField]
    public FighterObject[] enemies;

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

    public NetworkVariable<float> player1Health = new NetworkVariable<float>(100);
    public NetworkVariable<float> player2Health = new NetworkVariable<float>(100);
    public NetworkVariable<float> player1Stamina = new NetworkVariable<float>(100);
    public NetworkVariable<float> player2Stamina = new NetworkVariable<float>(100);

    public NetworkVariable<ulong> hostObjectId = new NetworkVariable<ulong>(0);
    public NetworkVariable<ulong> playerObjectId = new NetworkVariable<ulong>(0);

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
        if (NetworkManager.IsListening)
        {
            playerHealth.value = player1Health.Value;
            enemyHealth.value = player2Health.Value;
        }
        else
        {
            playerHealth.value = player.health;
            enemyHealth.value = enemy.health;

            if (player.health <= 0)
            {
                player.Knockdown();
                EndFight("Player");
            }
            else if (enemy.health <= 0)
            {
                enemy.Knockdown();
                EndFight("");
            }
        }
    }

    public void StaminaUpdate()
    {
        if (NetworkManager.IsListening)
        {
            playerStamina.value = player1Stamina.Value;
            enemyStamina.value = player2Stamina.Value;
        }
        else
        {
            playerStamina.value = player.stamina;
            enemyStamina.value = enemy.stamina;
        }
    }

    public void HitRegistered(Vector3 contactPoint)
    {
        Instantiate(hitParticle, contactPoint, hitParticle.transform.rotation);
        audioManager.PlayRandomWithTag("hit");
    }

    [ClientRpc]
    public void HitRegisteredClientRpc(Vector3 contactPoint)
    {
        Instantiate(hitParticle, contactPoint, hitParticle.transform.rotation);
        audioManager.PlayRandomWithTag("hit");
    }

    [ServerRpc(RequireOwnership = false)]
    public void HitRegisteredServerRpc(Vector3 contactPoint)
    {
        HitRegisteredClientRpc(contactPoint);
    }

    public void BlockRegistered(Vector3 contactPoint)
    {
        audioManager.PlayRandomWithTag("block");
    }

    [ClientRpc]
    public void BlockRegisteredClientRpc(Vector3 contactPoint)
    {
        audioManager.PlayRandomWithTag("block");
    }

    public void StartFight()
    {
        if (NetworkManager.IsListening)
        {
            StartFightClientRpc();
        }
        else
        {
            fightUI.SetActive(true);
            fightInProgress = true;
            StartCoroutine(CountdowCoroutine());
        }
    }

    public void EndFight(string tag)
    {
        fightInProgress = false;

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

        if (enemyWins == 3)
        {
            menuHandler.FightLost();
        }
        else if (playerWins == 3)
        {
            menuHandler.FightWin();
        }
        else
        {
            StartCoroutine(NewRoundCoroutine());
        }
    }

    [ClientRpc]
    public void EndFightClientRpc(string tag, ClientRpcParams clientRpcParams = default)
    {
        fightInProgress = false;

        if (tag == "Player")
        {
            playerHealth.value = 0;
            audioManager.Play("Victory");
            fightTextObject.GetComponent<TextMeshProUGUI>().text = "Victory";
            fightTextObject.GetComponent<CanvasGroup>().alpha = 1;
            enemyCounter.text = (++enemyWins).ToString();
            GetNetworkObject(playerObjectId.Value).GetComponent<Fighter>().Celebration();
            GetNetworkObject(hostObjectId.Value).GetComponent<Fighter>().Knockdown();
        }
        else
        {
            enemyHealth.value = 0;
            audioManager.Play("GameOver");
            fightTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over";
            fightTextObject.GetComponent<CanvasGroup>().alpha = 1;
            playerCounter.text = (++playerWins).ToString();
            GetNetworkObject(hostObjectId.Value).GetComponent<Fighter>().Celebration();
            GetNetworkObject(playerObjectId.Value).GetComponent<Fighter>().Knockdown();
        }

        GetNetworkObject(playerObjectId.Value).GetComponent<Fighter>().Freeze();

        if (enemyWins == 3)
        {
            menuHandler.FightLost();

        }
        else if (playerWins == 3)
        {
            menuHandler.FightWin();
        }
        else
        {
            StartCoroutine(NewRoundCoroutine());
        }
    }

    public void FightEndedCleanup()
    {
        fightUI.SetActive(false);
        audioManager.Play("Ambient");
        audioManager.Stop("Combat");
        if (NetworkManager.IsListening)
        {
            if (IsHost)
            {
                try
                {
                    Destroy(player.gameObject);
                    Destroy(enemy.gameObject);
                } catch (Exception e) { Debug.Log(e); }
            }
            NetworkManager.Singleton.Shutdown();
        }
        else
        {
            try
            {
                Destroy(player.gameObject);
                Destroy(enemy.gameObject);
            } catch (Exception e) { Debug.Log(e); }
        }
    }

    public void FighterSelected(int index)
    {
        audioManager.Stop("Ambient");
        audioManager.Play("Combat");

        player = Instantiate(fighters[index].obj, startingPositionPlayer, startingRotationPlayer).GetComponent<Fighter>();
        playerName.text = fighters[index].name;
        player.tag = "Player";
        playerCounter.text = (playerWins = 0).ToString();

        enemy = Instantiate(enemies[0].obj, startingPositionEnemy, startingRotationEnemy).GetComponent<Fighter>();
        enemy.isAI = true;
        enemyName.text = enemies[0].name;
        enemyCounter.text = (enemyWins = 0).ToString();
    }

    public void FighterSelectedMultiplayer()
    {
        int indexP1 = UnityEngine.Random.Range(0, 2);
        GameObject player1 = Instantiate(fighters[indexP1].obj, startingPositionPlayer, startingRotationPlayer);
        player1.GetComponent<NetworkObject>().SpawnWithOwnership(0);
        player = player1.GetComponent<Fighter>();
        player.tag = "Player";

        int indexP2 = UnityEngine.Random.Range(0, 2);
        GameObject player2 = Instantiate(fighters[indexP2].obj, startingPositionEnemy, startingRotationEnemy);
        player2.GetComponent<NetworkObject>().SpawnWithOwnership(1);
        enemy = player2.GetComponent<Fighter>();

        hostObjectId.Value = player1.GetComponent<Unity.Netcode.NetworkObject>().NetworkObjectId;
        playerObjectId.Value = player2.GetComponent<Unity.Netcode.NetworkObject>().NetworkObjectId;
        FighterSelectedClientRpc(indexP1, indexP2);
    }

    IEnumerator NewRoundCoroutine()
    {
        yield return new WaitForSeconds(4);

        if (NetworkManager.IsListening)
        {
            if (NetworkManager.IsHost)
            {
                player1Health.Value = 100;
                player2Health.Value = 100;
                player1Stamina.Value = 100;
                player2Stamina.Value = 100;

                player.ResetStats();
                player.transform.position = startingPositionPlayer;
                enemy.ResetStats();

            }
            else
            {
                GetNetworkObject(hostObjectId.Value).GetComponent<Fighter>().ResetStats();
                GetNetworkObject(playerObjectId.Value).GetComponent<Fighter>().ResetStats();
                GetNetworkObject(playerObjectId.Value).transform.position = startingPositionEnemy;
            }
        }
        else
        {
            player.ResetStats();
            player.transform.position = startingPositionPlayer;

            enemy.ResetStats();
            enemy.transform.position = startingPositionEnemy;
        }
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

        if (NetworkManager.IsListening && !NetworkManager.IsHost)
        {
            GetNetworkObject(playerObjectId.Value).GetComponent<Fighter>().Unfreeze();
        }
        else
        {
            player.Unfreeze();
            enemy.Unfreeze();
        }

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
            if (NetworkManager.IsListening)
            {
                if (NetworkManager.IsHost)
                {
                    if (player1Health.Value > player2Health.Value)
                    {
                        EndFight("");
                        EndFightClientRpc("", new ClientRpcParams
                        {
                            Send = new ClientRpcSendParams
                            {
                                TargetClientIds = new ulong[] { 1 }
                            }
                        });
                    }
                    else
                    {
                        EndFight("Player");
                        EndFightClientRpc("Player", new ClientRpcParams
                        {
                            Send = new ClientRpcSendParams
                            {
                                TargetClientIds = new ulong[] { 1 }
                            }
                        });
                    }
                }
            }

            else
            {
                if (player.health > enemy.health)
                {
                    EndFight("");
                }
                else
                {
                    EndFight("Player");
                }
            }

        }
    }

    [ClientRpc]
    public void FighterSelectedClientRpc(int indexP1 = 0, int indexP2 = 0, ClientRpcParams clientRpcParams = default)
    {
        audioManager.Stop("Ambient");
        audioManager.Play("Combat");

        playerName.text = fighters[indexP1].name;
        enemyName.text = fighters[indexP2].name;

        playerCounter.text = (playerWins = 0).ToString();
        enemyCounter.text = (enemyWins = 0).ToString();
    }

    [ClientRpc]
    public void StartFightClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (IsHost)
        {
            player1Health.Value = 100;
            player2Health.Value = 100;
        }
        fightUI.SetActive(true);
        fightInProgress = true;
        StartCoroutine(CountdowCoroutine());
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerHealthServerRpc(bool isHost, float value)
    {
        if (!isHost)
        {
            player1Health.Value += value;
        }
        else
        {
            player2Health.Value += value;
        }

        if (player1Health.Value <= 0)
        {
            HealthUpdate();
            player.Knockdown();
            EndFightClientRpc("Player", new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 }
                }
            });
            EndFight("Player");
        }
        else if (player2Health.Value <= 0)
        {
            HealthUpdate();
            enemy.Knockdown();
            EndFightClientRpc("", new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { 1 }
                }
            });
            EndFight("");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangePlayerStaminaServerRpc(bool isHost, float value)
    {
        if (isHost)
        {
            player1Stamina.Value += value;
            if (player1Stamina.Value > 100) player1Stamina.Value = 100;
        }
        else
        {
            player2Stamina.Value += value;
            if (player2Stamina.Value > 100) player2Stamina.Value = 100;
        }
    }

}
