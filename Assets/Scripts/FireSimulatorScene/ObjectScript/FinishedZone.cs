using UnityEngine;
using UnityEngine.EventSystems;

public class FinishedZone : MonoBehaviour
{
    public TaskManager taskManager;
    public MonoBehaviour playerMovementScript;
    public MonoBehaviour[] otherInputScripts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (taskManager != null)
            {
                taskManager.CompleteTask(4);
                taskManager.ShowFinalReport(); 

                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }

                foreach (var script in otherInputScripts)
                {
                    if (script != null)
                    {
                        script.enabled = false;
                    }
                }

                EnableUIInteraction();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void EnableUIInteraction()
    {
        EventSystem.current.SetSelectedGameObject(null);

        CanvasGroup failCanvasGroup = taskManager.failPanel?.GetComponent<CanvasGroup>();
        if (failCanvasGroup != null)
        {
            failCanvasGroup.interactable = true;
            failCanvasGroup.blocksRaycasts = true;
        }

        CanvasGroup reportCanvasGroup = taskManager.reportPanel?.GetComponent<CanvasGroup>();
        if (reportCanvasGroup != null)
        {
            reportCanvasGroup.interactable = true;
            reportCanvasGroup.blocksRaycasts = true;
        }
    }
}
