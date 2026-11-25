using UnityEngine;
using CampaignManager.GameManager;
using CampaignManager.DifficultyManager;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace CampaignManager.SetUpGame
{
    public class SetUpGame : MonoBehaviour
    {
        private Difficulty GameDifficulty;

        [SerializeField] private GameObject candidateCardContainer;
        [SerializeField] private GameObject candidateCardPrefab;



        private Database.DatabaseManager dataBaseManager;    
        private Database.CandidateDB candidateDB;
        private Database.PartyDB partyDB;
        private Database.StateDB stateDB;
    
        private List<GameObject> candidateCards = new List<GameObject>();

        private void Start()
        {
            GameDifficulty = GameManager.GameManager.Instance.DifficultyLevel;
            dataBaseManager = Database.DatabaseManager.Instance;
            candidateDB = dataBaseManager.FetchDatabase("Candidate").GetComponent<Database.CandidateDB>();
            partyDB = dataBaseManager.FetchDatabase("Party").GetComponent<Database.PartyDB>();
            stateDB = dataBaseManager.FetchDatabase("State").GetComponent<Database.StateDB>();
            SetUpCandidateCards(GameDifficulty);
        }

        private void SetUpCandidateCards(Difficulty difficulty)
        {
            int numberOfCards = 0;

            if (candidateCardContainer == null || candidateCardPrefab == null)
            {
                Debug.LogError("Candidate card container or prefab is not assigned.");
                return;
            }

            if (candidateCardContainer.GetComponent<HorizontalLayoutGroup>() == null)
            {
                candidateCardContainer.AddComponent<HorizontalLayoutGroup>();
            }

            switch (difficulty)
            {
                case Difficulty.Easy:
                    numberOfCards = CreateCandidateCards(Random.Range(5, 8)); // Randomly create between 5 to 7 cards for Easy difficulty
                    break;
                case Difficulty.Medium:
                    numberOfCards = CreateCandidateCards(Random.Range(3, 6)); // Randomly create between 3 to 5 cards for Medium difficulty
                    break;
                case Difficulty.Hard:
                    numberOfCards = CreateCandidateCards(Random.Range(1, 4)); // Randomly create between 1 to 3 cards for Hard difficulty
                    break;
                default:
                    Debug.LogError("Invalid difficulty level.");
                    break;
            }

            AdjustSpacingAndSizeBasedOnNumberOfCards(numberOfCards);
            DesignCandidateCards(numberOfCards);

        }

        private void AdjustSpacingAndSizeBasedOnNumberOfCards(int numberOfCards)
        {
            HorizontalLayoutGroup layoutGroup = candidateCardContainer.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                float spacing = Mathf.Clamp(10f - (numberOfCards * 1.5f), 2f, 10f); // Adjust spacing based on number of cards
                layoutGroup.spacing = spacing;
            }

            // Adjust the size of the candidate card prefab based on the number of cards
            foreach (GameObject card in candidateCards)
            {
                RectTransform cardTransform = card.GetComponent<RectTransform>();
                if (cardTransform != null)
                {
                    float width = Mathf.Clamp(200f - (numberOfCards * 10f), 100f, 200f); // Adjust width based on number of cards
                    float height = Mathf.Clamp(300f - (numberOfCards * 15f), 150f, 300f); // Adjust height based on number of cards
                    cardTransform.sizeDelta = new Vector2(width, height);
                }
            }
        }

        private void DesignCandidateCards(int numberOfCards)
        {
            
            List<Database.CandidateDBColumns> candidateEntries = new List<Database.CandidateDBColumns>(candidateDB.entries);
            
            for (int i = 0; i < numberOfCards; i++)
            {
                int chosenCandidate = Random.Range(0, candidateEntries.Count);

                GameObject card = candidateCards[i];
                UI.Card.CandidateCard cardScript = card.GetComponent<UI.Card.CandidateCard>();

               

                if (cardScript != null)
                {
                    var candidateData = candidateDB.GetData<Database.CandidateDBColumns>(chosenCandidate.ToString());
                    var partyData = partyDB.GetData<Database.PartyDBColumns>(candidateData.PARTY_ID.ToString());
                    var stateData = stateDB.GetData<Database.StateDBColumns>(candidateData.STATE_ABBR.ToString());
                    
                    if (candidateData != null)
                    {
                        cardScript.CardNameText.text = $"{candidateData.FIRST_NAME} {candidateData.LAST_NAME}";
                        cardScript.CardImage.sprite = candidateData.CANDIDATE_IMAGE.sprite;
                    }
                    else
                    {
                        Debug.LogWarning($"No candidate data found for CANDIDATE_ID: {candidateData.CANDIDATE_ID}");
                    }

                    if (partyData != null)
                    {
                        cardScript.CardPartyText.text = partyData.PARTY_NAME;
                        cardScript.CardBackground.color = partyData.PARTY_COLOR;
                    }
                    else
                    {
                        Debug.LogWarning($"No party data found for PARTY_ID: {candidateData.PARTY_ID}");
                    }

                    if (stateData != null)
                    {
                        cardScript.CardStateText.text = stateData.STATE_NAME;
                    }
                    else
                    {
                        Debug.LogWarning($"No state data found for STATE_ABBR: {candidateData.STATE_ABBR}");
                    }

                    // Remove the chosen candidate to avoid duplicates
                    candidateEntries.Remove(candidateEntries[chosenCandidate]);
                }

                
            }

        }
        private int CreateCandidateCards(int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                GameObject card = Instantiate(candidateCardPrefab, candidateCardContainer.transform);
                card.name = $"CandidateCard_{i + 1}";
                candidateCards.Add(card);
            }
            return numberOfCards;
        }
    }
}
