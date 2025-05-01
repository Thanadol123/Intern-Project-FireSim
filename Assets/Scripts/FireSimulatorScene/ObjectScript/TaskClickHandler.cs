using UnityEngine;
using UnityEngine.EventSystems;

public class TaskClickHandler : MonoBehaviour, IPointerClickHandler
{
    private TaskManager taskManager;
    public int taskIndex;

    public void SetTaskManager(TaskManager manager)
    {
        taskManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        taskManager?.CompleteTask(taskIndex);
        gameObject.SetActive(false);
    }
}
