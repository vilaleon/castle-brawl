using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("movement", horizontalInput);

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftShift)) animator.SetTrigger("bodyBlock");

        if (Input.GetKeyDown(KeyCode.B)) animator.SetTrigger("legKick");
        if (Input.GetKeyDown(KeyCode.N)) animator.SetTrigger("bodyKick");
        if (Input.GetKeyDown(KeyCode.M)) animator.SetTrigger("headKick");
        if (Input.GetKeyDown(KeyCode.G)) animator.SetTrigger("legPunch");
        if (Input.GetKeyDown(KeyCode.H)) animator.SetTrigger("bodyPunch");
        if (Input.GetKeyDown(KeyCode.J)) animator.SetTrigger("headPunch");
        if (Input.GetKeyDown(KeyCode.Space)) animator.SetTrigger("knockedout");
    }
}
