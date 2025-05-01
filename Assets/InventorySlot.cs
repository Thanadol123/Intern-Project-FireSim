using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private bool isCorrectlyPlaced = false;

    [SerializeField] private GameObject correctItem;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;

            isCorrectlyPlaced = CheckIfCorrect(draggableItem);

            if (isCorrectlyPlaced)
            {
                Task3.Instance.ObjectPlacedCorrectly();
            }
        }
    }

    private bool CheckIfCorrect(DraggableItem item)
    {
        if (item.gameObject == correctItem)
        {
            Debug.Log(item.gameObject.name + " placed correctly!");
            return true;
        }

        Debug.Log(item.gameObject.name + " placed incorrectly!");
        return false;
    }
}
