using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FireSpawner : MonoBehaviour
{
    public GameObject firePrefab;
    public Transform[] spawnPositions;
    public TMP_Text timerText;
    public float spawnDelay = 1f;

    private bool spawnFires = false;
    private int currentFireIndex = 0;
    private List<GameObject> spawnedFires = new List<GameObject>();

    void Start()
    {
        spawnFires = false;
        StartCoroutine(WaitForCutsceneToEnd());
    }

    IEnumerator WaitForCutsceneToEnd()
    {
        while (TimedEventAfterCutscene.instance == null || TimedEventAfterCutscene.instance.GetRemainingTime() == 240)
        {
            yield return null;
        }
        yield return new WaitForSeconds(3f);

        StartFireSpawning();
    }

    void StartFireSpawning()
    {
        spawnFires = true;
        StartCoroutine(SpawnFires());
    }

    IEnumerator SpawnFires()
    {
        while (spawnFires && currentFireIndex < spawnPositions.Length)
        {
            if (currentFireIndex == 0)
            {
                SpawnFireAtPosition(currentFireIndex);
                currentFireIndex++;
                yield return new WaitForSeconds(spawnDelay);
                continue;
            }

            if (spawnedFires.Count > 0)
            {
                Fire firstFire = spawnedFires[0].GetComponent<Fire>();
                if (firstFire != null && firstFire.GetIntensity() <= 0)
                {
                    yield break;
                }
            }

            SpawnFireAtPosition(currentFireIndex);
            currentFireIndex++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnFireAtPosition(int index)
    {
        if (index < spawnPositions.Length)
        {
            GameObject fireInstance = Instantiate(firePrefab, spawnPositions[index].position, Quaternion.identity);
            fireInstance.tag = "Fire";
            spawnedFires.Add(fireInstance);

            BoxCollider fireCollider = fireInstance.AddComponent<BoxCollider>();
            fireCollider.size = new Vector3(5, 5, 5);
            fireCollider.isTrigger = false;

            fireInstance.AddComponent<FireBlock>();
        }
    }
}
