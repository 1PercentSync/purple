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

    [Header("Time Settings")]
    public float timeScale = 80f;         

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
        float realSecond = 1f;

        while (isWorking)
        {
            quotaManager.inGameTime = quotaManager.inGameTime.AddSeconds(timeScale * realSecond);

            quotaManager.UpdateHUD();

            if (ClockTime != null)
                ClockTime.text = quotaManager.inGameTime.ToString("HH:mm:ss");

            if (quotaManager.inGameTime.Hour >= 17)
            {
                EndWorkday();
                yield break;
            }

            yield return new WaitForSeconds(realSecond);
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
