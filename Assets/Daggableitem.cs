using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private Transform originalParent; // Store original parent

    void Start()
    {
        originalParent = transform.parent; // Save the initial parent
    }

    public void ResetPosition()
    {
        transform.SetParent(originalParent); // Move back to the original parent
        transform.localPosition = Vector3.zero; // Reset position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        image.raycastTarget = true;
    }
}
