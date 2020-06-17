using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TrafficRacer
{
    /// <summary>
    /// This script controls the game, from spawning enemy, road, player to setting game status
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        [SerializeField] private float moveSpeed = 8;               //speed with which game moves
        [SerializeField] private GameObject roadPrefab;             //ref to raod prefab
        [SerializeField] private GameObject[] vehiclePrefabs;       //ref to all vehicle prefabs


        private List<GameObject> roads;                             //list to store the roads spawned in the game
        private Vector3 nextRoadPos = Vector3.zero;                 //position for next road
        private GameObject tempRoad, roadHolder;                    //variables
        private EnemyManager enemyManager;                          //variable to store EnemyManager
        private PlayerController playerController;                  //variable to store PlayerController
        private float distanceTravelled = 0;                        //track distance travelled
        private int trackRoadAtIndex = 0, lastTrack = 0;            //this variable are used to Reuse the roads in the game

        public GameObject[] VehiclePrefabs { get { return vehiclePrefabs; } }           //getter
        public PlayerController PlayerController { get { return playerController; } }   //getter

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            roadHolder = new GameObject("RoadHolder");                  //create gameobject with name RoadHolder
            roads = new List<GameObject>();                             //initialize the roads list

            for (int i = 0; i < 5; i++)                                 //Spawn 5 road prefabs
            {
                GameObject road = Instantiate(roadPrefab, nextRoadPos, Quaternion.identity);    //spawn the road
                road.transform.SetParent(roadHolder.transform);         //set its parent
                nextRoadPos += Vector3.forward * 10;                    //set the nextRoadPos
                roads.Add(road);                                        //add road to roads list
            }

            enemyManager = new EnemyManager(nextRoadPos, moveSpeed);    //create EnemyManager
            enemyManager.SpawnEnemies(vehiclePrefabs);                  //spawn enemies

            SpawnPlayer();
        }

        void SpawnPlayer()                                              //method used to spawn player
        {
            GameObject player = new GameObject("Player");               //create Player gameobject
            player.transform.position = Vector3.zero;                   //set its position to zero
            player.AddComponent<PlayerController>();                    //attach PlayerController script to it
            playerController = player.GetComponent<PlayerController>(); //get reference to PlayerController
        }

        private void MoveRoad()                                         //method responsible for moving roads
        {
            for (int i = 0; i < roads.Count; i++)                       //loop through roads list
            {                                                           //move each road with moveSpeed
                roads[i].transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
            }

            if (roads[trackRoadAtIndex].transform.position.z <= -10f)     //if the road at 0th element is at z distance below -10
            {
                //lastTrack = trackRoadAtIndex - 1;                       //set lastTrack
                //if (lastTrack < 0)                                      //check if lastTrack is less than 0
                //{
                //    lastTrack = roads.Count - 1;                        //set lastTrack to last index of road
                //}

                //set lastTrack, check if lastTrack is less than 0, set lastTrack to last index of road
                lastTrack = (trackRoadAtIndex - 1) < 0 ? roads.Count - 1 : trackRoadAtIndex - 1;
                                                                        //change its position
                roads[trackRoadAtIndex].transform.position = roads[lastTrack].transform.position + Vector3.forward * 10f;

                //increase trackRoadAtIndex by 1, check if trackRoadAtIndex more or equal to roads elments, set trackRoadAtIndex to zero
                trackRoadAtIndex = ++trackRoadAtIndex >= roads.Count ? 0 : trackRoadAtIndex;

                //trackRoadAtIndex++;                                     //increase trackRoadAtIndex by 1
                //if (trackRoadAtIndex >= roads.Count)                    //check if trackRoadAtIndex more or equal to roads elments
                //{
                //    trackRoadAtIndex = 0;                               //set trackRoadAtIndex to zero
                //}
            }
        }

        public void GameStarted()                                   //method called when Play button is clicked
        {
            GameManager.singeton.gameStatus = GameStatus.PLAYING;   //set the game status to Playing
            enemyManager.ActivateEnemy();                           //activate the enemy
            playerController.GameStarted();                         //inform player about game starting
        }

        private void Update()
        {
            if (GameManager.singeton.gameStatus != GameStatus.FAILED)   //check if gamestatus is not Failed
            {
                MoveRoad();                                             //move the roads
            }

            if (GameManager.singeton.gameStatus == GameStatus.PLAYING)  //check if gamestatus is not PLAYING
            {
                distanceTravelled += moveSpeed * Time.deltaTime;        //update the distanceTravelled
                                                                        //update DistanceText
                UIManager.instance.DistanceText.text = string.Format("{0:0}", distanceTravelled);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())               //check if the collided object has EnemyController on it        
            {
                enemyManager.ActivateEnemy();                           //ActivateEnemy
            }
        }

        public void GameOver()                                  
        {
            GameManager.singeton.gameStatus = GameStatus.FAILED;    //set gameStatus to FAILED
            //do camera shake adn after 1sec call UIManager GameOver method
            Camera.main.transform.DOShakePosition(1f, Random.insideUnitCircle.normalized, 5, 10f, false, true).OnComplete
                (
                        () => UIManager.instance.GameOver()
                );
        }
    }
}