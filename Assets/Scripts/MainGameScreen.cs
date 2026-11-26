using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CampaignManager.Database;

namespace CampaignManager.UI.MainGame
{
    public class MainGameScreen : MonoBehaviour
    {
        public GameObject ChosenCandidateCard;
        public Image GameOverlay;

        private void OnEnable()
        {
            GameManager.GameManager.StartGameEvent += OnStartGame;
        }

        private void OnDisable()
        {
            GameManager.GameManager.StartGameEvent -= OnStartGame;
        }

        private void OnStartGame()
        {
            Debug.Log("StartGameEvent triggered in MainGameScreen.");
            PopulateChoseCandidateCard(GameManager.GameManager.Instance.ChosenCandidateID);
            StartCoroutine(FadeOutOverlay(1.0f)); // Fade out over 1 second
        }

        private void PopulateChoseCandidateCard(int candidateID)
        {
            // Fetch the candidate data from the database using the candidateID
            var candidateData = DatabaseManager.Instance.candidateDB.GetData<CandidateDBColumns>(candidateID.ToString());
            var partyData = DatabaseManager.Instance.partyDB.GetData<PartyDBColumns>(candidateData.PARTY_ID.ToString());
            var stateData = DatabaseManager.Instance.stateDB.GetData<StateDBColumns>(candidateData.STATE_ABBR.ToString());
            var roleData = DatabaseManager.Instance.roleDB.GetData<RoleDBColumns>(candidateData.ROLE_ID.ToString());

            if (candidateData != null)
            {
                // Assuming ChosenCandidateCard has a script to set its UI elements
                var cardScript = ChosenCandidateCard.GetComponent<UI.Card.CandidateCard>();
                if (cardScript != null)
                {
                    cardScript.CardNameText.text = candidateData.FIRST_NAME + " " + candidateData.LAST_NAME;
                    cardScript.CardImage.sprite = candidateData.CANDIDATE_IMAGE.sprite;
                    cardScript.CardPartyText.text = partyData.PARTY_NAME;
                    cardScript.CardStateText.text = stateData.STATE_NAME;
                    cardScript.CardRoleText.text = roleData.ROLE_NAME;
                    cardScript.CardBackground.color = partyData.PARTY_COLOR;
                    cardScript.CANDIDATE_ID = candidateData.CANDIDATE_ID;
                }
                else
                {
                    Debug.LogError("CandidateCard script not found on ChosenCandidateCard.");
                }
            }
            else
            {
                Debug.LogError($"No candidate data found for ID: {candidateID}");
            }
        }

        private IEnumerator FadeOutOverlay(float duration)
        {
            float elapsedTime = 0f;
            GameOverlay.gameObject.SetActive(true); // Ensure the overlay is active before starting the fade
            Color initialColor = GameOverlay.color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

            while (elapsedTime < duration)
            {
                GameOverlay.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GameOverlay.color = targetColor; // Ensure the final color is set
            GameOverlay.gameObject.SetActive(false); // Optionally disable the overlay after fading out
        }
    }
}
