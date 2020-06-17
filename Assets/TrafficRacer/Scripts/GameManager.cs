using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrafficRacer
{
    /// <summary>
    /// Script which keep track of game status and selected car 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager singeton;

        [HideInInspector] public GameStatus gameStatus = GameStatus.NONE;
        [HideInInspector] public int currentCarIndex = 0;

        private void Awake()
        {
            if (singeton == null)
            {
                singeton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}