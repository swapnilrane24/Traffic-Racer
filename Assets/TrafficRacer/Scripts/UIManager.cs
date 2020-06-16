using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Common;

namespace TrafficRacer
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [SerializeField] private GameObject mainMenu, gameMenu, gameOverMenu, selectPanel, selectHolder, carHolder;
        [SerializeField] private Text distanceText;

        public Text DistanceText { get { return distanceText; } }

        private int currentSelectedCarIndex;
        private Vector3 defaultCarHolderPos;

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
            GameManager.singeton.gameStatus = GameStatus.NONE;
            InputManager.instance.swipeCallback += ActionOnSwipe;
            currentSelectedCarIndex = GameManager.singeton.currentCarIndex;
            defaultCarHolderPos = carHolder.transform.position;
            carHolder.transform.position -= Vector3.right * 8 * currentSelectedCarIndex;

            PopulateSelectPanel();
        }

        public void PlayButton()
        {
            mainMenu.SetActive(false);
            gameMenu.SetActive(true);
            GameManager.singeton.gameStatus = GameStatus.PLAYING;
            LevelManager.instance.EnemyManagerGet.ActivateEnemy();
            PlayerController.instance.GameStarted();
        }

        public void RetryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OpenSelectPanel(bool value)
        {
            if (value)
            {
                mainMenu.SetActive(false);
                selectPanel.SetActive(true);
                selectHolder.SetActive(true);
                SetCarHolderPos();
            }
            else
            {
                mainMenu.SetActive(true);
                selectPanel.SetActive(false);
                selectHolder.SetActive(false);
            }
        }

        void ActionOnSwipe(SwipeType swipeType)
        {
            switch (swipeType)
            {
                case SwipeType.RIGHT:
                    if (currentSelectedCarIndex > 0)
                    {
                        currentSelectedCarIndex--;
                    }
                    break;
                case SwipeType.LEFT:
                    if (currentSelectedCarIndex < LevelManager.instance.VehiclePrefabs.Length - 1)
                    {
                        currentSelectedCarIndex++;
                    }
                    break;
            }

            SetCarHolderPos();
        }

        void SetCarHolderPos()
        {
            float newXPos = defaultCarHolderPos.x - 8 * currentSelectedCarIndex;
            carHolder.transform.DOMoveX(newXPos, 0.5f);
        }

        public void SelectCarButton()
        {
            GameManager.singeton.currentCarIndex = currentSelectedCarIndex;
            PlayerController.instance.SpawnVehicle(GameManager.singeton.currentCarIndex);
            OpenSelectPanel(false);
        }

        void PopulateSelectPanel()
        {
            for (int i = 0; i < LevelManager.instance.VehiclePrefabs.Length; i++)
            {
                GameObject car = Instantiate(LevelManager.instance.VehiclePrefabs[i], carHolder.transform);
                car.transform.Rotate(new Vector3(0, 1, 0), 230);
                car.transform.localPosition = Vector3.right * i * 8;
            }
        }

        public void GameOver()
        {
            gameOverMenu.SetActive(true);
        }
    }
}