using Fighting;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    #region Stats
    public float health = 100;
    public float stamina = 100;

    [Range(0.5f, 1.5f)]
    public float strength = 1f;

    [Range(0.75f, 1.25f)]
    public float agility = 1f;

    [Range(0.75f, 2.25f)]
    public float endurance = 1f;
    #endregion

    private GameManager gameManager;
    private AudioManager audioManager;
    public Animator fighterAnimator;

    private Stats fighterMoveStats;
    private Queue<Move> movesQueue;

    private Move moveInExecution;
    private bool moveIsCombo;

    private Block blockInExecution;
    private bool isBlocking;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        audioManager = GetComponent<AudioManager>();
        fighterAnimator = GetComponent<Animator>();

        fighterMoveStats = new Stats();
        movesQueue = new Queue<Move>();
    }

    void Start()
    {
        fighterAnimator.SetFloat("MoveSpeedMultiplier", agility);
        fighterMoveStats.AdjustDamage(strength);
    }

    void Update()
    {
        if (fighterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ExecuteNextMoveInQueue();
        }
    }

    private void LateUpdate()
    {
        if (!isBlocking)
        {
            fighterAnimator.SetBool("Blocking", false);
            blockInExecution = Block.None;
        }

        isBlocking = false;
        fighterAnimator.SetFloat("Movement", 0);
    }

    void FixedUpdate()
    {
        if (gameManager.fightInProgress)
        {
            stamina += Time.deltaTime * 10 * endurance;
            if (stamina > 100) stamina = 100;
        }
    }

    public void AddMoveToQueue(Move move)
    {
        if (movesQueue.Count < 2)
        {
            fighterAnimator.SetBool("Blocking", false);
            movesQueue.Enqueue(move);
        }
    }

    private void ExecuteNextMoveInQueue()
    {
        Move move;
        if (movesQueue.TryDequeue(out move) && stamina >= fighterMoveStats.Get(move).stamina)
        {
            if (move == moveInExecution && !moveIsCombo)
            {
                moveIsCombo = true;
                fighterAnimator.SetTrigger("Combo");

                if (move.ToString().Contains("Kick"))
                {
                    audioManager.Play("Kick Grunt");
                }
                else
                {
                    audioManager.Play("Punch Grunt");
                }
            }
            else
            {
                moveIsCombo = false;
                moveInExecution = move;
            }

            stamina -= fighterMoveStats.Get(move).stamina;
            fighterAnimator.SetTrigger(move.ToString());
            gameManager.AttackRegistered();
        }
        else
        {
            moveIsCombo = false;
            moveInExecution = Move.None;
        }
    }

    public void Movement(float horizontalInput)
    {
        fighterAnimator.SetFloat("Movement", horizontalInput);
    }

    public void TryToBlock(Block block)
    {
        if (moveInExecution == Move.None)
        {
            if (fighterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                fighterAnimator.SetBool("Blocking", true);
            }

            isBlocking = true;
            blockInExecution = block;
        }
    }

    private void ProcessAttack(Stats.Move attackMove)
    {
        Vector3 contactPoint = transform.position + new Vector3(0, attackMove.height, 0);

        if (blockInExecution == attackMove.block)
        {
            stamina += 10;
            fighterAnimator.SetTrigger(blockInExecution.ToString());
            gameManager.BlockRegistered(contactPoint);
        }
        else
        {
            health -= attackMove.damage;
            movesQueue.Clear();
            fighterAnimator.StopPlayback();
            fighterAnimator.Play("Idle", 0);
            fighterAnimator.SetTrigger(attackMove.trigger);
            gameManager.HitRegistered(contactPoint);

            if (health <= 0)
            {
                Knockdown();
                gameManager.EndFight(tag);
            }
        }
    }

    public void Knockdown()
    {
        fighterAnimator.SetTrigger("Knockdown");
    }

    public void Celebration()
    {
        fighterAnimator.SetTrigger("Celebration");
    }

    public void Freeze()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<AIController>().enabled = false;
    }

    public void Unfreeze()
    {
        if (CompareTag("Player"))
        {
            GetComponent<PlayerController>().enabled = true;
        }
        else
        {
            GetComponent<AIController>().enabled = true;
        }
    }

    public void ResetStats()
    {
        health = 100;
        stamina = 100;

        foreach (var parametar in fighterAnimator.parameters)
        {
            if (parametar.type == AnimatorControllerParameterType.Trigger)
            {
                fighterAnimator.ResetTrigger(parametar.name);
            }
        }

        fighterAnimator.Play("Idle", 0);
        fighterAnimator.Play("Empty", 1);
    }

    private float attackDelayTime = 0.2f;
    private float attackDelayTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox") && moveInExecution != Move.None && Time.time > attackDelayTimer && gameManager.fightInProgress)
        {
            attackDelayTimer = Time.time + attackDelayTime;
            other.GetComponentInParent<Fighter>().ProcessAttack(fighterMoveStats.Get(moveInExecution));
        }
    }
}
