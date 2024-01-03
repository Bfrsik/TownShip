using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/Entity")]
public class Entity : ScriptableObject
{
    [SerializeField] public float maxHP;
    [SerializeField] public float speed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float stamina;
    [SerializeField] public float mass;
    [SerializeField] public float staminaRegeneration;

    //[SerializeField] private int size = 0;
    //[SerializeField] private bool canUseTools = false;
}
