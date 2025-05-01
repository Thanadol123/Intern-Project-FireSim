using UnityEngine;

public class Task2 : MonoBehaviour
{
    private TaskManager taskManager;
    private bool taskDone = false;
    private bool isNearTask = false;

    [System.Obsolete]
    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    void Update()
    {
        if (!taskDone && isNearTask && Input.GetKeyDown(KeyCode.E))
        {
            CompleteTask();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !taskDone)
        {
            isNearTask = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTask = false;
        }
    }

    private void CompleteTask()
    {
        if (taskManager != null && !taskDone)
        {
            if (!taskManager.tasks[0].color.Equals(new Color32(170, 170, 170, 255)))
            {
                taskManager.tasks[1].color = Color.red;
                Debug.Log("Task 1 must be completed first!");
                return;
            }

            taskDone = true;
            taskManager.CompleteTask(1);
        }
    }
}
