using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TherapyDialogue : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public GameObject dialoguePanel;
    public Button option1Button;
    public Button option2Button;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;

    private int currentQuestionIndex = 0;
    private List<TherapyQuestion> questions = new List<TherapyQuestion>();

    [System.Serializable]
    public class TherapyQuestion
    {
        public string question;
        public string option1;
        public string option2;

        public TherapyQuestion(string q, string opt1, string opt2)
        {
            question = q;
            option1 = opt1;
            option2 = opt2;
        }
    }

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        option1Button.onClick.AddListener(() => OnOptionSelected(1));
        option2Button.onClick.AddListener(() => OnOptionSelected(2));

        SetupQuestions();

        Invoke("StartTherapySession", 1f);
    }

    private void SetupQuestions()
    {
        questions.Add(new TherapyQuestion(
            "Welcome to mandatory wellness check. How are you feeling today?",
            "Honestly, I hate my job and my life.",
            "Same as always… numb and tired."
        ));

        questions.Add(new TherapyQuestion(
            "I see. Can you describe why you feel this way?",
            "Because I spend my days assembling bombs for a company that doesn't care.",
            "Everything feels pointless. I just exist to follow orders."
        ));

        questions.Add(new TherapyQuestion(
            "And your relationships with coworkers and friends? Are they satisfactory?",
            "I avoid them. They’re as dead inside as this place.",
            "No one cares, and I don’t either."
        ));

        questions.Add(new TherapyQuestion(
            "Understood. What activities outside of work bring you any sense of happiness?",
            "Nothing. I go home, sleep, repeat.",
            "I can’t remember a time I actually felt joy."
        ));

        questions.Add(new TherapyQuestion(
            "One last question. What would help you feel safer at work?",
            "Knowing that a mistake won’t get me killed.",
            "Somehow, having less fear of the machines around me."
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

        TherapyQuestion currentQuestion = questions[currentQuestionIndex];

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
        yield return new WaitForSeconds(3f);

        dialoguePanel.SetActive(false);
        Debug.Log("Therapy session complete");
        OnSessionComplete();
    }

    private void OnSessionComplete()
    {

        // This is where the next day UI overlay will happen
    }
}