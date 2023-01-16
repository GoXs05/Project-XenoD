using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{

    [SerializeField] EnemyData enemyData;
    [SerializeField] private PlayerLevel playerLevel;

    private EnemySpawner spawner;

    private float health;
    private float shields;

    private void Awake() 
    {
        spawner = transform.parent.GetComponent<EnemySpawner>();
        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        health = enemyData.health;
        shields = enemyData.shields;
    }



    public void TakeDamage(float damage)
    {

        if (shields > 0)
        {
            shields -= damage;
        }
        else
        {
            health -= damage;
        }

        if (shields < 0)
        {
            shields = 0;
        }

        if (health <= 0)
        {
            spawner.Spawn();
            Destroy(gameObject);
            GiveXP();
        }
    }



    public void GiveXP()
    {
        playerLevel.levelXP += enemyData.XPDrop;
    }

}
