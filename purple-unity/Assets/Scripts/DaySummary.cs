using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DaySummary : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI summaryText;
    public Button continueButton;

    [Header("Content")]
    [TextArea(2, 5)]
    public string titleContent = "END OF DAY 1";

    [TextArea(5, 15)]
    public string summaryContent = "";

    [Header("Settings")]
    public string nextSceneName = "SampleScene";

    private void Start()
    {
        if (titleText != null)
            titleText.text = titleContent;

        if (summaryText != null)
            summaryText.text = summaryContent;

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}