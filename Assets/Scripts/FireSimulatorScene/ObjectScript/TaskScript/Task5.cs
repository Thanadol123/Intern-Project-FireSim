using UnityEngine;

public class Task5 : MonoBehaviour
{
    private TaskManager taskManager;
    private bool taskDone = false;

    [System.Obsolete]
    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !taskDone)
        {

            if (taskManager != null && taskManager.taskCompleted[0] && taskManager.taskCompleted[1] && taskManager.taskCompleted[2] && taskManager.taskCompleted[3])
            {
                taskDone = true; 


                taskManager.CompleteTask(4);


                Debug.Log("Task 5 completed!");

                
                taskManager.ShowFinalReport();
            }
            else
            {
                taskManager.tasks[4].color = Color.red; 
                Debug.Log("You must complete all previous tasks before Task 5!");
            }
        }
    }
}
