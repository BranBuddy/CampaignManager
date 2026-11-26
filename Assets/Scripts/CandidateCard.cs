using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace CampaignManager.UI.Card
{
    public class CandidateCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TMP_Text CardNameText;
        public Image CardBackground;
        public Image CardImage;

        public GameObject CardDetailsPanel; 
        public TMP_Text CardRoleText;
        public TMP_Text CardStateText;
        public TMP_Text CardPartyText;
        
        public int CANDIDATE_ID;

        private bool isDetailsPanelVisible = false;
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Mouse entered UI element");
            // Example: Scale up slightly
            StartCoroutine(ChangeScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f, gameObject));
        }

        // Triggered when the mouse cursor leaves the UI element
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Mouse exited UI element");
            // Example: Reset scale
            StartCoroutine(ChangeScale(new Vector3(1f, 1f, 1f), 0.2f, gameObject));
        }

        private IEnumerator ChangeScale(Vector3 targetScale, float duration, GameObject targetObject)
        {
            Vector3 initialScale = targetObject.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                targetObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetObject.transform.localScale = targetScale; // Ensure the final scale is set
        }

        public int ReturnCandidateID()
        {
            return CANDIDATE_ID;
        }

        public void ShowDetailsPage()
        {
            if (isDetailsPanelVisible)
            {
                StartCoroutine(ChangeScale(new Vector3(0f, 0f, 0f), 0.2f, CardDetailsPanel));
                isDetailsPanelVisible = false;
            }
            else
            {
                StartCoroutine(ChangeScale(new Vector3(1f, 1f, 1f), 0.2f, CardDetailsPanel));
                isDetailsPanelVisible = true;
            }
        }

    }
}
