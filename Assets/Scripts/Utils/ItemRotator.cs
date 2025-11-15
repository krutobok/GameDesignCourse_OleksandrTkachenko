using UnityEngine;
using UnityEngine.EventSystems;

public class ItemRotator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform targetToRotate;
    public float rotationSpeed = 25f;

    private bool isDragging = false;

    public void SetTarget(Transform target)
    {
        targetToRotate = target;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || targetToRotate == null)
            return;

        float rotX = eventData.delta.y * rotationSpeed * Time.deltaTime;
        float rotY = -eventData.delta.x * rotationSpeed * Time.deltaTime;

        targetToRotate.Rotate(Camera.main.transform.up, rotY, Space.World);
        targetToRotate.Rotate(Camera.main.transform.right, rotX, Space.World);
    }
}


