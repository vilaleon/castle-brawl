using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    private Animator fighterAnimator;
    private Queue<string> movesQueue;

    string moveInExecution;

    private float delayTime = 0.1f;
    private float attackDelayTimer = 0f;

    void Start()
    {
        fighterAnimator = GetComponent<Animator>();
        movesQueue = new Queue<string>();
    }

    void Update()
    {
        if (fighterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            moveInExecution = string.Empty;
            ExecuteNextMoveInQueue();
        }
    }

    private void AddMoveToQueue(string moveTrigger)
    {
        if (movesQueue.Count < 2)
        {
            if (movesQueue.Count == 0 && moveTrigger == moveInExecution)
            {
                fighterAnimator.SetTrigger("combo");
            }
            else
            {
                movesQueue.Enqueue(moveTrigger);
            }
        }
    }

    private void ExecuteNextMoveInQueue()
    {
        string moveTrigger;
        if (movesQueue.TryDequeue(out moveTrigger))
        {
            fighterAnimator.SetTrigger(moveTrigger);
            moveInExecution = moveTrigger;

            string nextMoveTrigger;
            if (movesQueue.TryPeek(out nextMoveTrigger))
            {
                if (moveTrigger == nextMoveTrigger)
                {
                    fighterAnimator.SetTrigger("combo");
                }
                movesQueue.Dequeue();
            }
        }
    }

    private void TryToBlock(string blockTrigger)
    {
        if (fighterAnimator.GetFloat("horizontalMovement") < 0 && moveInExecution == string.Empty)
        {
            fighterAnimator.SetTrigger(blockTrigger);
        }
    }

    public void LegBlock()
    {
        TryToBlock("legBlock");
    }

    public void BodyBlock() { }

    public void HeadBlock() { }

    public void LegKick()
    {
        AddMoveToQueue("legKick");
    }

    public void BodyKick()
    {
        AddMoveToQueue("bodyKick");
    }

    public void HeadKick()
    {
        AddMoveToQueue("headKick");
    }

    public void LegPunch()
    {
        AddMoveToQueue("legPunch");
    }

    public void BodyPunch()
    {
        AddMoveToQueue("bodyPunch");
    }

    public void HeadPunch()
    {
        AddMoveToQueue("headPunch");
    }

    public void HorizontalMovement(float input)
    {
        fighterAnimator.SetFloat("horizontalMovement", input);
    }
}
