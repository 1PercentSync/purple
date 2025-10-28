using UnityEngine;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    [TextArea(2, 5)]
    public string[] dialogueLines;
    public GameObject dialogueTemplate;
    public GameObject canvas;

    public bool useTagCheck = true;
    public string playerName = "HumanDummy_M White";

    private bool dialogueActive = false;
    private GameObject[] dialogueObjects;
    private int currentIndex = 0;

    void Start()
    {
        if (canvas != null) canvas.SetActive(false);
    }

    void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        dialogueActive = true;
        canvas.SetActive(true);

        dialogueObjects = new GameObject[dialogueLines.Length];
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            dialogueObjects[i] = CreateDialogue(dialogueLines[i]);
            dialogueObjects[i].SetActive(i == 0);
        }

        currentIndex = 0;
    }

    GameObject CreateDialogue(string text)
    {
        GameObject clone = Instantiate(dialogueTemplate, canvas.transform, false);
        clone.SetActive(false);

        RectTransform rt = clone.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        TextMeshProUGUI tmp = null;
        if (clone.transform.childCount > 1)
            tmp = clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (tmp == null)
            tmp = clone.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = text;

        return clone;
    }

    void NextDialogue()
    {
        if (dialogueObjects == null || dialogueObjects.Length == 0)
        {
            EndDialogue();
            return;
        }

        if (currentIndex >= 0 && currentIndex < dialogueObjects.Length)
            dialogueObjects[currentIndex].SetActive(false);

        currentIndex++;

        if (currentIndex < dialogueObjects.Length)
            dialogueObjects[currentIndex].SetActive(true);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        if (canvas != null) canvas.SetActive(false);

        if (dialogueObjects != null)
        {
            foreach (GameObject go in dialogueObjects)
                if (go != null) Destroy(go);
        }

        dialogueObjects = null;
        currentIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool matches = useTagCheck ? other.CompareTag("Player") : other.name == playerName;
        if (matches && !dialogueActive)
            StartDialogue();
    }

    private void OnTriggerExit(Collider other)
    {
        bool matches = useTagCheck ? other.CompareTag("Player") : other.name == playerName;
        if (matches)
            EndDialogue();
    }
}
