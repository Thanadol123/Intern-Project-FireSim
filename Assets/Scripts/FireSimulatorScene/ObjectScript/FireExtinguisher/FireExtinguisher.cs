using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    private static FireExtinguisher currentlyHeldExtinguisher; // 🔥 Ensure only one extinguisher is held

    private Transform playerHand;
    private bool isGrabbed = false;
    private Rigidbody rb;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private Renderer grabHandRenderer;

    [SerializeField] private Vector3 grabPositionOffset = new Vector3(0.2f, 0, 0.5f);
    [SerializeField] private Vector3 grabRotationOffset = new Vector3(0, 90, 0);
    [SerializeField] private GameObject sprayPrefab;
    [SerializeField] private Transform nozzleTransform;

    private bool isSpraying = false;
    private GameObject currentSprayInstance;

    [SerializeField] private AudioClip spraySound;
    private AudioSource audioSource;

    [Header("Fire Extinguishing")]
    [SerializeField] private float extinguishRate = 1.0f;
    [SerializeField] private float sprayRange = 3.0f;

    void Start()
    {
        if (!CompareTag("FireExtinguisher")) // ✅ Ensure extinguisher has correct tag
        {
            Debug.LogError($"{gameObject.name} is missing the FireExtinguisher tag!");
        }

        playerHand = GameObject.FindGameObjectWithTag("PlayerHand")?.transform;
        rb = GetComponent<Rigidbody>();

        grabHandRenderer = playerHand?.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        if (grabHandRenderer != null)
        {
            grabHandRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGrabbed && currentlyHeldExtinguisher == null)
                GrabExtinguisher();
            else if (isGrabbed)
                DropExtinguisher();
        }

        if (isGrabbed)
        {
            if (Input.GetMouseButtonDown(0)) StartSpraying();
            if (Input.GetMouseButtonUp(0)) StopSpraying();

            if (isSpraying && nozzleTransform != null)
            {
                UpdateSprayPosition();
                TryExtinguishFire();
            }
        }
    }

    void GrabExtinguisher()
    {
        if (playerHand != null)
        {
            isGrabbed = true;
            currentlyHeldExtinguisher = this; // 🔥 Ensure only one extinguisher is held
            transform.SetParent(playerHand);
            transform.localPosition = grabPositionOffset;
            transform.localRotation = Quaternion.Euler(grabRotationOffset);
            transform.localScale = originalScale;

            rb.isKinematic = true;

            if (grabHandRenderer != null)
                grabHandRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("Player hand not found!");
        }
    }

    void DropExtinguisher()
    {
        isGrabbed = false;
        currentlyHeldExtinguisher = null; // 🔥 Allow picking up another extinguisher
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        if (grabHandRenderer != null)
            grabHandRenderer.enabled = true;

        StopSpraying();
    }

    void StartSpraying()
    {
        if (sprayPrefab != null && !isSpraying && nozzleTransform != null)
        {
            currentSprayInstance = Instantiate(sprayPrefab, nozzleTransform.position, nozzleTransform.rotation);
            currentSprayInstance.transform.parent = nozzleTransform;
            isSpraying = true;

            if (audioSource != null && spraySound != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = spraySound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    void StopSpraying()
    {
        if (currentSprayInstance != null)
        {
            Destroy(currentSprayInstance);
            isSpraying = false;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }

    void UpdateSprayPosition()
    {
        if (currentSprayInstance != null)
        {
            currentSprayInstance.transform.position = nozzleTransform.position;
            currentSprayInstance.transform.rotation = nozzleTransform.rotation;
        }
    }

    void TryExtinguishFire()
    {
        if (Physics.Raycast(nozzleTransform.position, nozzleTransform.forward, out RaycastHit hit, sprayRange))
        {
            if (hit.collider.TryGetComponent(out Fire fire))
            {
                fire.TryExtinguish(extinguishRate * Time.deltaTime);
            }
        }
    }
}
