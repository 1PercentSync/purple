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

    [HideInInspector] public bool isActive = false;

    private List<int> sequence;
    private int playerStep = 0;
    private int failCount = 0;
    private bool inputEnabled = false;
    private bool isProcessingInput = false;
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    private Coroutine currentSequenceCoroutine;
    private Coroutine wrongInputCoroutine;

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

            Renderer rend = simonButtons[i].GetComponent<Renderer>();
            if (rend != null)
                originalColors[rend] = rend.material.color;
        }
    }

    public void StartMinigame()
    {
        StopAllMinigameCoroutines();
        ResetAllButtonColors();

        isActive = true;
        failCount = 0;
        playerStep = 0;
        inputEnabled = false;
        isProcessingInput = false;

        GenerateSequence();
        currentSequenceCoroutine = StartCoroutine(PlaySequenceWithDelay(startDelay));
    }

    void GenerateSequence()
    {
        sequence = new List<int>();
        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, simonButtons.Length));

        Debug.Log("Simon sequence: " + string.Join(",", sequence));
    }

    public void PlayerPress(int buttonIndex)
    {
        if (!IsInputEnabled()) return;
        if (buttonIndex < 0 || buttonIndex >= simonButtons.Length) return;
        if (playerStep >= sequence.Count) return;

        isProcessingInput = true;

        if (sequence[playerStep] == buttonIndex)
        {
            playerStep++;
            Renderer rend = simonButtons[buttonIndex].GetComponent<Renderer>();
            if (rend != null)
                StartCoroutine(FlashButtonAndUnlock(rend, Color.green, 0.3f));
            else
                isProcessingInput = false;

            if (playerStep >= sequence.Count)
            {
                inputEnabled = false;
                enterBomb?.OnSimonComplete();
            }
        }
        else
        {
            failCount++;
            inputEnabled = false;
            isProcessingInput = false;

            if (failCount >= maxFails)
            {
                ExitDueToFail();
                return;
            }

            playerStep = 0;

            if (wrongInputCoroutine != null)
                StopCoroutine(wrongInputCoroutine);

            wrongInputCoroutine = StartCoroutine(HandleWrongInput());
        }
    }

    void ExitDueToFail()
    {
        isActive = false;

        StopAllMinigameCoroutines();

        ResetAllButtonColors();
        playerStep = 0;
        failCount = 0;

        enterBomb?.ExitBombCamera();
    }

    public void ResetMinigame(bool autoStart = true)
    {
        StopAllMinigameCoroutines();

        ResetAllButtonColors();
        playerStep = 0;
        failCount = 0;
        inputEnabled = false;
        isProcessingInput = false;
        isActive = false;

        if (autoStart)
            StartMinigame();
    }

    public bool IsInputEnabled()
    {
        return isActive && inputEnabled && !isProcessingInput;
    }

    IEnumerator PlaySequenceWithDelay(float delay)
    {
        if (!isActive) yield break;
        yield return new WaitForSeconds(delay);
        if (!isActive) yield break;

        yield return PlaySequence();

        if (isActive)
            inputEnabled = true;
    }

    IEnumerator PlaySequence()
    {
        inputEnabled = false;
        isProcessingInput = false;

        foreach (int index in sequence)
        {
            if (!isActive) yield break;
            if (index < 0 || index >= simonButtons.Length) continue;

            GameObject button = simonButtons[index];
            if (button != null)
            {
                Renderer rend = button.GetComponent<Renderer>();
                SimonButton sb = button.GetComponent<SimonButton>();
                sb?.PlaySound();
                if (rend != null)
                    yield return StartCoroutine(FlashButton(rend, Color.red, highlightDuration));
            }

            yield return new WaitForSeconds(timeBetweenHighlights);
        }
    }

    IEnumerator FlashButton(Renderer rend, Color color, float duration)
    {
        if (rend == null || !isActive) yield break;

        Color original = originalColors.ContainsKey(rend) ? originalColors[rend] : Color.white;
        rend.material.color = color;
        yield return new WaitForSeconds(duration);

        if (rend != null && isActive)
            rend.material.color = original;
    }

    IEnumerator FlashButtonAndUnlock(Renderer rend, Color color, float duration)
    {
        yield return StartCoroutine(FlashButton(rend, color, duration));
        if (isActive)
            isProcessingInput = false;
    }

    IEnumerator HandleWrongInput()
    {
        if (!isActive || failCount >= maxFails) yield break;

        yield return StartCoroutine(FlashAllButtons(Color.red, 0.5f));

        float waitTime = 0.5f;
        float elapsed = 0f;
        while (elapsed < waitTime)
        {
            if (!isActive || failCount >= maxFails) yield break;
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isActive || failCount >= maxFails) yield break;

        currentSequenceCoroutine = StartCoroutine(PlaySequenceWithDelay(1f));
    }

    IEnumerator FlashAllButtons(Color color, float duration)
    {
        if (!isActive) yield break;

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

        if (!isActive) yield break;

        foreach (var rend in rends)
        {
            if (rend != null)
            {
                Color original = originalColors.ContainsKey(rend) ? originalColors[rend] : Color.white;
                rend.material.color = original;
            }
        }
    }

    void ResetAllButtonColors()
    {
        foreach (var btn in simonButtons)
        {
            if (btn != null)
            {
                Renderer rend = btn.GetComponent<Renderer>();
                if (rend != null)
                {
                    Color original = originalColors.ContainsKey(rend) ? originalColors[rend] : Color.white;
                    rend.material.color = original;
                }
            }
        }
    }

    void StopAllMinigameCoroutines()
    {
        if (currentSequenceCoroutine != null)
            StopCoroutine(currentSequenceCoroutine);
        if (wrongInputCoroutine != null)
            StopCoroutine(wrongInputCoroutine);

        currentSequenceCoroutine = null;
        wrongInputCoroutine = null;
    }

    void OnDisable()
    {
        isActive = false;
        StopAllMinigameCoroutines();
        ResetAllButtonColors();
    }
}
