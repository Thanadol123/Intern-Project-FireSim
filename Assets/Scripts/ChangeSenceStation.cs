using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneStation : MonoBehaviour
{
    public GameObject Instruction;
    public string SceneName; // The scene to load after the loading screen
    private bool canChangeScene = false;

    private void Update()
    {
        if (canChangeScene && Input.GetKeyDown(KeyCode.F))
        {
            LoadLoadingScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instruction.SetActive(true);
            canChangeScene = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canChangeScene = false;
            Instruction.SetActive(false);
        }
    }

    void LoadLoadingScene()
    {
        // Store the next scene name in a static variable so the loading scene can access it
        LoadingSceneManager.NextSceneName = SceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
