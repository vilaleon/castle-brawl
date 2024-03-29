using Fighting;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Fighter fighter;
    private GameManager gameManager;

    private float pressDelay = 0.2f;
    private float pressDelayTimer = 0f;
    private bool pressedHorizontalAxis;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        fighter = GetComponent<Fighter>();
    }

    void Update()
    {
        if (NetworkManager.IsListening && !IsOwner) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (NetworkManager.IsListening && !NetworkManager.IsHost) horizontalInput = -horizontalInput;
        if (Input.GetKeyDown(KeyCode.Escape)) gameManager.PauseGame();

        if (horizontalInput < 0)
        {
            if (pressedHorizontalAxis && Time.time > pressDelayTimer)
            {
                float verticalInput = Input.GetAxis("Vertical");

                switch (verticalInput)
                {
                    case 0:
                        {
                            fighter.TryToBlock(Block.BodyBlock);
                            break;
                        }
                    case > 0:
                        {
                            fighter.TryToBlock(Block.HeadBlock);
                            break;
                        }
                    case < 0:
                        {
                            fighter.TryToBlock(Block.LegBlock);
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
                fighter.Movement(-1);
            }
            else
            {
                pressedHorizontalAxis = false;
                fighter.Movement(horizontalInput);
            }
        }

        if (Input.GetKeyDown(KeyCode.B)) fighter.AddMoveToQueue(Move.LegKick);
        if (Input.GetKeyDown(KeyCode.N)) fighter.AddMoveToQueue(Move.BodyKick);
        if (Input.GetKeyDown(KeyCode.M)) fighter.AddMoveToQueue(Move.HeadKick);

        if (Input.GetKeyDown(KeyCode.G)) fighter.AddMoveToQueue(Move.LegPunch);
        if (Input.GetKeyDown(KeyCode.H)) fighter.AddMoveToQueue(Move.BodyPunch);
        if (Input.GetKeyDown(KeyCode.J)) fighter.AddMoveToQueue(Move.HeadPunch);
    }
}
