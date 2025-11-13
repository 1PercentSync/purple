using UnityEngine;

public class NumpadButton : MonoBehaviour
{
    [HideInInspector] public int number;                  // 0–9
    [HideInInspector] public NumpadMinigame controller;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (controller != null)
        {
            controller.PressNumber(number);
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
