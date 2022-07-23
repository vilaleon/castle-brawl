using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private float health = 100;
    private float stamina = 100;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float strength = 1f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float agility = 1f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float endurance = 1f;


    private GameManager gameManager;
    private AudioManager audioManager;
    private Animator fighterAnimator;

    private FighterMoves fighterMoves;

    private Queue<FighterMove> movesQueue;

    private FighterMove moveInExecution;
    private FighterBlock blockInExecution;

    private bool moveIsCombo;

    private float attackDelayTime = 0.2f;
    private float attackDelayTimer = 0f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = GetComponent<AudioManager>();
        fighterAnimator = GetComponent<Animator>();
        fighterMoves = new FighterMoves();

        movesQueue = new Queue<FighterMove>();

        fighterAnimator.SetFloat("MoveSpeedMultiplier", agility);
        fighterMoves.AdjustDamage(strength);
    }

    void Update()
    {
        if (fighterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ExecuteNextMoveInQueue();
        }
    }

    void FixedUpdate()
    {
        stamina += Time.deltaTime * 10 * endurance;
        if (stamina > 100) stamina = 100;
    }

    public void AddMoveToQueue(FighterMove move)
    {
        if (movesQueue.Count < 2)
        {
            movesQueue.Enqueue(move);
        }
    }

    private void ExecuteNextMoveInQueue()
    {
        FighterMove move;
        if (movesQueue.TryDequeue(out move) && stamina >= fighterMoves.Get(move).stamina)
        {
            stamina -= fighterMoves.Get(move).stamina;
            Debug.Log("stamina: " + stamina);

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

            fighterAnimator.SetTrigger(move.ToString());
            
        }
        else
        {
            moveIsCombo = false;
            moveInExecution = FighterMove.None;
        }
    }

    public void Move(float horizontalInput)
    {
        fighterAnimator.SetFloat("Movement", horizontalInput);
    }

    public void Block(FighterBlock block)
    {
        if (moveInExecution == FighterMove.None)
        {
            blockInExecution = block;
        }
    }

    private void TryToBlock(FighterMoves.Move attackMove)
    {
        Vector3 contactPoint = transform.position + new Vector3(0, attackMove.height, 0);

        if (blockInExecution == attackMove.block)
        {
            fighterAnimator.Play("move_backward_B", 0);
            fighterAnimator.SetTrigger(blockInExecution.ToString());
            gameManager.BlockRegistered(contactPoint);
        }
        else
        {
            movesQueue.Clear();
            fighterAnimator.Play("Idle", 0);
            fighterAnimator.SetTrigger(attackMove.trigger);
            gameManager.HitRegistered(contactPoint);

            Debug.Log("damage: " + attackMove.damage);
            health -= attackMove.damage;
            Debug.Log("health: " + health);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox") && moveInExecution != FighterMove.None && Time.time > attackDelayTimer)
        {
            attackDelayTimer = Time.time + attackDelayTime;
            other.GetComponentInParent<Fighter>().TryToBlock(fighterMoves.Get(moveInExecution));
        }
    }
}
