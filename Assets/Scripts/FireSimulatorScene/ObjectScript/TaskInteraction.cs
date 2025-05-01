using UnityEngine;

public class TaskInteraction : MonoBehaviour
{
    public GameObject taskPopup;  // The popup UI for the task
    public AudioSource AlarmSound;
    public KeyCode interactionKey = KeyCode.E;  // Set this in the Inspector

    private bool isPlayerInside = false;  // Track if the player is in the trigger zone

    void Start()
    {
        if (taskPopup != null)
        {
            taskPopup.SetActive(false);
        }
    }

    void Update()
    {
        // If the player is inside and presses the key, play the sound
        if (isPlayerInside && Input.GetKeyDown(interactionKey))
        {
            if (AlarmSound != null)
            {
                AlarmSound.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            taskPopup?.SetActive(true);
            isPlayerInside = true;  // Mark player as inside
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            taskPopup?.SetActive(false);
            isPlayerInside = false;  // Mark player as outside
        }
    }
}
