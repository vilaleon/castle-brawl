using UnityEngine;
using Fighting;

public class AIController : MonoBehaviour
{
    public enum level {easy, medium, hard};
    public level difficulty;

    private Fighter fighterController;
    private GameManager gameManager;

    private float playerDistance;
    bool inHandReach, inLegReach;
    const float HAND_REACH = 0.8f, LEG_REACH = 1.35f;
    Block blockInExecution = Block.None;
    float blockCooldown = 0f;
    int cycleCooldown = 0;
    int randomNumber;

    void Start()
    {
        fighterController = GetComponent<Fighter>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (blockInExecution != Block.None) fighterController.TryToBlock(blockInExecution);
    }

    void FixedUpdate()
    {
        cycleCooldown = ++cycleCooldown % 10;
        if (cycleCooldown != 0) return;
        playerDistance = gameObject.transform.position.x - gameManager.player.transform.position.x;
        inHandReach = playerDistance <= HAND_REACH;
        inLegReach = playerDistance <= LEG_REACH;
        randomNumber = Random.Range(1, 100);

        if (difficulty == level.hard)
        {
            if (Time.time < blockCooldown) return;
            if (inLegReach)
            {
                Block block = gameManager.player.GetBlock();
                if (randomNumber <= 50 && block != Block.None)
                {
                    blockInExecution = block;
                    blockCooldown = Time.time + 1f;
                }
                else 
                {
                    blockInExecution = Block.None;
                    if (inHandReach)
                    {
                        if (randomNumber <= 30) fighterController.AddMoveToQueue(Move.LegPunch);
                        else if (randomNumber <= 50) fighterController.AddMoveToQueue(Move.HeadPunch);
                        else if (randomNumber <= 65) fighterController.AddMoveToQueue(Move.BodyPunch);
                    }
                    else
                    {

                        if (fighterController.stamina > 35)
                        {
                            if (randomNumber <= 30) fighterController.AddMoveToQueue(Move.BodyKick);
                            else if (randomNumber <= 50) fighterController.AddMoveToQueue(Move.LegKick);
                            else if (randomNumber <= 65) fighterController.AddMoveToQueue(Move.HeadKick);
                        }
                        else
                        {
                            fighterController.Movement(-1);
                        }
                    }
                }
            }
            else
            {
                blockInExecution = Block.None;
                if (randomNumber <= 50)
                {
                    fighterController.Movement(0);
                }
                else if (fighterController.stamina < 45 && playerDistance < 2)
                {
                    fighterController.Movement(-1);
                }
                else
                {
                    fighterController.Movement(1);
                }
            }
        }
        else if (difficulty == level.medium)
        {
            if (!inLegReach && randomNumber <= 35) fighterController.Movement(1);
            else if (inHandReach)
            {
                if (randomNumber <= 15) fighterController.AddMoveToQueue(Move.LegPunch);
                else if (randomNumber <= 30) fighterController.AddMoveToQueue(Move.BodyPunch);
                else if (randomNumber <= 60) fighterController.AddMoveToQueue(Move.HeadPunch);
                else if (randomNumber <= 80) fighterController.AddMoveToQueue(Move.LegKick);
            } 
            else if (randomNumber <= 20) fighterController.Movement(-1);
            else if (randomNumber <= 50) fighterController.AddMoveToQueue(Move.HeadPunch);
            else if (randomNumber <= 60) fighterController.AddMoveToQueue(Move.LegKick);
            else if (randomNumber <= 70) fighterController.AddMoveToQueue(Move.HeadKick);
        }
        else
        {
            if (randomNumber <= 30) fighterController.Movement(1);
            else if (randomNumber <= 40) fighterController.Movement(-1);
            else if (randomNumber <= 50) fighterController.AddMoveToQueue(Move.BodyKick);
            else if (randomNumber <= 60) fighterController.AddMoveToQueue(Move.HeadKick);
            else if (randomNumber <= 65) fighterController.AddMoveToQueue(Move.BodyPunch);
            else if (randomNumber <= 70) fighterController.AddMoveToQueue(Move.HeadPunch);
        }
    }
}
