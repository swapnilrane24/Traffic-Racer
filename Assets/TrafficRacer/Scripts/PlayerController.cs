using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using DG.Tweening;

namespace TrafficRacer
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        private Collider colliderComponenet;
        private float endXPos = 0;

        private void OnDisable()
        {
            InputManager.instance.swipeCallback -= ActionOnSwipe;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            CameraFollow.instance.SetTarget(this.gameObject);
            SpawnVehicle(GameManager.singeton.currentCarIndex);
        }

        public void GameStarted()
        {
            InputManager.instance.swipeCallback += ActionOnSwipe;
        }

        public void SpawnVehicle(int index)
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            GameObject child = Instantiate(LevelManager.instance.VehiclePrefabs[index], transform);

            colliderComponenet = child.GetComponent<Collider>();
            colliderComponenet.isTrigger = true;
        }

        void ActionOnSwipe(SwipeType swipeType)
        {
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING)
            {
                switch (swipeType)
                {
                    case SwipeType.LEFT:
                        endXPos = transform.position.x - 3;
                        break;
                    case SwipeType.RIGHT:
                        endXPos = transform.position.x + 3;
                        break;
                }

                endXPos = Mathf.Clamp(endXPos, -3, 3);
                transform.DOMoveX(endXPos, 0.15f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())
            {
                if (GameManager.singeton.gameStatus == GameStatus.PLAYING)
                {
                    DOTween.Kill(this);
                    LevelManager.instance.GameOver();
                    colliderComponenet.isTrigger = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    gameObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle.normalized * 100f);
                }
            }
        }
    }
}