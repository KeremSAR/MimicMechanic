using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.Core{
    public class Timer : MonoBehaviour
    {
        [SerializeField] float timeRemaining = 60;
        [SerializeField] Text timeText;
        bool timerIsRunning = false;

        public event Action<bool> timeRunOut = delegate{};

        private void Start()
        {
            // Starts the timer automatically
            timerIsRunning = true;
        }

        void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    DisplayTime(timeRemaining);
                    timeRemaining -= Time.deltaTime;
                    
                }
                else
                {
                    timeRemaining = 0;
                    timeRunOut.Invoke(true);
                    timerIsRunning = false;
                }
            }
        }

        void DisplayTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }
}

