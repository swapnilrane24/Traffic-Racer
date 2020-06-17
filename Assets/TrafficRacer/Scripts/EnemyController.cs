using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    /// <summary>
    /// Script which controls the enemy
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        private float moveSpeed;                        //variable to store speed
        EnemyManager enemyManager;                      //variable to store EnemyManager
        public void SetDefault(float speed, EnemyManager enemyManager)  //set speed and EnemyManager
        {
            moveSpeed = speed;
            this.enemyManager = enemyManager;
        }

        private void Update()
        {
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING)  //if gameStatus is PLAYING
            {
                transform.Translate(-transform.forward * moveSpeed * 0.8f * Time.deltaTime);    //move the gameobject

                if (transform.position.z <= -10f)                       //if object z is less than -10
                {
                    enemyManager.DeactivateEnemy(gameObject);           //deactvate the object
                }
            }
        }
    }
}