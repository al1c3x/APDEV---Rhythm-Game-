using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnScreenStickParent : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image hitbox;
    public OnScreenJoyStick joystick;

    public void OnDrag(PointerEventData eventData)
    {
        joystick.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 locPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hitbox.rectTransform, eventData.position, eventData.pressEventCamera, out locPos))
        {
            joystick.gameObject.SetActive(true);
            RectTransform rTrans = joystick.gameObject.GetComponent<RectTransform>() ;

            rTrans.localPosition = locPos;
            joystick.OnPointerDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.gameObject.SetActive(false);
        joystick.OnPointerUp(eventData);
    }
}
