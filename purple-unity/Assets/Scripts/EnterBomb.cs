using UnityEngine;
using System.Collections;

public class EnterBomb : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public Camera bombCamera;
    public PlayerMovement playerMovement;
    public QuotaManager quotaManager;            // QuotaManager
    public BombMinigame simonController;         // Simon minigame
    public NumpadMinigame numpadController;      // Numpad minigame 

    private bool inMinigame = false;
    private bool playerInRange = false;

    private BombCameraLook bombLookScript;
    private Quaternion bombDefaultRotation;

    void Start()
    {
        if (bombCamera != null)
        {
            bombDefaultRotation = bombCamera.transform.rotation;
            bombCamera.gameObject.SetActive(false);

            bombLookScript = bombCamera.GetComponent<BombCameraLook>();
            if (bombLookScript != null)
                bombLookScript.enabled = false;
        }

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (quotaManager == null || !quotaManager.IsQuotaActive())
            {
                Debug.Log("You need to clock in before creating bombs!");
                return;
            }

            if (quotaManager.IsQuotaCompleted())
            {
                Debug.Log("Daily quota completed! Please visit therapy before working on more bombs.");
                return;
            }

            if (quotaManager.GetRemainingQuota() <= 0)
            {
                Debug.Log("Daily quota completed! Cannot create more bombs today.");
                return;
            }

            if (!inMinigame)
                EnterMinigame();
            else
                ExitMinigame();
        }
    }

    void EnterMinigame()
    {
        inMinigame = true;

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (bombCamera != null)
        {
            bombCamera.gameObject.SetActive(true);
            bombCamera.transform.rotation = bombDefaultRotation;
            if (bombLookScript != null)
                bombLookScript.enabled = true;
        }

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Entered Bomb Minigame");

        // Reset numpad to ensure it's cleared and inactive
        if (numpadController != null)
            numpadController.ResetMinigame();

        // Start Simon Says minigame after 3 seconds
        if (simonController != null)
            StartCoroutine(StartSimonWithDelay());
    }

    IEnumerator StartSimonWithDelay()
    {
        yield return new WaitForSeconds(3f);
        simonController.StartMinigame();
    }

    void ExitMinigame()
    {
        inMinigame = false;

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        if (bombCamera != null)
        {
            bombCamera.gameObject.SetActive(false);
            if (bombLookScript != null)
                bombLookScript.enabled = false;
        }

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Exited Bomb Minigame");

        if (simonController != null)
            simonController.ResetMinigame();
        if (numpadController != null)
            numpadController.ResetMinigame();
    }

    // Called by SimonController when Simon Says is completed
    public void OnSimonComplete()
    {
        Debug.Log("Simon Says complete! Starting Numpad in 5 seconds...");
        StartCoroutine(StartNumpadAfterDelay());
    }

    IEnumerator StartNumpadAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        if (numpadController != null)
            numpadController.StartMinigame();
    }

    // Called by NumpadController when Numpad sequence is completed
    public void OnNumpadComplete()
    {
        if (quotaManager != null)
        {
            quotaManager.CompleteTask();       // Count 1 bomb
            Debug.Log("Bomb completed! Remaining quota: " + quotaManager.GetRemainingQuota());

            // Check if quota is now complete
            if (quotaManager.IsQuotaCompleted())
            {
                Debug.Log("Daily quota finished! Exiting to main camera. Head to therapy!");
                ExitMinigame();             
            }
            else if (quotaManager.GetRemainingQuota() > 0)
            {
                StartCoroutine(ResetMinigamesWithDelay());
            }
        }
    }

    IEnumerator ResetMinigamesWithDelay()
    {
        yield return new WaitForSeconds(3f);

        if (numpadController != null)
            numpadController.ResetMinigame();

        if (simonController != null)
            simonController.ResetMinigame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    // Called by either minigame after too many failures
    public void ExitBombCamera()
    {
        Debug.Log("Too many failures! Exiting Bomb Minigame.");
        ExitMinigame();
    }

    // Call this after therapy is complete to unlock bomb access for next day
    public void OnTherapyComplete()
    {
        if (quotaManager != null)
        {
            quotaManager.StartNextDay();
            quotaManager.StartQuota();
            Debug.Log("Therapy complete! New day started. You can now work on bombs again.");
        }
    }
}