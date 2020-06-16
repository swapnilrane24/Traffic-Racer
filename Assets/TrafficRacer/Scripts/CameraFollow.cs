using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TrafficRacer
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow instance;

        private GameObject target;
        private Vector3 offset;
        private Vector3 changeZAxis;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
            offset = target.transform.position - transform.position;
            changeZAxis = transform.position;
        }

        private void LateUpdate()
        {
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING && target)
            {
                changeZAxis = target.transform.position - offset;
                transform.position = new Vector3(transform.position.x, transform.position.y, changeZAxis.z);
            }
        }
    }
}