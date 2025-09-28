using UnityEngine;

public class SimonButton : MonoBehaviour
{
    [HideInInspector] public int buttonIndex;
    [HideInInspector] public BombMinigame controller;

    private void OnMouseDown()
    {
        if (controller != null)
            controller.PlayerPress(buttonIndex);
    }
}
