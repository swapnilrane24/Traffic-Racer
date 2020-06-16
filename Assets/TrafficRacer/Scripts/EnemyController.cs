using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    public class EnemyController : MonoBehaviour
    {
        private float moveSpeed;

        EnemyManager enemyManager;
        public void SetDefault(float speed, EnemyManager enemyManager)
        {
            moveSpeed = speed;
            this.enemyManager = enemyManager;
        }

        private void Update()
        {
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING)
            {
                transform.Translate(-transform.forward * moveSpeed * 0.8f * Time.deltaTime);

                if (transform.position.z <= -10f)
                {
                    enemyManager.DeactivateEnemy(gameObject);
                }
            }
        }
    }
}