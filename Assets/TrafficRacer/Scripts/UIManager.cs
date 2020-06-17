using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Common;

namespace TrafficRacer
{
    /// <summary>
    /// This script is used to control the UI logic of the game
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

                                                                    //references to important panels and holders
        [SerializeField] private GameObject mainMenu, gameMenu, gameOverMenu, selectPanel, selectHolder, carHolder;
        [SerializeField] private Text distanceText;                 //ref to the text which shows the distance info

        public Text DistanceText { get { return distanceText; } }   //getter for distanceText

        private int currentSelectedCarIndex;                        //store the selected car
        private Vector3 defaultCarHolderPos;                        //store default position of CarHolder

        private void OnDisable()
        {
            InputManager.instance.swipeCallback -= ActionOnSwipe;   //removing event subscrition
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
            GameManager.singeton.gameStatus = GameStatus.NONE;                              //set the game status to None
            InputManager.instance.swipeCallback += ActionOnSwipe;                           //add event subscription
            currentSelectedCarIndex = GameManager.singeton.currentCarIndex;                 //set the selected car index
            defaultCarHolderPos = carHolder.transform.position;                             //set the default car holder position
            carHolder.transform.position -= Vector3.right * 8 * currentSelectedCarIndex;    //set the car holder position to respective selected car

            PopulateSelectPanel();                                                          
        }

        public void PlayButton()                                    //method called by play button                                        
        {
            mainMenu.SetActive(false);                              //deactivate main menu panel
            gameMenu.SetActive(true);                               //activate game menu panel
            LevelManager.instance.GameStarted();                    //inform LevelManager about game starting
        }

        public void RetryButton()                                   //method called by play button 
        {                                                           //reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OpenSelectPanel(bool value)                     //method called by Select button 
        {
            if (value)                                              //if value is true
            {
                mainMenu.SetActive(false);                          //deactivate main menu panel
                selectPanel.SetActive(true);                        //activate select panel
                selectHolder.SetActive(true);                       //activate select holder
                SetCarHolderPos();
            }
            else                                                    //if value is false
            {
                mainMenu.SetActive(true);                           //activate game menu panel
                selectPanel.SetActive(false);                       //deactivate select panel
                selectHolder.SetActive(false);                      //deactivate select holder
            }
        }

        void ActionOnSwipe(SwipeType swipeType)                     //method called on swipe action by InputManager
        {
            switch (swipeType)                          
            {
                case SwipeType.RIGHT:                               //if swipeType is right
                    if (currentSelectedCarIndex > 0)                //check if currentSelectedCarIndex more than zero
                    {
                        currentSelectedCarIndex--;                  //reduce the currentSelectedCarIndex by 1
                    }
                    break;
                case SwipeType.LEFT:                                //if swipeType is left
                                                                    //check if currentSelectedCarIndex less than total cars
                    if (currentSelectedCarIndex < LevelManager.instance.VehiclePrefabs.Length - 1)
                    {
                        currentSelectedCarIndex++;                  //increase the currentSelectedCarIndex by 1
                    }
                    break;
            }

            SetCarHolderPos();
        }

        void SetCarHolderPos()                                      //method to set position of CarHolder according to car index
        {
            float newXPos = defaultCarHolderPos.x - 8 * currentSelectedCarIndex;    //get the x position
            carHolder.transform.DOMoveX(newXPos, 0.5f);             //set the x position
        }

        public void SelectCarButton()                               //method called by Select car button
        {
            GameManager.singeton.currentCarIndex = currentSelectedCarIndex; //set the GameManager currentCarIndex
            LevelManager.instance.PlayerController.SpawnVehicle(GameManager.singeton.currentCarIndex); //spawn the respective car
            OpenSelectPanel(false);                                 
        }

        void PopulateSelectPanel()                                  //method which spawn all the cars in Select Panel
        {                                                           //loop thorugh no. of cars
            for (int i = 0; i < LevelManager.instance.VehiclePrefabs.Length; i++)
            {                                                       //spawn the car
                GameObject car = Instantiate(LevelManager.instance.VehiclePrefabs[i], carHolder.transform);
                car.transform.Rotate(new Vector3(0, 1, 0), 230);    //set its rotation
                car.transform.localPosition = Vector3.right * i * 8;//set its position
            }
        }

        public void GameOver()
        {
            gameOverMenu.SetActive(true);
        }
    }
}