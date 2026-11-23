using UnityEngine;
using CampaignManager.Singleton;
using CampaignManager.DifficultyManager;

namespace CampaignManager.GameManager
{   
    public class GameManager : Singleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }  
       
        public Difficulty DifficultyLevel;
    }
}

