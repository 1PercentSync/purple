using UnityEngine;

public class TirednessEffect : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup overlay;

    [Header("Audio")]
    public AudioClip snoreClip;
    private AudioSource snoreSource;

    [Header("Tiredness")]
    public float tiredness = 0f;
    public float maxTiredness = 100f;

    private float timeAwake = 0f;
    public float timeToFullSleep = 180f; 

    [Header("Reset Settings")]
    public float timeAtMaxBeforeReset = 15f;
    private float maxTirednessTimer = 0f;

    [Header("Dozing Settings")]
    public float minDozeDelay = 3f;
    public float maxDozeDelay = 7f;

    public float closeSpeed = 0.5f;
    public float openSpeed = 0.3f;

    public float maxClosedAmount = 1f;
    public float sleepThreshold = 95f;

    private float nextDozeTime;
    private bool dozing = false;
    private bool eyesClosed = false;

    void Start()
    {
        snoreSource = gameObject.AddComponent<AudioSource>();
        snoreSource.clip = snoreClip;
        snoreSource.loop = true;
        snoreSource.playOnAwake = false;
        snoreSource.volume = 0.6f;

        ScheduleNextDoze();
    }

    void Update()
    {
        timeAwake += Time.deltaTime;
        float t = Mathf.Clamp01(timeAwake / timeToFullSleep);
        tiredness = Mathf.Lerp(0f, maxTiredness, t);

        if (Mathf.Approximately(tiredness, maxTiredness))
        {
            maxTirednessTimer += Time.deltaTime;

            if (maxTirednessTimer >= timeAtMaxBeforeReset)
            {
                ResetTirednessFully();
                return;
            }
        }
        else
        {
            maxTirednessTimer = 0f; 
        }

        // Snoring only when full asleep & eyes closed
        if (eyesClosed && tiredness >= sleepThreshold)
        {
            if (!snoreSource.isPlaying)
                snoreSource.Play();
        }
        else
        {
            if (snoreSource.isPlaying)
                snoreSource.Stop();
        }

        float idleAlpha = Mathf.Lerp(0f, 0.35f, tiredness / maxTiredness);

        if (!dozing)
        {
            overlay.alpha = Mathf.Lerp(overlay.alpha, idleAlpha, Time.deltaTime);

            if (Time.time >= nextDozeTime)
                StartCoroutine(DozeRoutine());
        }
    }

    void ResetTirednessFully()
    {
        // Player snaps awake
        tiredness = 0f;
        timeAwake = 0f;
        maxTirednessTimer = 0f;

        eyesClosed = false;
        dozing = false;
        if (snoreSource.isPlaying) snoreSource.Stop();

        overlay.alpha = 0f;

        ScheduleNextDoze();
    }

    void ScheduleNextDoze()
    {
        float ti = tiredness / maxTiredness;
        float delay = Random.Range(minDozeDelay - ti * 2f, maxDozeDelay - ti * 2f);
        delay = Mathf.Clamp(delay, 0.3f, maxDozeDelay);
        nextDozeTime = Time.time + delay;
    }

    System.Collections.IEnumerator DozeRoutine()
    {
        dozing = true;

        float ti = tiredness / maxTiredness;
        float targetClosed = Mathf.Lerp(0.5f, maxClosedAmount, ti);

        // Close eyes
        while (overlay.alpha < targetClosed)
        {
            overlay.alpha += Time.deltaTime * closeSpeed;
            yield return null;
        }

        eyesClosed = true;

        // Hold closed based on tiredness
        float hold = Mathf.Lerp(0.2f, 1.5f, ti);
        yield return new WaitForSeconds(hold);

        // Open eyes
        float idleAlpha = Mathf.Lerp(0f, 0.35f, ti);
        while (overlay.alpha > idleAlpha)
        {
            overlay.alpha -= Time.deltaTime * openSpeed;
            yield return null;
        }

        eyesClosed = false;

        dozing = false;
        ScheduleNextDoze();
    }
}
