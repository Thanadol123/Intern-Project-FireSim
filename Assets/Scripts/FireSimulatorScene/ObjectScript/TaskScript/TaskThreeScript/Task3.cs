using UnityEngine;
using UnityEngine.UI;

public class Task3 : MonoBehaviour
{
    public static Task3 Instance { get; private set; }

    private TaskManager taskManager;
    private bool taskDone = false;

    public Canvas taskCanvas;
    public Canvas oldMenuCanvas;
    public InventorySlot[] inventorySlots;
    public CharacterController characterController;
    public Button resetButton;
    public GameObject taskUIPopup;

    //private bool canMove = true;
    private int correctPlacements = 0;
    //private int maxScore = 40;
    private int scorePerCorrectPlacement = 10;

    public Collider interactionCollider;
    public MeshRenderer objectRenderer;
    public DraggableItem[] draggableItems;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        taskManager = Object.FindFirstObjectByType<TaskManager>();

        if (taskCanvas != null)
            taskCanvas.gameObject.SetActive(false);

        if (oldMenuCanvas != null)
            oldMenuCanvas.gameObject.SetActive(true);

        if (characterController != null)
            characterController.enabled = true;

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetTask);

        draggableItems = Object.FindObjectsByType<DraggableItem>(FindObjectsSortMode.None);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowCanvas();
        }

        if (!taskDone && Input.GetKeyDown(KeyCode.L))
        {
            CompleteTask();
        }
    }

    private void ShowCanvas()
    {
        if (taskCanvas != null)
        {
            taskCanvas.gameObject.SetActive(true);
        }

        if (oldMenuCanvas != null)
        {
            oldMenuCanvas.gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //canMove = false;
    }

    public void HideCanvas()
    {
        if (taskCanvas != null)
        {
            taskCanvas.gameObject.SetActive(false);
        }

        if (oldMenuCanvas != null)
        {
            oldMenuCanvas.gameObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //canMove = true;
    }

    public void ObjectPlacedCorrectly()
    {
        correctPlacements++;

        if (correctPlacements == inventorySlots.Length)
        {
            CompleteTask();
        }
    }

    private void CompleteTask()
    {
        if (taskManager != null && !taskDone)
        {
            taskDone = true;
            taskManager.CompleteTask(2);

            HideCanvas();

            DisableInteraction();
        }
    }

    public int GetScore()
    {
        return correctPlacements * scorePerCorrectPlacement;
    }

    private void DisableInteraction()
    {
        if (interactionCollider != null)
        {
            interactionCollider.enabled = false; 
        }

        if (objectRenderer != null)
        {
            objectRenderer.enabled = false; 
        }

        if (taskUIPopup != null)
        {
            Destroy(taskUIPopup); 
        }

        if (taskCanvas != null)
        {
            Destroy(taskCanvas.gameObject); 
        }

        Destroy(gameObject, 1f); 
    }




    public void ResetTask()
    {
        foreach (DraggableItem item in draggableItems)
        {
            item.ResetPosition();
        }

        correctPlacements = 0;
        taskDone = false;

        if (interactionCollider != null)
        {
            interactionCollider.enabled = true;
        }

        if (objectRenderer != null)
        {
            objectRenderer.enabled = true;
        }
    }
}
