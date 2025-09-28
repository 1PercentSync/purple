using UnityEngine;
using System.Collections;
using System;

public class WorkClock : MonoBehaviour
{
    [Header("World Clock")]
    public TMPro.TMP_Text ClockTime;       // World-space 3D clock
    public GameObject clockCanvas;         // World-space canvas

    [Header("Quota / HUD")]
    public QuotaManager quotaManager;

    private bool isWorking = false;

    void Start()
    {
        if (clockCanvas != null)
            clockCanvas.SetActive(false);
    }

    public void StartWorkday()
    {
        if (clockCanvas != null)
            clockCanvas.SetActive(true);

        if (!isWorking)
        {
            isWorking = true;

            // Start clock at 9 AM
            quotaManager.inGameTime = DateTime.Today.AddHours(9);

            StartCoroutine(UpdateClockRoutine());

            // Start Quota/HUD
            quotaManager.StartQuota();

            Debug.Log("Workday started at 9 AM!");
        }
    }

    private IEnumerator UpdateClockRoutine()
    {
        while (isWorking)
        {
            // Increment in-game time
            quotaManager.inGameTime = quotaManager.inGameTime.AddSeconds(1);

            // Update HUD overlay
            quotaManager.UpdateHUD();

            if (ClockTime != null)
                ClockTime.text = quotaManager.inGameTime.ToString("HH:mm:ss");

            yield return new WaitForSeconds(1f);
        }
    }

    public void EndWorkday()
    {
        if (!isWorking) return;

        isWorking = false;

        if (clockCanvas != null)
            clockCanvas.SetActive(false);

        quotaManager.EndQuota();

        Debug.Log("Workday ended!");
    }
}
