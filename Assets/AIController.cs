using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            fighterController.Move(1);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1.5f;
            }
        }

        if (cycle == 1)
        {
            fighterController.Move(0);
            fighterController.AddMoveToQueue(FighterMove.HeadPunch);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1f;
            }
        }

        if (cycle == 2)
        {
            fighterController.Move(-1);

            if (attackDelayTimer < Time.time)
            {
                cycle++;
                attackDelayTimer = Time.time + 1.5f;
            }
        }

        if (cycle == 3)
        {
            fighterController.Move(0);
            fighterController.AddMoveToQueue(FighterMove.LegKick);
            fighterController.AddMoveToQueue(FighterMove.BodyPunch);

            if (attackDelayTimer < Time.time)
            {
                cycle = 0;
                attackDelayTimer = Time.time + 1.5f;
            }
        }
    }
}
