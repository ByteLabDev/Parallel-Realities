using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehindPlayer : MonoBehaviour
{

    public GameObject enemy;
    public GameObject spawnArea;
    public Vector3 spawnOffset;
    public int rarity;
    
    void Update()
    {
        if (!enemy) return;
        int random = UnityEngine.Random.Range(1, rarity);
        
        if( random == 1 )
        {
            {
                GameObject enemyObj = Instantiate(enemy, transform.position + spawnOffset, Quaternion.identity);
            }
        } 
    }
}
