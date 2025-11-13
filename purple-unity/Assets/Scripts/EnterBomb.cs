using UnityEngine;
using System.Collections;

public class EnterBomb : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public Camera bombCamera;
    public PlayerMovement playerMovement;
    public QuotaManager quotaManager;
    public BombMinigame simonController;
    public NumpadMinigame numpadController;

    private bool inMinigame = false;
    private bool playerInRange = false;

    private BombCameraLook bombLookScript;
    private Quaternion bombDefaultRotation;

    private Coroutine simonDelayCoroutine;
    private Coroutine numpadDelayCoroutine;
    private Coroutine resetMinigamesCoroutine;

    void Start()
    {
        if (bombCamera != null)
        {
            bombDefaultRotation = bombCamera.transform.rotation;
            bombCamera.gameObject.SetActive(false);
            bombLookScript = bombCamera.GetComponent<BombCameraLook>();
            if (bombLookScript != null) bombLookScript.enabled = false;
        }

        mainCamera?.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (quotaManager == null || !quotaManager.IsQuotaActive())
            {
                Debug.Log("Clock in first!");
                return;
            }
            if (quotaManager.IsQuotaCompleted() || quotaManager.GetRemainingQuota() <= 0)
            {
                Debug.Log("Daily quota done!");
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
        mainCamera?.gameObject.SetActive(false);
        if (bombCamera != null)
        {
            bombCamera.gameObject.SetActive(true);
            bombCamera.transform.rotation = bombDefaultRotation;
            if (bombLookScript != null) bombLookScript.enabled = true;
        }

        if (playerMovement != null) playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Entered Bomb Minigame");

        simonDelayCoroutine = StartCoroutine(StartSimonWithDelay());
    }

    IEnumerator StartSimonWithDelay()
    {
        yield return new WaitForSeconds(1f);
        if (!inMinigame) yield break;
        simonController?.StartMinigame();
    }

    void ExitMinigame()
    {
        inMinigame = false;
        mainCamera?.gameObject.SetActive(true);
        if (bombCamera != null)
        {
            bombCamera.gameObject.SetActive(false);
            if (bombLookScript != null) bombLookScript.enabled = false;
        }

        if (playerMovement != null) playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Exited Bomb Minigame");

        StopAllDelayedCoroutines();
    }

    public void OnSimonComplete()
    {
        Debug.Log("Simon complete! Starting Numpad in 1 second...");
        numpadDelayCoroutine = StartCoroutine(StartNumpadAfterDelay());
    }

    IEnumerator StartNumpadAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        if (!inMinigame) yield break;
        numpadController?.StartMinigame();
    }

    public void OnNumpadComplete()
    {
        if (quotaManager != null)
        {
            quotaManager.CompleteTask();
            Debug.Log("Bomb finished! Remaining quota: " + quotaManager.GetRemainingQuota());

            if (!quotaManager.IsQuotaCompleted())
            {
                // restart the process
                resetMinigamesCoroutine = StartCoroutine(RestartMinigamesAfterDelay(1f));
            }
            else
            {
                ExitMinigame();
            }
        }
    }

    IEnumerator RestartMinigamesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!inMinigame) yield break;
        simonController?.ResetMinigame();
        numpadController?.ResetMinigame(false); // autoStart false
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }

    public void ExitBombCamera()
    {
        Debug.Log("Too many failures! Exiting Bomb Minigame.");
        StopAllDelayedCoroutines();
        simonController?.ResetMinigame(false);
        numpadController?.ResetMinigame(false);
        ExitMinigame();
    }

    private void StopAllDelayedCoroutines()
    {
        simonDelayCoroutine = StopCoroutineSafe(simonDelayCoroutine);
        numpadDelayCoroutine = StopCoroutineSafe(numpadDelayCoroutine);
        resetMinigamesCoroutine = StopCoroutineSafe(resetMinigamesCoroutine);
    }

    private Coroutine StopCoroutineSafe(Coroutine coroutine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        return null;
    }
}
