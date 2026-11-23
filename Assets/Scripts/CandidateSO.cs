using UnityEngine;

namespace CampaignManager.CandidateSO
{
    [CreateAssetMenu(fileName = "NewCandidate", menuName = "Candidate")]
    public class CandidateSO : ScriptableObject
    {
        [Header("Candidate Info")]
        public string candidateName;
        public string candidateID;
        public Sprite candidateImage;

        [Header("Candidate Stats")]
        public int charisma;
        public int intelligence;
        public int experience;
    }
}
