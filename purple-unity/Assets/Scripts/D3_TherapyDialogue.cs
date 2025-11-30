using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class D3_TherapyDialogue : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public GameObject dialoguePanel;
    public Button option1Button;
    public Button option2Button;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;

    private int currentQuestionIndex = 0;
    private List<D3_TherapyQuestion> questions = new List<D3_TherapyQuestion>();

    [System.Serializable]
    public class D3_TherapyQuestion
    {
        public string question;
        public string option1;
        public string option2;

        public D3_TherapyQuestion(string q, string opt1, string opt2)
        {
            question = q;
            option1 = opt1;
            option2 = opt2;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        option1Button.onClick.AddListener(() => OnOptionSelected(1));
        option2Button.onClick.AddListener(() => OnOptionSelected(2));

        SetupQuestions();

        Invoke("StartTherapySession", 1f);
    }

    private void SetupQuestions()
    {
        questions.Add(new D3_TherapyQuestion(
            "Welcome back. Since our last session, have things improved or worsened?",
            "Worsened. Every day feels heavier than the last.",
            "Improved? No… it’s the same. Just the same."
        ));

        questions.Add(new D3_TherapyQuestion(
            "When you think about going to work now, what comes to mind?",
            "Dread. The machines, the deadlines, the pressure… it never stops.",
            "Honestly, nothing. I don't think much anymore. I just do."
        ));

        questions.Add(new D3_TherapyQuestion(
            "Your performance today seems to have worsened considerably. Have you taken any actions for self care lately?",
            "No. I don’t have the energy or motivation.",
            "I try to, but nothing seems to make a difference."
        ));

        questions.Add(new D3_TherapyQuestion(
            "Do you feel safe around your coworkers and workplace environment?",
            "Of course not. Everyone is stressed, tense, on edge.",
            "Safe? No. It feels like something could go wrong at any time."
        ));

        questions.Add(new D3_TherapyQuestion(
            "Thank you for being honest. One final question — what do you feel you need right now?",
            "For my hands to stop shaking.",
            "Someone to actually hear me, not just record my answers."
        ));
    }

    public void StartTherapySession()
    {
        dialoguePanel.SetActive(true);
        currentQuestionIndex = 0;
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            EndTherapySession();
            return;
        }

        D3_TherapyQuestion currentQuestion = questions[currentQuestionIndex];

        questionText.text = currentQuestion.question;
        option1Text.text = currentQuestion.option1;
        option2Text.text = currentQuestion.option2;
    }

    private void OnOptionSelected(int optionNumber)
    {
        Debug.Log($"[Day 3] Question {currentQuestionIndex + 1}: Selected Option {optionNumber}");
        currentQuestionIndex++;
        ShowQuestion();
    }

    private void EndTherapySession()
    {
        questionText.text = "Thank you. Your responses have been submitted.";
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        StartCoroutine(FinishSession());
    }

    private IEnumerator FinishSession()
    {
        yield return new WaitForSeconds(5f);

        dialoguePanel.SetActive(false);
        Debug.Log("Day 3 therapy session finished.");

        UnityEngine.SceneManagement.SceneManager.LoadScene("DaySummaryDay3");
    }
}
