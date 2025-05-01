using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public TMP_Text taskTimerText;
    public TMP_Text[] tasks;
    public TMP_Text scoreText;
    public TMP_Text reportText;
    public GameObject reportPanel;
    public GameObject failPanel;
    public GameObject closePanel;
    public TMP_Text totalCompletionTimeText; 
    public TMP_Text failTotalScoreText;
    public TMP_Text failTotalCompletionTimeText;
    public TMP_Text failReportText;
    public TaskHighlighter[] taskHighlights; 



    public GameObject[] fireIcons;

    private int totalScore = 0;
    public bool[] taskCompleted;
    private bool[] taskSkipped;
    private float[] taskFailureTimes;
    private float[] completionTimes;
    private int[] taskScores;
    private int currentTaskIndex = 0;
    private bool allTasksCompleted = false;
    private float totalCompletionTime = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        int taskCount = tasks.Length;
        taskCompleted = new bool[taskCount];
        taskSkipped = new bool[taskCount];
        taskFailureTimes = new float[taskCount];
        completionTimes = new float[taskCount];
        taskScores = new int[taskCount];

        taskFailureTimes[0] = 210f;
        taskFailureTimes[1] = 160f;
        taskFailureTimes[2] = 100f;
        taskFailureTimes[3] = 50f;
        taskFailureTimes[4] = 0f;

        taskScores[0] = 10;
        taskScores[1] = 10;
        taskScores[2] = 0;
        taskScores[3] = 20;
        taskScores[4] = 20;

        reportPanel.SetActive(false);
        failPanel.SetActive(false);
        scoreText.text = $"Score: {totalScore}";

        foreach (GameObject fireIcon in fireIcons)
        {
            if (fireIcon != null)
                fireIcon.SetActive(true);
        }

        for (int i = 0; i < taskHighlights.Length; i++)
        {
            if (taskHighlights[i] != null)
            {
                taskHighlights[i].DisableHighlight();
            }
        }

        if (taskHighlights.Length > 0 && taskHighlights[0] != null)
        {
            taskHighlights[0].EnableHighlight();
        }
        currentTaskIndex = 0;
    }




    void Update()
    {
        if (allTasksCompleted) return;

        float remainingTime = TimedEventAfterCutscene.instance.GetRemainingTime();

        if (currentTaskIndex < tasks.Length)
        {
            if (!taskCompleted[currentTaskIndex] && remainingTime <= taskFailureTimes[currentTaskIndex])
            {
                FailTask(currentTaskIndex);
            }
        }

        if (currentTaskIndex >= tasks.Length)
        {
            allTasksCompleted = true;
            ShowFinalReport();
        }
    }

    void ActivateHighlight(int index)
    {
        if (index < taskHighlights.Length && taskHighlights[index] != null)
        {
            taskHighlights[index].EnableHighlight();
        }
    }

    void DeactivateHighlight(int index)
    {
        if (index < taskHighlights.Length && taskHighlights[index] != null)
        {
            taskHighlights[index].DisableHighlight();
        }
    }


    public bool AllTasksCompleted()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!taskCompleted[i] && !taskSkipped[i]) 
            {
                return false;
            }
        }
        return true;
    }


    public void AddScore(int score)
    {
        totalScore += score;
        scoreText.text = $"{totalScore}/ 100";
    }


    public void CompleteTask(int taskIndex)
    {
        if (taskIndex < tasks.Length && !taskCompleted[taskIndex])
        {
            if (taskIndex != currentTaskIndex)
            {
                return;
            }

            if (taskIndex == 4 && !(taskCompleted[0] && taskCompleted[1] && taskCompleted[2] && taskCompleted[3]))
            {
                tasks[4].color = Color.red;
                return;
            }

            float totalTime = TimedEventAfterCutscene.instance.GetTotalTime();
            float remainingTime = TimedEventAfterCutscene.instance.GetRemainingTime();
            float timeTaken = totalTime - remainingTime;

            taskCompleted[taskIndex] = true;
            completionTimes[taskIndex] = timeTaken;
            tasks[taskIndex].color = new Color32(170, 170, 170, 255);

            if (taskIndex == 2)
            {
                taskScores[2] = Task3.Instance.GetScore();
            }

            AddScore(taskScores[taskIndex]);

            if (fireIcons[taskIndex] != null)
            {
                fireIcons[taskIndex].SetActive(false);
            }

            totalCompletionTime = timeTaken;

            DeactivateHighlight(taskIndex);

            currentTaskIndex++;
            ActivateHighlight(currentTaskIndex);

            if (taskIndex == 3 && taskHighlights.Length > 4 && taskHighlights[4] != null)
            {
                ActivateHighlight(4); // Activate Task 4 Highlight
            }
            if (taskIndex == 4 && taskHighlights.Length > 5 && taskHighlights[5] != null)
            {
                ActivateHighlight(5); // Activate Task 5 Highlight
            }
        }

        if (currentTaskIndex >= tasks.Length)
        {
            allTasksCompleted = true;
            ShowFinalReport();
        }
    }




    public void UpdateTaskTimer(float remainingTime)
    {
        if (currentTaskIndex < tasks.Length && !taskCompleted[currentTaskIndex])
        {
            if (remainingTime <= taskFailureTimes[currentTaskIndex])
            {
                FailTask(currentTaskIndex);
            }
        }
    }


    public void FailTask(int taskIndex)
    {
        if (taskIndex < tasks.Length && !taskCompleted[taskIndex])
        {
            tasks[taskIndex].color = Color.red;
            completionTimes[taskIndex] = -1f; // ✅ Ensure failed tasks have a value
            currentTaskIndex++;

            if (fireIcons[taskIndex] != null)
            {
                fireIcons[taskIndex].SetActive(false);
            }
        }

        if (TimedEventAfterCutscene.instance.GetRemainingTime() <= 0)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (!taskCompleted[i] && !taskSkipped[i])
                {
                    taskSkipped[i] = false;
                    completionTimes[i] = -1f;
                    tasks[i].color = Color.red;
                }
            }

            ShowFailReport();
        }
    }




    public void ShowFailReport()
    {
        if (reportPanel != null) reportPanel.SetActive(false);
        if (failPanel != null) failPanel.SetActive(true);
        if (closePanel != null) closePanel.SetActive(false);

        string report = "Final Report:\n";

        for (int i = 0; i < tasks.Length; i++)
        {
            if (taskCompleted[i])
            {
                report += $"Task {i + 1}:\t{completionTimes[i]:F2}s\t\t\t\tEarned {taskScores[i]} pts\n";
            }
            else if (taskSkipped[i] && TimedEventAfterCutscene.instance.GetRemainingTime() > 0)
            {
                report += $"Task {i + 1}:\tSkip\t\t\t\t\tEarned 0 pts\n";
            }
            else
            {
                report += $"Task {i + 1}:\tFail\t\t\t\t\tEarned 0 pts\n";
            }
        }

        if (failReportText != null)
        {
            failReportText.text = report;
        }

        if (failTotalCompletionTimeText != null)
        {
            failTotalCompletionTimeText.text = "04:00";
        }


        if (failTotalScoreText != null)
        {
            failTotalScoreText.text = $"{totalScore}/100";
        }


        TimedEventAfterCutscene.instance.StopTimer();
        EnableUIInteraction();
        LockCursor(false);
    }


    public void ShowFinalReport()
    {
        if (TimedEventAfterCutscene.instance.GetRemainingTime() <= 0) 
        {
            ShowFailReport();
            return;
        }

        if (AllTasksCompleted())
        {
            reportPanel.SetActive(true);
            failPanel.SetActive(false);
        }
        else
        {
            reportPanel.SetActive(true);
            failPanel.SetActive(false);
        }

        if (closePanel != null)
        {
            closePanel.SetActive(false);
        }

        string report = "Final Report:\n";
        for (int i = 0; i < tasks.Length; i++)
        {
            if (taskCompleted[i])
                report += $"Task {i + 1}:\t{completionTimes[i]:F2}s\t\t\t\tEarned {taskScores[i]} pts\n";
            else if (taskSkipped[i])
                report += $"Task {i + 1}:\tSkip\t\t\t\t\tEarned 0 pts\n";
            else
                report += $"Task {i + 1}:\tFail\t\t\t\t\tEarned 0 pts\n";
        }

        reportText.text = report;

        if (totalCompletionTimeText != null)
        {
            int minutes = Mathf.FloorToInt(totalCompletionTime / 60);
            int seconds = Mathf.FloorToInt(totalCompletionTime % 60);
            totalCompletionTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }


        TimedEventAfterCutscene.instance.StopTimer();
        EnableUIInteraction();
        LockCursor(false);
    }


    void DisablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
        }
    }

    void LockCursor(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        EnableUIInteraction();
    }

    private void EnableUIInteraction()
    {
        EventSystem.current.SetSelectedGameObject(null);

        CanvasGroup canvasGroup = failPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        canvasGroup = reportPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }


}
