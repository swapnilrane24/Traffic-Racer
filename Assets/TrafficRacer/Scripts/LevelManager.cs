using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        [SerializeField] private float moveSpeed = 8;
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject[] vehiclePrefabs;

        public GameObject[] VehiclePrefabs { get { return vehiclePrefabs; } }

        private List<GameObject> roads;
        private Vector3 nextRoadPos = Vector3.zero;
        private GameObject tempRoad, roadHolder;
        private EnemyManager enemyManager;
        private float distanceTravelled = 0;

        public EnemyManager EnemyManagerGet { get { return enemyManager; } }

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
            roadHolder = new GameObject("RoadHolder");

            roads = new List<GameObject>();

            //Spawn 5 road prefabs
            for (int i = 0; i < 5; i++)
            {
                GameObject road = Instantiate(roadPrefab, nextRoadPos, Quaternion.identity);
                road.transform.SetParent(roadHolder.transform);
                nextRoadPos += Vector3.forward * 10;
                roads.Add(road);
            }

            //Spawn enemies
            enemyManager = new EnemyManager(nextRoadPos, moveSpeed);
            enemyManager.SpawnEnemies(vehiclePrefabs);

            //Spawn car
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        private void MoveRoad()
        {
            for (int i = 0; i < roads.Count; i++)
            {
                roads[i].transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
            }

            if (roads[0].transform.position.z <= -10f)
            {
                roads[0].transform.position = roads[roads.Count - 1].transform.position + Vector3.forward * 10f;
                tempRoad = roads[0];
                roads.RemoveAt(0);
                roads.Add(tempRoad);
            }
        }

        private void Update()
        {
            if (GameManager.singeton.gameStatus != GameStatus.FAILED)
            {
                MoveRoad();
                distanceTravelled += moveSpeed * Time.deltaTime;
                UIManager.instance.DistanceText.text = string.Format("{0:0}", distanceTravelled);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Enemy")
            {
                enemyManager.ActivateEnemy();
            }
        }

        public void GameOver()
        {
            GameManager.singeton.gameStatus = GameStatus.FAILED;
            UIManager.instance.GameOver();
        }
    }
}