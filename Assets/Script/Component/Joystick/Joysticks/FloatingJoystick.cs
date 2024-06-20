using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public UnityAction OnPointerDownEvent;
    public UnityAction OnPointerUpEvent;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        OnPointerDownEvent?.Invoke();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        OnPointerUpEvent?.Invoke();
        base.OnPointerUp(eventData);
    }
}