using UnityEngine;
using System.Collections;

public class FireBlock : MonoBehaviour
{
    private Fire fire;
    private bool isExtinguishing = false; 

    void Start()
    {
        fire = GetComponent<Fire>(); 
    }

    void Update()
    {
        if (!isExtinguishing && fire != null && fire.GetIntensity() <= 0)
        {
            isExtinguishing = true;
            StartCoroutine(ExtinguishFireWithDelay());
        }
    }

    IEnumerator ExtinguishFireWithDelay()
    {
        Debug.Log("🔥 Fire extinguished, removing block in 2 seconds...");
        yield return new WaitForSeconds(2f); 

        Debug.Log("🔥 Fire removed!");
        Destroy(gameObject); 
    }
}
