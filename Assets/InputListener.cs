using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    private FighterController fighterController;

    // Start is called before the first frame update
    void Start()
    {
        fighterController = GetComponent<FighterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        fighterController.HorizontalMovement(horizontalInput);

        if (Input.GetKeyDown(KeyCode.B)) fighterController.LegKick();
        if (Input.GetKeyDown(KeyCode.N)) fighterController.BodyKick();
        if (Input.GetKeyDown(KeyCode.M)) fighterController.HeadKick();

        if (Input.GetKeyDown(KeyCode.G)) fighterController.LegPunch();
        if (Input.GetKeyDown(KeyCode.H)) fighterController.BodyPunch();
        if (Input.GetKeyDown(KeyCode.J)) fighterController.HeadPunch();
    }
}
