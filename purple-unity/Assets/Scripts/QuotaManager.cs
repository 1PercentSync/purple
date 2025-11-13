using UnityEngine;
using TMPro;
using System;

public class QuotaManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text hudText;

    [Header("Quota Settings")]
    public int dailyQuota = 3;

    [HideInInspector]
    public int bombsCreated = 0;
    private bool quotaActive = false;
    private bool quotaCompleted = false;

    public int currentDay = 1;
    public DateTime inGameTime;

    public event Action OnQuotaComplete;

    void Start()
    {
        if (hudText != null)
            hudText.gameObject.SetActive(true);

        bombsCreated = 0;
        quotaCompleted = false;
    }

    public void StartQuota()
    {
        bombsCreated = 0;
        quotaActive = true;
        quotaCompleted = false;
        UpdateHUD();
        Debug.Log($"Day {currentDay} quota started! Daily quota: {dailyQuota}");
    }

    public void CompleteTask()
    {
        if (!quotaActive || quotaCompleted) return;

        bombsCreated = Mathf.Min(bombsCreated + 1, dailyQuota);
        UpdateHUD();

        if (bombsCreated >= dailyQuota)
        {
            Debug.Log($"Day {currentDay} all tasks completed!");
            quotaCompleted = true;
            EndQuota();
            ShowTherapyReminder();
            OnQuotaComplete?.Invoke();
        }
    }

    public void EndQuota()
    {
        quotaActive = false;
        Debug.Log($"Day {currentDay} quota ended!");
    }

    public void StartNextDay()
    {
        currentDay++;
        bombsCreated = 0;
        quotaCompleted = false;
        quotaActive = false;
        UpdateHUD();
        Debug.Log($"Starting Day {currentDay}");
    }

    public void UpdateHUD()
    {
        if (hudText != null)
        {
            string timeStr = inGameTime != DateTime.MinValue ? inGameTime.ToString("HH:mm:ss") : "00:00:00";
            string quotaStr;

            if (quotaCompleted)
            {
                quotaStr = "Quota Complete - Visit Therapy";
            }
            else if (quotaActive)
            {
                quotaStr = $"Daily Quota {bombsCreated}/{dailyQuota}";
            }
            else
            {
                quotaStr = "Quota inactive";
            }

            hudText.text = $"Day {currentDay} | {timeStr}\n{quotaStr}";
        }
    }

    public bool IsQuotaActive()
    {
        return quotaActive;
    }

    public bool IsQuotaCompleted()
    {
        return quotaCompleted;
    }

    public bool CanAccessBomb()
    {
        return quotaActive && !quotaCompleted;
    }

    public int GetRemainingQuota()
    {
        return dailyQuota - bombsCreated;
    }

    private void ShowTherapyReminder()
    {
        Debug.Log("Daily quota completed! Time to head to therapy.");
        if (hudText != null)
        {
            UpdateHUD();
        }
    }
}
