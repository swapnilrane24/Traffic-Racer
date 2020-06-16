using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrafficRacer;

namespace Common
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        private float swipeThreshold = 0.15f;

        private Vector2 startPos, endPos;
        private Vector2 difference;
        private SwipeType swipe = SwipeType.NONE;
        private float swipeTimeLimit = 0.25f;
        private float startTime, endTime;

        public Action<SwipeType> swipeCallback;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                startTime = endTime = Time.time; 
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPos = Input.mousePosition;
                endTime = Time.time;
                if (endTime - startTime <= swipeTimeLimit)
                {
                    DetectSwipe();
                }
            }
        }

        private void DetectSwipe()
        {
            swipe = SwipeType.NONE;
            difference = endPos - startPos;
            if (difference.magnitude > swipeThreshold * Screen.width)
            {
                if (difference.x > 0) //right swipe
                {
                    swipe = SwipeType.RIGHT;
                }
                else if (difference.x < 0) //left swipe
                {
                    swipe = SwipeType.LEFT;
                }
            }

            swipeCallback(swipe);
        }


    }
}