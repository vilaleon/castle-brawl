using UnityEngine;
using Fighting;

public class AIController : MonoBehaviour
{
    private Fighter fighterController;

    private float attackDelayTimer = 0f;
    private int cycle = 0;

    void Start()
    {
        fighterController = GetComponent<Fighter>();
    }

    void Update()
    {
        if (cycle == 0)
        {
            fighterController.Movement(1);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1.5f;
            }
        }

        if (cycle == 1)
        {
            fighterController.Movement(0);
            fighterController.AddMoveToQueue(Move.HeadPunch);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1f;
            }
        }

        if (cycle == 2)
        {
            fighterController.Movement(-1);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1.5f;
            }
        }

        if (cycle == 3)
        {
            fighterController.Movement(0);
            fighterController.AddMoveToQueue(Move.LegKick);
            fighterController.AddMoveToQueue(Move.BodyPunch);

            if (attackDelayTimer < Time.time)
            {
                cycle = 0;
                attackDelayTimer = Time.time + 1.5f;
            }
        }
    }
}
