using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Fighter fighter;

    private float pressDelay = 0.2f;
    private float pressDelayTimer = 0f;
    private bool pressedHorizontalAxis;

    void Start()
    {
        fighter = GetComponent<Fighter>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput < 0)
        {
            if (pressedHorizontalAxis && Time.time > pressDelayTimer)
            {
                switch (verticalInput)
                {
                    case 0:
                        {
                            fighter.Block(FighterBlock.BodyBlock);
                            break;
                        }
                    case > 0:
                        {
                            fighter.Block(FighterBlock.HeadBlock);
                            break;
                        }
                    case < 0:
                        {
                            fighter.Block(FighterBlock.LegBlock);
                            break;
                        }
                }
            }
            else
            {
                if (!pressedHorizontalAxis)
                {
                    pressedHorizontalAxis = true;
                    pressDelayTimer = Time.time + pressDelay;
                }
            }
        }
        else
        {
            if (pressedHorizontalAxis && Time.time < pressDelayTimer)
            {
                fighter.Move(-1);
            }
            else
            {
                pressedHorizontalAxis = false;
                fighter.Move(horizontalInput);
            }
        }

        if (Input.GetKeyDown(KeyCode.B)) fighter.AddMoveToQueue(FighterMove.LegKick);
        if (Input.GetKeyDown(KeyCode.N)) fighter.AddMoveToQueue(FighterMove.BodyKick);
        if (Input.GetKeyDown(KeyCode.M)) fighter.AddMoveToQueue(FighterMove.HeadKick);

        if (Input.GetKeyDown(KeyCode.G)) fighter.AddMoveToQueue(FighterMove.LegPunch);
        if (Input.GetKeyDown(KeyCode.H)) fighter.AddMoveToQueue(FighterMove.BodyPunch);
        if (Input.GetKeyDown(KeyCode.J)) fighter.AddMoveToQueue(FighterMove.HeadPunch);
    }
}
