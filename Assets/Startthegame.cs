using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Startthegame : MonoBehaviour
{
    public GameObject loadingImageCanvas; // Canvas with the image
    public float imageDuration = 3f; // Time to display the image

    private void Start()
    {
        StartCoroutine(ShowLoadingImage());
    }

    IEnumerator ShowLoadingImage()
    {
        // Show the image canvas
        if (loadingImageCanvas != null)
            loadingImageCanvas.SetActive(true);

        // Wait for 3 seconds
        yield return new WaitForSeconds(imageDuration);

        // Hide the image canvas
        if (loadingImageCanvas != null)
            loadingImageCanvas.SetActive(false);
    }
}

