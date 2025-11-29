using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class D2_TherapyDialogue : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public GameObject dialoguePanel;
    public Button option1Button;
    public Button option2Button;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;

    private int currentQuestionIndex = 0;
    private List<D2_TherapyQuestion> questions = new List<D2_TherapyQuestion>();

    [System.Serializable]
    public class D2_TherapyQuestion
    {
        public string question;
        public string option1;
        public string option2;

        public D2_TherapyQuestion(string q, string opt1, string opt2)
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
        questions.Add(new D2_TherapyQuestion(
            "Hello, [REDACTED]. How was your shift today?",
            "Not as bad, I think.",
            "Same as yesterday."
        ));

        questions.Add(new D2_TherapyQuestion(
            "That's wonderful! It looks like your BPH (bombs per hour) ratio has widened. Tell me, have you been experiencing any fatigue lately?",
            "Yeah, I barely have time to do anything at home other than sleep, when I can.",
            "These extra unpaid therapy sessions only take more time from my day."
        ));

        questions.Add(new D2_TherapyQuestion(
            "Hmm...have you considered taking some sort of sleeping aid?",
            "You don't pay me enough for more than just bills and food.",
            "No matter how exhausted I am I just can't sleep."
        ));

        questions.Add(new D2_TherapyQuestion(
            "I see, that's rather unfortunate. Perhaps a good Sunday off will bring back your energy!",
            "I work Sundays.",
            "You make me work Sundays."
        ));

        questions.Add(new D2_TherapyQuestion(
            "....",
            "....",
            "No chipper response to that one?"
        ));
        
        questions.Add(new D2_TherapyQuestion(
            "I'm sorry it seems we're all out of time. How unfortunate.",
            "Bastards.",
            "That sounds about right."
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

        D2_TherapyQuestion currentQuestion = questions[currentQuestionIndex];

        questionText.text = currentQuestion.question;
        option1Text.text = currentQuestion.option1;
        option2Text.text = currentQuestion.option2;
    }

    private void OnOptionSelected(int optionNumber)
    {
        Debug.Log($"Question {currentQuestionIndex + 1}: Selected Option {optionNumber}");

        currentQuestionIndex++;
        ShowQuestion();
    }

    private void EndTherapySession()
    {
        questionText.text = "Thank you for opening up today. Our session is complete.";
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        StartCoroutine(FinishSession());
    }

    private IEnumerator FinishSession()
    {
        yield return new WaitForSeconds(5f);

        dialoguePanel.SetActive(false);
        Debug.Log("Therapy session complete. Loading Day Summary...");

        UnityEngine.SceneManagement.SceneManager.LoadScene("DaySummary");
    }
}
