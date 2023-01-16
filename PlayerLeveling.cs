using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;


    void Update()
    {

        LevelSetter();

    }



    void LevelSetter()
    {

        if ((playerLevel.level <= 30) && playerLevel.levelXP >= (Mathf.Pow(2, playerLevel.level) * 500))
        {
            playerLevel.level++;
            playerLevel.levelXP = 0;
        }

    }
}
