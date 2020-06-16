using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    public class EnemyManager
    {
        private List<GameObject> deactiveEnemyList;
        private Vector3[] enemySpawnPos = new Vector3[3];
        private GameObject enemyHolder;
        private float moveSpeed;

        public EnemyManager(Vector3 spawnPos, float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            deactiveEnemyList = new List<GameObject>();

            enemySpawnPos[0] = spawnPos - Vector3.right * 3;
            enemySpawnPos[1] = spawnPos;
            enemySpawnPos[2] = spawnPos + Vector3.right * 3;

            enemyHolder = new GameObject("EnemyHolder");
        }

        public void SpawnEnemies(GameObject[] vehiclePrefabs)
        {
            for (int i = 0; i < vehiclePrefabs.Length; i++)
            {
                GameObject enemy = Object.Instantiate(vehiclePrefabs[i], enemySpawnPos[1], Quaternion.identity);
                enemy.SetActive(false);
                enemy.transform.SetParent(enemyHolder.transform);
                enemy.name = "Enemy";
                enemy.AddComponent<EnemyController>();
                enemy.GetComponent<EnemyController>().SetDefault(moveSpeed, this);
                deactiveEnemyList.Add(enemy);
            }
        }

        public void ActivateEnemy()
        {
            if (deactiveEnemyList.Count > 0)
            {
                GameObject enemy = deactiveEnemyList[Random.Range(0, deactiveEnemyList.Count)];
                deactiveEnemyList.Remove(enemy);
                enemy.transform.position = enemySpawnPos[Random.Range(0, enemySpawnPos.Length)];
                enemy.SetActive(true);
            }
        }

        public void DeactivateEnemy(GameObject enemy)
        {
            enemy.SetActive(false);
            deactiveEnemyList.Add(enemy);
        }
    }
}