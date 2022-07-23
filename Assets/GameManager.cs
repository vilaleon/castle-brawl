using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool fightEnded;
    public GameObject hitParticle;
    private AudioManager audioManager;

    public static GameObject player, enemy;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        
    }

    public void HitRegistered(Vector3 contactPoint)
    {
        Instantiate(hitParticle, contactPoint, hitParticle.transform.rotation);
        audioManager.PlayRandomWithTag("hit");
    }

    public void BlockRegistered(Vector3 contactPoint)
    {
        audioManager.PlayRandomWithTag("block");
    }
}
