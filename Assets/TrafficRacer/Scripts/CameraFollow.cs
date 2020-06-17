using UnityEngine;

/// <summary>
/// This script is used to make camera follow the target object only in z direction
/// </summary>
namespace TrafficRacer
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow instance;

        private GameObject target;              //variable to store the target
        private Vector3 offset;                 //variable to store the position differences between camera and target
        private Vector3 changeZAxis;            //vector3 used to change the position of camera

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void SetTarget(GameObject target)                        //method used to set the target
        {
            this.target = target;                                       //set the target
            offset = target.transform.position - transform.position;    //set the offset
            changeZAxis = transform.position;                           //set the changeZAxis
        }

        //we use LateUpdate to update out camera as we are updating target position in Update method
        private void LateUpdate()
        {
            //we check if game is in playing mode and target is not null
            if (GameManager.singeton.gameStatus == GameStatus.PLAYING && target)
            {
                changeZAxis = target.transform.position - offset;       //set the changeZAxis
                //set the z axis of camera
                transform.position = new Vector3(transform.position.x, transform.position.y, changeZAxis.z);
            }
        }
    }
}