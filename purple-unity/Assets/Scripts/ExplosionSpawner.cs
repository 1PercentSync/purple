using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    public GameObject explosionPrefab;

    public void SpawnExplosion(Vector3 position)
    {
        if (explosionPrefab == null)
        {
            Debug.Log("Need VFX assigned");
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        Destroy(explosion, 5f);

    }
}
