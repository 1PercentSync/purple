using UnityEngine;
using TMPro;
using System;

public class QuotaManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text hudText;          // Single HUD text showing day, time, quota

    [Header("Quota Settings")]
    public int dailyQuota = 5;

    private int remainingTasks = 0;
    private bool quotaActive = false;
    public int currentDay = 1;

    public DateTime inGameTime;        // Updated by WorkClock

    public event Action OnQuotaComplete;

    public void StartQuota()
    {
        remainingTasks = dailyQuota;
        quotaActive = true;
        UpdateHUD();
        Debug.Log($"Day {currentDay} quota started! Daily quota: {dailyQuota}");
    }

    public void CompleteTask()
    {
        if (!quotaActive) return;

        remainingTasks = Mathf.Max(remainingTasks - 1, 0);
        UpdateHUD();

        if (remainingTasks == 0)
        {
            Debug.Log($"Day {currentDay} all tasks completed!");
            EndQuota();
            OnQuotaComplete?.Invoke();
        }
    }

    public void EndQuota()
    {
        quotaActive = false;
        currentDay++; // Next day
        Debug.Log($"Day {currentDay - 1} quota ended!");
    }

    public void UpdateHUD()
    {
        if (hudText != null)
        {
            string timeStr = inGameTime.ToString("HH:mm:ss");
            hudText.text = $"Day {currentDay} | {timeStr}\nDaily Quota {remainingTasks}/{dailyQuota}";
        }
    }
}
