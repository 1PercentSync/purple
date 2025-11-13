using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NumpadMinigame : MonoBehaviour
{
    [Header("Numpad Buttons")]
    public GameObject[] numpadButtons;

    [Header("UI")]
    public TMP_Text screenText;

    public float codeDisplayTime = 3f;
    public int codeLength = 4;
    public int maxFails = 2;

    private List<int> codeSequence;
    private int playerStep = 0;
    private int failCount = 0;
    private bool inputEnabled = false;
    private bool isActive = false;
    private bool isProcessingInput = false;

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
            if (col != null) col.enabled = false;
        }

        ClearScreen();
    }

    public void StartMinigame()
    {
        StopAllCoroutines();

        isActive = true;
        GenerateCode();
        playerStep = 0;
        failCount = 0;
        inputEnabled = false;
        isProcessingInput = false;

        EnableAllButtons();

        StartCoroutine(ShowCodeThenStartInput());
    }

    void GenerateCode()
    {
        codeSequence = new List<int>();
        for (int i = 0; i < codeLength; i++)
            codeSequence.Add(Random.Range(1, numpadButtons.Length + 1));
    }

    IEnumerator ShowCodeThenStartInput()
    {
        if (screenText != null)
        {
            screenText.color = Color.yellow;
            screenText.text = string.Join(" ", codeSequence);
        }

        yield return new WaitForSeconds(codeDisplayTime);

        playerStep = 0;
        inputEnabled = true;
        isProcessingInput = false;
        UpdateScreen();
    }

    public void PressNumber(int number)
    {
        if (!inputEnabled || !isActive || isProcessingInput) return;
        if (playerStep >= codeSequence.Count) return;

        isProcessingInput = true;

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
            else
                isProcessingInput = false;
        }
        else
        {
            failCount++;
            playerStep = 0;
            UpdateScreen(true);

            if (failCount >= maxFails)
            {
                inputEnabled = false;
                isActive = false;
                isProcessingInput = false;
                DisableAllButtons();
                StopAllCoroutines();
                enterBomb?.ExitBombCamera();
            }
            else
            {
                StartCoroutine(UnlockAfterDelay(0.5f));
            }
        }
    }

    IEnumerator UnlockAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isProcessingInput = false;
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
                display += (i < playerStep ? codeSequence[i] : "_") + " ";
            screenText.text = display.TrimEnd();
        }
    }

    IEnumerator ResetScreenColor()
    {
        yield return new WaitForSeconds(0.5f);
        if (screenText != null)
        {
            screenText.color = Color.white;
            UpdateScreen();
        }
    }

    void EnableAllButtons()
    {
        foreach (var btn in numpadButtons)
            if (btn != null) btn.GetComponent<Collider>().enabled = true;
    }

    void DisableAllButtons()
    {
        foreach (var btn in numpadButtons)
            if (btn != null) btn.GetComponent<Collider>().enabled = false;
    }

    void ClearScreen()
    {
        if (screenText != null)
        {
            screenText.text = "";
            screenText.color = Color.white;
        }
    }

    public void ResetMinigame(bool autoStart = true)
    {
        StopAllCoroutines();
        isActive = false;
        playerStep = 0;
        failCount = 0;
        inputEnabled = false;
        isProcessingInput = false;
        DisableAllButtons();
        ClearScreen();

        if (autoStart)
            StartMinigame();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetMinigame(false);
    }
}
