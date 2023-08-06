using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OnScreenJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image JoyStickParent;
    public Image Stick;


    //Input GetAxis
    public Vector2 JoyStickAxis = Vector2.zero;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 locPos;
       if(RectTransformUtility.ScreenPointToLocalPointInRectangle(JoyStickParent.rectTransform, eventData.position, eventData.pressEventCamera, out locPos))
        {
            Debug.Log($"Loc Pos: {locPos}");
            float half_w = JoyStickParent.rectTransform.rect.width / 2;
            float half_h = JoyStickParent.rectTransform.rect.height / 2;

            float x = locPos.x / half_w;
            float y= locPos.y / half_h;

            JoyStickAxis.x = x;
            JoyStickAxis.y = y;

            if(JoyStickAxis.magnitude > 1)
            {
                JoyStickAxis.Normalize();
            }

            Stick.rectTransform.localPosition = new Vector2(JoyStickAxis.x * half_w , JoyStickAxis.y * half_h);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoyStickAxis = Vector2.zero;
        Stick.rectTransform.localPosition = Vector2.zero;
    }
}
