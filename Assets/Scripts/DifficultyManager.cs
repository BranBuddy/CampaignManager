using UnityEngine;
using CampaignManager.Singleton;
using CampaignManager.GameManager;

namespace CampaignManager.DifficultyManager
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        public const float EasyMultiplier = 0.5f;
        public const float MediumMultiplier = 1.0f;
        public const float HardMultiplier = 1.5f;

        protected override void Awake()
        {
            base.Awake();
        }    

        public void SetDifficulty(int difficulty)
        {
            Difficulty selectedDifficulty = (Difficulty)difficulty;

            GameManager.GameManager.Instance.DifficultyLevel = selectedDifficulty;
        }
    }

    [System.Serializable]
    public enum Difficulty
        {
            None = 0,
            Easy = 1,
            Medium = 2,
            Hard = 3
        }
}
