using UnityEngine;
using Cinemachine;

public class HighlightOnCamera : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material[] originalMaterials;
    public Material glowOutlineMaterial; // Assign in Inspector
    public CinemachineVirtualCamera targetCamera; // Assign in Inspector

    private bool isOutlined = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterials = objectRenderer.materials; // Store original materials
    }

    void Update()
    {
        if (targetCamera.Priority > 0 && !isOutlined)
        {
            EnableOutline();
        }
        else if (targetCamera.Priority == 0 && isOutlined)
        {
            DisableOutline();
        }
    }

    public void EnableOutline()
    {
        Debug.Log("✅ Glow Highlight Enabled: " + gameObject.name);

        if (!isOutlined && glowOutlineMaterial != null)
        {
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                newMaterials[i] = originalMaterials[i];
            }
            newMaterials[newMaterials.Length - 1] = glowOutlineMaterial;
            objectRenderer.materials = newMaterials;
            isOutlined = true;
        }
    }

    public void DisableOutline()
    {
        Debug.Log("❌ Glow Highlight Disabled: " + gameObject.name);
        objectRenderer.materials = originalMaterials;
        isOutlined = false;
    }
}
