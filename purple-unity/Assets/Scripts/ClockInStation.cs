using UnityEngine;

public class ClockInStation : MonoBehaviour
{
    [Header("References")]
    public WorkClock workClock;  // Drag your WorkClock here

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (workClock != null)
            {
                workClock.StartWorkday();
                Debug.Log("Clock-in triggered!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
}
