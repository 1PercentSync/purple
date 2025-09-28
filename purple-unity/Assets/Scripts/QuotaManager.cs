using UnityEngine;
using TMPro;
using System;

public class QuotaManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text hudText;          // Single HUD text showing day, time, quota

    [Header("Quota Settings")]
    public int dailyQuota = 5;

    [HideInInspector]
    public int bombsCreated = 0;      // Number of bombs created today
    private bool quotaActive = false;
    public int currentDay = 1;

    public DateTime inGameTime;        // Updated externally by WorkClock
    public event Action OnQuotaComplete;

    void Start()
    {
        // Start HUD visible right away so the clock is always showing
        if (hudText != null)
            hudText.gameObject.SetActive(true);

        bombsCreated = 0;
    }

    public void StartQuota()
    {
        bombsCreated = 0;
        quotaActive = true;

        // ? Removed forced time reset here — let WorkClock update it
        UpdateHUD();

        Debug.Log($"Day {currentDay} quota started! Daily quota: {dailyQuota}");
    }

    public void CompleteTask()
    {
        if (!quotaActive) return;

        bombsCreated = Mathf.Min(bombsCreated + 1, dailyQuota);
        UpdateHUD();

        if (bombsCreated >= dailyQuota)
        {
            Debug.Log($"Day {currentDay} all tasks completed!");
            EndQuota();
            OnQuotaComplete?.Invoke();
        }
    }

    public void EndQuota()
    {
        quotaActive = false;

        currentDay++;
        bombsCreated = 0;

        Debug.Log($"Day {currentDay - 1} quota ended!");
    }

    public void UpdateHUD()
    {
        if (hudText != null)
        {
            string timeStr = inGameTime != DateTime.MinValue ? inGameTime.ToString("HH:mm:ss") : "00:00:00";

            string quotaStr = quotaActive
                ? $"Daily Quota {bombsCreated}/{dailyQuota}"
                : "Quota inactive";

            hudText.text = $"Day {currentDay} | {timeStr}\n{quotaStr}";
        }
    }

    public bool IsQuotaActive()
    {
        return quotaActive;
    }

    public int GetRemainingQuota()
    {
        return dailyQuota - bombsCreated;
    }
}
