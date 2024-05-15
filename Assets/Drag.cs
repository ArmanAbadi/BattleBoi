using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image thisImage;
    public Vector3 startPosition;
    public Canvas CanvasTop;
    public Canvas CanvasBot;

    void Start()
    {
        startPosition = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(CanvasTop.transform, false);
        thisImage.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(CanvasBot.transform, false);
        transform.position = startPosition;
        thisImage.raycastTarget = true;
    }
}
