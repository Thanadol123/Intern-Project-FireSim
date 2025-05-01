using UnityEngine;

public class Task1 : MonoBehaviour
{
    private TaskManager taskManager;
    private bool taskDone = false;

    [System.Obsolete]
    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    void Update()
    {
        if (!taskDone && Input.GetKeyDown(KeyCode.K))
        {
            CompleteTask();
        }
    }

    private void CompleteTask()
    {
        if (taskManager != null && !taskDone)
        {
            taskDone = true;
            taskManager.CompleteTask(0);
        }
    }
}
