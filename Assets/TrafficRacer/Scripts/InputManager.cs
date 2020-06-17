using System;
using UnityEngine;
using TrafficRacer;

namespace Common
{
    /// <summary>
    /// Script which manages the input of game
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        private float swipeThreshold = 0.15f;           //Threshold to conform swipe
        private Vector2 startPos, endPos;               //vector2 to decide swipe dircetion
        private Vector2 difference;                     //get the difference between startPos and endPos
        private SwipeType swipe = SwipeType.NONE;       //save swipeType
        private float swipeTimeLimit = 0.25f;           //TimeLimit to conform swipe
        private float startTime, endTime;               //times to get difference

        public Action<SwipeType> swipeCallback;         //SwipeType event trigger

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))                    //on mouse down
            {
                startPos = endPos = Input.mousePosition;        //set the startPos and endPos
                startTime = endTime = Time.time;                //set the startTime and endTime
            }

            if (Input.GetMouseButtonUp(0))                      //on mouse up
            {
                endPos = Input.mousePosition;                   //set the endPos
                endTime = Time.time;                            //set the endTime
                if (endTime - startTime <= swipeTimeLimit)      //check time difference
                {
                    DetectSwipe();                              //if less tha limit then call method
                }
            }
        }

        private void DetectSwipe()                              //decide swipe direction and swipe
        {
            swipe = SwipeType.NONE;
            difference = endPos - startPos;                     //get the difference
            if (difference.magnitude > swipeThreshold * Screen.width)   //check if magnitude is more than Threshold
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

            swipeCallback(swipe);                               //call the event
        }


    }
}