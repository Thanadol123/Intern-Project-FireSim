using System.Collections;
using UnityEngine;

public class FireGrownUp : MonoBehaviour
{
    public GameObject firePrefab; // Assign your Fire Prefab here
    public float spawnDelay = 1f; // Delay before fire starts growing
    public float maxScale = 3f; // Maximum size of fire
    public float growthDuration = 10f; // Fire will reach full size in 10 seconds

    private GameObject spawnedFire;
    private AudioSource fireAudio;

    void Start()
    {
        StartCoroutine(SpawnAndGrowFire());
    }

    IEnumerator SpawnAndGrowFire()
    {
        yield return new WaitForSeconds(spawnDelay);

        // Spawn the fire prefab at this GameObject's position
        spawnedFire = Instantiate(firePrefab, transform.position, Quaternion.identity);
        spawnedFire.transform.localScale = Vector3.zero; // Start with no size

        // Get the fire sound (if the prefab has an AudioSource)
        fireAudio = spawnedFire.GetComponent<AudioSource>();
        if (fireAudio != null)
        {
            fireAudio.volume = 0; // Start silent
            fireAudio.Play();
        }

        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(maxScale, maxScale, maxScale);

        // Gradually increase the size and sound volume over 10 seconds
        while (elapsedTime < growthDuration)
        {
            float progress = elapsedTime / growthDuration;
            spawnedFire.transform.localScale = Vector3.Lerp(startScale, endScale, progress);

            if (fireAudio != null)
            {
                fireAudio.volume = Mathf.Lerp(0, 1, progress);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final size and volume
        spawnedFire.transform.localScale = endScale;
        if (fireAudio != null) fireAudio.volume = 1;
    }
}
