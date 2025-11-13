using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorTransition : MonoBehaviour
{
    [Header("Door Settings")]
    public string sceneToLoad = "TherapyRoom";

    [Header("References")]
    public QuotaManager quotaManager;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (quotaManager == null)
            {
                Debug.LogWarning("QuotaManager not assigned on DoorTransition!");
                return;
            }

            Debug.Log($"Quota completed? {quotaManager.IsQuotaCompleted()}");

            if (quotaManager.IsQuotaCompleted())
            {
                hasTriggered = true;
                StartCoroutine(LoadSceneNextFrame());
            }
            else
            {
                Debug.Log("Complete today's quota first!");
            }
        }
    }

    private IEnumerator LoadSceneNextFrame()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(sceneToLoad);
    }
}
