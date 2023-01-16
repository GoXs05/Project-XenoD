using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy" , menuName = "Enemy")]

public class EnemyData : ScriptableObject
{
    
    [Header("HP")]
    public float health;
    public float shields;

    [Header("Shooting")]
    public float deviation;
    public float damage;
    public float timeBetweenAttacks;

    [Header("XP")]
    public int XPDrop;
}
