using UnityEngine;
using UnityEngine.UI;

public class InventoryReset : MonoBehaviour
{
    public Button resetButton;
    private DraggableItem[] draggableItems;
    private InventorySlot[] inventorySlots; // ✅ Reference to all inventory slots

    void Start()
    {
        draggableItems = Object.FindObjectsByType<DraggableItem>(FindObjectsSortMode.None);
        inventorySlots = Object.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);

        resetButton.onClick.AddListener(ResetInventory);
    }

    public void ResetInventory()
    {
        Debug.Log("🔄 Resetting Inventory and Task 3!");

        // ✅ Reset draggable item positions
        foreach (DraggableItem item in draggableItems)
        {
            item.ResetPosition();
        }

        // ✅ Clear inventory slots (remove children)
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                foreach (Transform child in slot.transform)
                {
                    Destroy(child.gameObject); // Remove dropped objects
                }
            }
        }

        // ✅ Reset Task 3 State
        if (Task3.Instance != null)
        {
            Task3.Instance.ResetTask();
        }

        Debug.Log("✅ Inventory and Task 3 fully reset!");
    }
}
