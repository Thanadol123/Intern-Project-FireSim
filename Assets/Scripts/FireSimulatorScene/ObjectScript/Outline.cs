using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material[] originalMaterials;
    public Material outlineMaterial; // Assign this in the Inspector
    private bool isOutlined = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterials = objectRenderer.sharedMaterials; // Store original materials
    }

    public void EnableOutline()
    {
        if (!isOutlined && outlineMaterial != null)
        {
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                newMaterials[i] = originalMaterials[i]; // Keep original materials
            }
            newMaterials[newMaterials.Length - 1] = outlineMaterial; // Add outline on top
            objectRenderer.materials = newMaterials;
            isOutlined = true;
        }
    }

    public void DisableOutline()
    {
        if (isOutlined)
        {
            objectRenderer.materials = originalMaterials; // Restore original materials
            isOutlined = false;
        }
    }
}
