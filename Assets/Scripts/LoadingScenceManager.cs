using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string NextSceneName; // The scene to load after the loading screen
    public GameObject firstCanvas; // Assign the first canvas (GIF/video)
    public GameObject howToPlayCanvas; // Assign the second canvas ("How to Play")
    public float firstCanvasDuration = 9f; // Time before switching to second canvas

    private AsyncOperation operation;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(firstCanvasDuration);

        // Hide first canvas and show second canvas
        if (firstCanvas != null) firstCanvas.SetActive(false);
        if (howToPlayCanvas != null) howToPlayCanvas.SetActive(true);

        // Start loading the scene in the background but do not activate it
        operation = SceneManager.LoadSceneAsync(NextSceneName);
        operation.allowSceneActivation = false;

        // Wait until user presses any key
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Activate the scene
        operation.allowSceneActivation = true;
    }
}