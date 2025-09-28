using UnityEngine;

public class NumpadButton : MonoBehaviour
{
    [HideInInspector] public int number;                  // 0-9
    [HideInInspector] public NumpadMinigame controller;   // Reference to NumpadMinigame

    private void OnMouseDown()
    {
        if (controller != null)
            controller.PressNumber(number);
    }
}
