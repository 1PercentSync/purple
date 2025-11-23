using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionSpawner : MonoBehaviour
{
    [Header("Explosion")]
    public GameObject explosionPrefab; 
    public float destroyDelay = 2f;     

    [Header("Scene")]
    public string sceneToLoad = "NextScene";

    public void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Need VFX assigned");
        }

        // Start the delayed scene change
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private System.Collections.IEnumerator SwitchSceneAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        SceneManager.LoadScene(sceneToLoad);
    }
}
