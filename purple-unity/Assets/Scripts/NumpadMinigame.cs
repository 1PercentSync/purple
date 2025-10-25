using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NumpadMinigame : MonoBehaviour
{
    [Header("Numpad Buttons")]
    public GameObject[] numpadButtons;

    [Header("UI")]
    public TMP_Text screenText;  // Assign your screen TMP_Text here

    public float codeDisplayTime = 3f; // Time to show code
    public int codeLength = 4;
    public int maxFails = 2;

    private List<int> codeSequence;
    private int playerStep = 0;
    private int failCount = 0;
    private bool inputEnabled = false;
    private bool isActive = false;

    public EnterBomb enterBomb;

    void Awake()
    {
        for (int i = 0; i < numpadButtons.Length; i++)
        {
            NumpadButton nb = numpadButtons[i].GetComponent<NumpadButton>();
            if (nb != null)
            {
                nb.controller = this;
                nb.number = i + 1; // 1-9
            }

            Collider col = numpadButtons[i].GetComponent<Collider>();
            if (col != null) col.enabled = false; // Disable buttons initially
        }

        ClearScreen();
    }

    public void StartMinigame()
    {
        isActive = true;
        GenerateCode();
        playerStep = 0;
        failCount = 0;
        inputEnabled = false;

        EnableAllButtons();

        // Start coroutine to show code then switch to input
        StartCoroutine(ShowCodeThenStartInput());
    }

    void GenerateCode()
    {
        codeSequence = new List<int>();
        for (int i = 0; i < codeLength; i++)
            codeSequence.Add(Random.Range(1, numpadButtons.Length + 1)); // 1-9
    }

    IEnumerator ShowCodeThenStartInput()
    {
        // Show full code
        if (screenText != null)
        {
            screenText.color = Color.yellow;
            screenText.text = string.Join(" ", codeSequence);
        }

        yield return new WaitForSeconds(codeDisplayTime);

        // Clear screen for input
        playerStep = 0;
        inputEnabled = true;
        UpdateScreen();
    }

    public void PressNumber(int number)
    {
        if (!inputEnabled || !isActive) return; 

        if (playerStep >= codeSequence.Count) return;

        if (codeSequence[playerStep] == number)
        {
            playerStep++;
            UpdateScreen();

            if (playerStep >= codeSequence.Count)
            {
                inputEnabled = false;
                isActive = false; 
                DisableAllButtons();
                enterBomb?.OnNumpadComplete();
            }
        }
        else
        {
            failCount++;
            playerStep = 0;
            UpdateScreen(true); // flash red for wrong input

            if (failCount >= maxFails)
            {
                inputEnabled = false;
                isActive = false; 
                DisableAllButtons();
                enterBomb?.ExitBombCamera();
            }
        }
    }

    void UpdateScreen(bool wrong = false)
    {
        if (screenText == null) return;

        if (wrong)
        {
            screenText.color = Color.red;
            screenText.text = "WRONG!";
            StartCoroutine(ResetScreenColor());
        }
        else
        {
            screenText.color = Color.green;
            string display = "";
            for (int i = 0; i < codeLength; i++)
            {
                if (i < playerStep)
                    display += codeSequence[i] + " "; // show entered number
                else
                    display += "_ "; // placeholder
            }
            screenText.text = display.TrimEnd();
        }
    }

    IEnumerator ResetScreenColor()
    {
        yield return new WaitForSeconds(0.5f);
        screenText.color = Color.white;
        UpdateScreen();
    }

    void EnableAllButtons()
    {
        foreach (var btn in numpadButtons)
        {
            var col = btn.GetComponent<Collider>();
            if (col != null) col.enabled = true;
        }
    }

    void DisableAllButtons()
    {
        foreach (var btn in numpadButtons)
        {
            var col = btn.GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    void ClearScreen()
    {
        if (screenText != null)
        {
            screenText.text = "";
            screenText.color = Color.white;
        }
    }

    public void ResetMinigame()
    {
        isActive = false; 
        playerStep = 0;
        failCount = 0;
        DisableAllButtons(); 
        ClearScreen(); 
    }
}