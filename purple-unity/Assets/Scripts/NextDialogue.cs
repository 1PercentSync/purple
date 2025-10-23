using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    int index = 2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && transform.childCount > 1)
        {
            if (PlayerMovement.dialogue)
            {
                transform.GetChild(index).gameObject.SetActive(true);
                index += 1;
                if (transform.childCount == index)
                {
                    index = 2;
                    PlayerMovement.dialogue = false;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
