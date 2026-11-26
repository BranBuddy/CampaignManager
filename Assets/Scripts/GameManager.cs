using UnityEngine;
using CampaignManager.Singleton;
using CampaignManager.DifficultyManager;
using System;

namespace CampaignManager.GameManager
{   
    public class GameManager : Singleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }  
       
        public Difficulty DifficultyLevel;
        public int ChosenCandidateID;
        public static event Action StartGameEvent;

        public void StartGame()
        {
            StartGameEvent?.Invoke();
        }
    }
}

