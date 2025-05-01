using UnityEngine;

public class Task4 : MonoBehaviour
{
    private TaskManager taskManager;
    private bool taskCompleted = false;
    private bool hasFiresSpawned = false;

    [System.Obsolete]
    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    void Update()
    {
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fire");

        // Mark fires as spawned once at least one exists
        if (fireObjects.Length > 0)
        {
            hasFiresSpawned = true;
        }

        // Only check for completion if fires have spawned
        if (hasFiresSpawned && !taskCompleted && AreAllFiresExtinguished(fireObjects))
        {
            CompleteTask4();
        }
    }

    private bool AreAllFiresExtinguished(GameObject[] fireObjects)
    {
        foreach (GameObject fire in fireObjects)
        {
            Fire fireComponent = fire.GetComponent<Fire>();
            if (fireComponent != null && fireComponent.GetIntensity() > 0)
            {
                return false; // If any fire is still burning, task is not complete
            }
        }
        return true; // All fires are extinguished
    }

    private void CompleteTask4()
    {
        taskCompleted = true;
        if (taskManager != null)
        {
            taskManager.CompleteTask(3);
            Debug.Log("Task 4 Completed: All fires are extinguished!");
        }
        else
        {
            taskManager.tasks[3].color = Color.red;
            Debug.Log("You must complete all previous tasks before Task 4!");
        }
    }
}
