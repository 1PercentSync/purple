using UnityEngine;
using TMPro;
using System;

public class QuotaManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text hudText;       

    [Header("Quota Settings")]
    public int dailyQuota = 5;

    [HideInInspector]
    public int bombsCreated = 0;     
    private bool quotaActive = false;
    public int currentDay = 1;

    public DateTime inGameTime;       
    public event Action OnQuotaComplete;

    void Start()
    {
        if (hudText != null)
            hudText.gameObject.SetActive(true);

        bombsCreated = 0;
    }

    public void StartQuota()
    {
        bombsCreated = 0;
        quotaActive = true;

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
            ShowTherapyReminder();

            OnQuotaComplete?.Invoke();
        }
    }

    public void EndQuota()
    {
        quotaActive = false;
        bombsCreated = 0;

        Debug.Log($"Day {currentDay} quota ended!");
        currentDay++;
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

    private void ShowTherapyReminder()
    {
        Debug.Log("Daily quota completed! Time to head to therapy.");
        if (hudText != null)
        {
            hudText.text += "\n\nDaily quota complete! Head to therapy.";
        }
    }
}
