using UnityEngine;
using CampaignManager.Database;
using CampaignManager.DifficultyManager;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace CampaignManager.CandidatePicker
{
    public class CandidatePickerScreen : MonoBehaviour
    {
        private Difficulty GameDifficulty;

        [SerializeField] private GameObject candidateCardContainer;
        [SerializeField] private GameObject candidateCardPrefab;



        private Database.DatabaseManager dataBaseManager;    

        private List<GameObject> candidateCards = new List<GameObject>();

        private void Start()
        {
            GameDifficulty = GameManager.GameManager.Instance.DifficultyLevel;
            dataBaseManager = Database.DatabaseManager.Instance;
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
            
            List<Database.CandidateDBColumns> candidateEntries = new List<Database.CandidateDBColumns>(DatabaseManager.Instance.candidateDB.entries);
            
            print($"Number of candidate entries available: {candidateEntries.Count}");

            List<int> usedIndices = new List<int>();

            for (int i = 0; i < numberOfCards; i++)
            {
                int chosenCandidate = Random.Range(0, candidateEntries.Count);

                if (usedIndices.Contains(chosenCandidate))
                {
                    // If the chosen candidate has already been used, decrement i to retry this iteration
                    i--;
                    continue;
                }

                GameObject card = candidateCards[i];
                UI.Card.CandidateCard cardScript = card.GetComponent<UI.Card.CandidateCard>();

               print($"Chosen candidate index: {chosenCandidate}, CANDIDATE_ID: {candidateEntries[chosenCandidate].CANDIDATE_ID}");

                if (cardScript != null)
                {
                    var candidateData = DatabaseManager.Instance.candidateDB.GetData<CandidateDBColumns>(chosenCandidate.ToString());
                    var partyData = DatabaseManager.Instance.partyDB.GetData<PartyDBColumns>(candidateData.PARTY_ID.ToString());
                    var stateData = DatabaseManager.Instance.stateDB.GetData<StateDBColumns>(candidateData.STATE_ABBR.ToString());
                    var roleData = DatabaseManager.Instance.roleDB.GetData<RoleDBColumns>(candidateData.ROLE_ID.ToString());
                    
                    if (candidateData != null)
                    {
                        cardScript.CANDIDATE_ID = candidateData.CANDIDATE_ID;
                        cardScript.CardNameText.text = $"{candidateData.FIRST_NAME} {candidateData.LAST_NAME}";
                        ChangeFontSizeBasedOnLength(cardScript.CardNameText, 13, 20, 18);
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

                    if (roleData != null)
                    {
                        cardScript.CardRoleText.text = roleData.ROLE_NAME;
                    }
                    else
                    {
                        Debug.LogWarning($"No role data found for ROLE_ID: {candidateData.ROLE_ID}");
                    }
                    usedIndices.Add(chosenCandidate);

                }

                
            }

        }

        

        public void StartGame(GameObject card)
        {
            UI.Card.CandidateCard selectedCard = card.GetComponent<UI.Card.CandidateCard>();
            if (selectedCard != null)
            {
                GameManager.GameManager.Instance.ChosenCandidateID = selectedCard.ReturnCandidateID();
                Debug.Log($"Chosen Candidate ID: {GameManager.GameManager.Instance.ChosenCandidateID}");
            }
            else
            {
                Debug.LogError("Failed to get CandidateCard component");
            }

            DeleteCandidateCards();

            GameManager.GameManager.Instance.StartGame();
        }

        private void ChangeFontSizeBasedOnLength(TMP_Text textComponent, int maxLength, int defaultFontSize, int reducedFontSize)
        {
            if (textComponent.text.Length > maxLength)
            {
                textComponent.fontSize = reducedFontSize;
            }
            else
            {
                textComponent.fontSize = defaultFontSize;
            }
        }

        private void DeleteCandidateCards()
        {
            foreach (var card in candidateCards)
            {
                Destroy(card);
            }
            candidateCards.Clear();
        }

        private int CreateCandidateCards(int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                GameObject card = Instantiate(candidateCardPrefab, candidateCardContainer.transform);
                card.name = $"CandidateCard_{i + 1}";
                Button cardButton = card.GetComponent<Button>();
                if (cardButton != null)
                {
                    cardButton.onClick.AddListener(() =>
                    {
                        StartGame(card);
                    });
                }
                else
                {
                    Debug.LogWarning($"No Button component found on {card.name}");
                }
                candidateCards.Add(card);
            }
            return numberOfCards;
        }
    }
}
