using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] Transform playerTransform;

    float radiusFactor = 30f;


    void Start() 
    {
        Spawn();
    }



    public void Spawn()
    {
        Instantiate(enemy, transform.position + (new Vector3 (Random.value, Random.value * 0.3f, Random.value) * radiusFactor), Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

}
