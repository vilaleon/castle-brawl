using System;
using UnityEngine;

public class Stats : MonoBehaviour
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
    [Range(0.5f, 3f)]
    private float endurance = 1f;

    public float Health { get { return health; } set { health = value; } }

    public float Stamina { get { return stamina; } set { stamina = value; } }

    public float Strength { get { return strength; } set { strength = value; } }

    public float Agility { get { return agility; } set { agility = value; } }

    public float Endurance { get { return endurance; } set { endurance = value; } }
}
