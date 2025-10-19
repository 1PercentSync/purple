using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombMinigame : MonoBehaviour
{
    [Header("Simon Buttons")]
    public GameObject[] simonButtons;
    public float highlightDuration = 0.5f;
    public float timeBetweenHighlights = 0.3f;

    [Header("Sequence Settings")]
    public int sequenceLength = 5;

    [Header("Fail Settings")]
    public int maxFails = 2; 

    private List<int> sequence;
    private int playerStep = 0;
    private int failCount = 0;
    private bool inputEnabled = true; 

    public EnterBomb enterBomb; 
    public float startDelay = 1f; 

    void Awake()
    {
        for (int i = 0; i < simonButtons.Length; i++)
        {
            SimonButton sb = simonButtons[i].GetComponent<SimonButton>();
            if (sb != null)
            {
                sb.controller = this;
                sb.buttonIndex = i;
            }
        }
    }

    public void StartMinigame()
    {
        GenerateSequence();
        StartCoroutine(PlaySequenceWithDelay(startDelay));
    }

    void GenerateSequence()
    {
        sequence = new List<int>();
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, simonButtons.Length));

        playerStep = 0;
        inputEnabled = false; // disable input while sequence plays
        Debug.Log("Simon sequence: " + string.Join(",", sequence));
    }

    IEnumerator PlaySequenceWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return PlaySequence();
        inputEnabled = true; // enable input after sequence finishes
    }

    IEnumerator PlaySequence()
    {
        inputEnabled = false;

        foreach (int index in sequence)
        {
            if (index < 0 || index >= simonButtons.Length) continue;

            GameObject button = simonButtons[index];
            if (button != null)
            {
                Renderer rend = button.GetComponent<Renderer>();
                SimonButton sb = button.GetComponent<SimonButton>();

                if (sb != null)
                    sb.PlaySound();

                if (rend != null)
                    yield return StartCoroutine(FlashButton(rend, Color.red, highlightDuration));
            }

            yield return new WaitForSeconds(timeBetweenHighlights);
        }

        inputEnabled = true;
    }

    IEnumerator FlashButton(Renderer rend, Color color, float duration)
    {
        Color original = rend.material.color;
        rend.material.color = color;
        yield return new WaitForSeconds(duration);
        rend.material.color = original;
    }

    public void PlayerPress(int buttonIndex)
    {
        if (!inputEnabled) return;

        if (buttonIndex < 0 || buttonIndex >= simonButtons.Length)
        {
            Debug.LogWarning($"Button index {buttonIndex} out of bounds!");
            return;
        }

        if (playerStep >= sequence.Count) return;

        if (sequence[playerStep] == buttonIndex)
        {
            Debug.Log($"Correct button {buttonIndex} pressed!");
            playerStep++;

            SimonButton sb = simonButtons[buttonIndex].GetComponent<SimonButton>();
            Renderer rend = simonButtons[buttonIndex].GetComponent<Renderer>();

            if (sb != null)
                sb.PlaySound();

            if (rend != null)
                StartCoroutine(FlashButton(rend, Color.green, 0.3f));

            if (playerStep >= sequence.Count)
            {
                Debug.Log("Simon sequence completed!");
                playerStep = 0;

                // tell EnterBomb that Simon is done
                if (enterBomb != null)
                    enterBomb.OnSimonComplete();
            }
        }
        else
        {
            Debug.Log("Wrong button! Resetting sequence...");
            failCount++;
            playerStep = 0;
            inputEnabled = false;

            if (failCount >= maxFails)
            {
                Debug.Log("Max fails reached, kicking player out!");
                if (enterBomb != null)
                    enterBomb.ExitBombCamera();
                return;
            }

            StartCoroutine(FlashAllButtons(Color.red, 0.5f));
            StartCoroutine(PlaySequenceWithDelay(1f));
        }
    }

    IEnumerator FlashAllButtons(Color color, float duration)
    {
        List<Renderer> rends = new List<Renderer>();
        foreach (var btn in simonButtons)
        {
            if (btn != null)
            {
                Renderer rend = btn.GetComponent<Renderer>();
                if (rend != null)
                {
                    rends.Add(rend);
                    rend.material.color = color;
                }
            }
        }

        yield return new WaitForSeconds(duration);

        foreach (var rend in rends)
        {
            if (rend != null)
                rend.material.color = Color.white;
        }
    }

    public void ResetMinigame()
    {
        playerStep = 0;
        failCount = 0;
        StartMinigame();
    }
}
