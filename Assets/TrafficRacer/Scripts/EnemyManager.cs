using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    /// <summary>
    /// Script which manages spawning, activation and deactivation of enemies
    /// </summary>
    public class EnemyManager
    {
        private List<GameObject> deactiveEnemyList;                 //list to store deactive enemy gameobjects
        private Vector3[] enemySpawnPos = new Vector3[3];           //array of possible spawn position
        private GameObject enemyHolder;                             //parent object for all enemy objects
        private float moveSpeed;                                    //moving speed

        public EnemyManager(Vector3 spawnPos, float moveSpeed)      //constructor of script
        {       
            this.moveSpeed = moveSpeed;                             //set the speed
            deactiveEnemyList = new List<GameObject>();             //initialize list

            enemySpawnPos[0] = spawnPos - Vector3.right * 3;        //set 0 element of spawn position
            enemySpawnPos[1] = spawnPos;                            //set 1 element of spawn position
            enemySpawnPos[2] = spawnPos + Vector3.right * 3;        //set 2 element of spawn position
            //create gameobject of name EnemyHolder as assign to enemyHolder
            enemyHolder = new GameObject("EnemyHolder");           
        }

        public void SpawnEnemies(GameObject[] vehiclePrefabs)       //method to spawn enemies
        {
            for (int i = 0; i < vehiclePrefabs.Length; i++)         //loop through all the vehicles in the list
            {                                                       //spawn the enemy
                GameObject enemy = Object.Instantiate(vehiclePrefabs[i], enemySpawnPos[1], Quaternion.identity);
                enemy.SetActive(false);                             //deactivate the enemy
                enemy.transform.SetParent(enemyHolder.transform);   //set its parent
                enemy.name = "Enemy";                               //set the name
                enemy.AddComponent<EnemyController>();              //attach EnemyController componenet to it
                enemy.GetComponent<EnemyController>().SetDefault(moveSpeed, this);  
                deactiveEnemyList.Add(enemy);                       //add to deactiveEnemyList
            }
        }

        public void ActivateEnemy()                                 //method to activate the enemy
        {   
            if (deactiveEnemyList.Count > 0)                        //check if the deactiveEnemyList have elements
            {                                                       //if yes then randomly get anly one of them
                GameObject enemy = deactiveEnemyList[Random.Range(0, deactiveEnemyList.Count)];
                deactiveEnemyList.Remove(enemy);                    //remove the element from the list
                enemy.transform.position = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)];    //set spawn position
                enemy.SetActive(true);                              //activate the enemy
            }
        }

        public void DeactivateEnemy(GameObject enemy)               //method to deactivate the enemy
        {
            enemy.SetActive(false);                                 //deactivate the enemy
            deactiveEnemyList.Add(enemy);                           //add to deactiveEnemyList
        }
    }
}