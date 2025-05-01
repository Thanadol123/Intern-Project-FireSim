using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using TMPro;
using System.Collections;

public class TimedEventAfterCutscene : MonoBehaviour
{
    public PlayableDirector timeline;
    public TMP_Text timerText;
    public GameObject failCanvas;
    public GameObject player;
    private MonoBehaviour playerMovementScript;

    private bool timerRunning = false;
    private float remainingTime = 240f;
    public static TimedEventAfterCutscene instance;

    void Awake()
    {
        instance = this;
        if (failCanvas != null)
            failCanvas.SetActive(false);

        if (player != null)
        {
            playerMovementScript = player.GetComponent<MonoBehaviour>();
        }
    }

    void Start()
    {
        if (timeline != null)
        {
            timeline.stopped += OnTimelineFinished;
        }
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        StartCoroutine(StartTimerWithDelay());
    }

    IEnumerator StartTimerWithDelay()
    {
        yield return new WaitForSeconds(2f);
        timerRunning = true;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (timerRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            if (timerText != null)
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            TaskManager.Instance.UpdateTaskTimer(remainingTime);

            yield return null;
        }

        if (remainingTime <= 0 && !TaskManager.Instance.AllTasksCompleted()) 
        {
            ShowFailCanvas();
        }
    }


    void ShowFailCanvas()
    {
        timerRunning = false;

        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.ShowFailReport(); 
        }

        if (playerMovementScript != null)
            playerMovementScript.enabled = false; 

        LockCursor(false);
    }


    void LockCursor(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("DtiBuilding");
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public float GetTotalTime()
    {
        return 240f;
    }


}
