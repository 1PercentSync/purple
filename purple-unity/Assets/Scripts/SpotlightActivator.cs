using UnityEngine;

public class SpotlightActivator : MonoBehaviour
{

    public QuotaManager quotaManager;
    public GameObject[] spotlights;
    void Start()
    {
        quotaManager.OnQuotaComplete += ActivateSpotlights;

        foreach (var light in spotlights)
            light.SetActive(false);
    }

    private void ActivateSpotlights()
    {
        Debug.Log("Spotlights are active");

        foreach (var light in spotlights)
        {
            light.SetActive(true);
            AudioSource audio = light.GetComponent<AudioSource>();
            if (audio != null) {
                audio.Play();
             }
        }


    }

    private void OnDestroy()
    {
        if (quotaManager != null) 
            quotaManager.OnQuotaComplete -= ActivateSpotlights;
    }

}
