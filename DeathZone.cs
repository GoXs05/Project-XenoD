using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    [SerializeField] Transform respawnPos;

    private void OnCollisionEnter(Collision other) 
    {

        if ((other.gameObject.name == "Death Zone"))
        {
            transform.position = respawnPos.position;
        }
    }
}
