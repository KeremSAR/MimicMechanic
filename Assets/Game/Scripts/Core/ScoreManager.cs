using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using UnityEngine.Serialization;

namespace Game.Core{
    public class ScoreManager : MonoBehaviour
    {
        //list of text
        //get all players with mimicstat
        //assign text to each player
        [Header("Reference of TextDisplay")]
        [SerializeField] List<Text> scoreDisplay = new List<Text>();
        
        [Header("Holder of all existing player's HoleStats")]
        [SerializeField] List<MimicStats> mimicStats = new List<MimicStats>();
        [SerializeField] GameObject promptBox;
        [SerializeField] List<Text> scorePromptTexts = new List<Text>();
        [SerializeField] Timer timer;

        private void Start()
        {
            MimicStats[] allstats = FindObjectsOfType<MimicStats>();
            foreach (MimicStats mimicstat in allstats)
            {
                mimicStats.Add(mimicstat);
            }
            updateStanding();

            timer = this.GetComponent<Timer>();
            timer.timeRunOut += ShowPrompt;
            
        }

        public void SortPoints()
        {
            int listSize = mimicStats.Count;
            for (int i = 0; i < listSize - 1; i++)
            {
                for (int j = 0; j < listSize - i - 1; j++)
                {
                    if (mimicStats[j].GetPoints() < mimicStats[j + 1].GetPoints())
                    {
                        MimicStats tempMimicStat = mimicStats[j];
                        mimicStats[j] = mimicStats[j + 1];
                        mimicStats[j + 1] = tempMimicStat;
                    }
                }
            }

          
        }

        public void updateStanding()
        {
            StartCoroutine(updateScore());
        }

        IEnumerator updateScore()
        {
            SortPoints();
            for (int i = 0; i < mimicStats.Count; i++)
            {
                scoreDisplay[i].text = mimicStats[i].gameObject.name + "  :  " + mimicStats[i].GetPoints();
                yield return null;
            }

        }

        void ShowPrompt(bool timerEnds){
            if(timerEnds){
                SortPoints();
                for (int i = 0; i < mimicStats.Count; i++)
                {
                    scorePromptTexts[i].text = mimicStats[i].gameObject.name + "  :  " + mimicStats[i].GetPoints();
                    
                }
                promptBox.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            timer.timeRunOut -= ShowPrompt;
        }
    }
}

