using UnityEngine;
using Common;
using DG.Tweening;

namespace TrafficRacer
{
    /// <summary>
    /// Script which controls the player
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private Collider colliderComponent;                         //ref to collider component
        private float endXPos = 0;                                  //variable to change player x position

        private void OnDisable()
        {
            InputManager.instance.swipeCallback -= ActionOnSwipe;   //unsubscribe to the event
        }

        private void Start()
        {
            CameraFollow.instance.SetTarget(this.gameObject);       //set Camera target
            SpawnVehicle(GameManager.singeton.currentCarIndex);     //spawn the selected car
        }

        public void GameStarted()
        {
            InputManager.instance.swipeCallback += ActionOnSwipe;   //subscribe to the event
        }

        public void SpawnVehicle(int index)                         //method alled to spawn the selected car
        {
            if (transform.childCount > 0)                           //check for the children
            {
                Destroy(transform.GetChild(0).gameObject);          //destroy the 1st child
            }

                                                                    //spawn he selected car as child
            GameObject child = Instantiate(LevelManager.instance.VehiclePrefabs[index], transform);
            colliderComponent = child.GetComponent<Collider>();     //get reference to collider
            colliderComponent.isTrigger = true;                     //set isTrigger to true
        }

        void ActionOnSwipe(SwipeType swipeType)                     //method alled on swipe action of InputManager
        {
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING)  //is gamestatus is playing
            {
                switch (swipeType)
                {
                    case SwipeType.LEFT:                            //if we left swipe
                        endXPos = transform.position.x - 3;         //change endXPos by 3 to left
                        break;
                    case SwipeType.RIGHT:                           //if we right swipe
                        endXPos = transform.position.x + 3;         //change endXPos by 3 to right
                        break;
                }

                endXPos = Mathf.Clamp(endXPos, -3, 3);              //clamp endXPos between -3 and 3
                transform.DOMoveX(endXPos, 0.15f);                  //move the car
            }
        }

        private void OnTriggerEnter(Collider other)                 //Unity default mthod to detect collision
        {
            if (other.GetComponent<EnemyController>())              //check if the collided object has EnemyController on it
            {
                if (GameManager.singeton.gameStatus == GameStatus.PLAYING)  //check if gameStatus is PLAYING
                {
                    DOTween.Kill(this);                             //kill the dotween of this object
                    LevelManager.instance.GameOver();               //call GameOver
                    colliderComponent.isTrigger = false;            //set isTrigger to false
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;   //set isKinematic to false
                    gameObject.GetComponent<Rigidbody>().useGravity = true;     //set useGravity ture
                    //add a random force
                    gameObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitCircle.normalized * 100f);
                }
            }
        }
    }
}