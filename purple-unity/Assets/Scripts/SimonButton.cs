using UnityEngine;

public class SimonButton : MonoBehaviour
{
    [HideInInspector] public int buttonIndex;
    [HideInInspector] public BombMinigame controller;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (controller != null)
        {
            controller.PlayerPress(buttonIndex);
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
