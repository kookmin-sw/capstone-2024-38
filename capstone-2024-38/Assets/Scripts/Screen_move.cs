using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class Screen_move : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    Image CamControlArea;
    [SerializeField] CinemachineFreeLook cam;
    string MouseXstr = "Mouse X", MouseYstr = "Mouse Y";


    void Start()
    {
        CamControlArea = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            CamControlArea.rectTransform, eventData.position, eventData.enterEventCamera, out Vector2 posOut))
        {
            cam.m_XAxis.m_InputAxisName = MouseXstr;
            cam.m_YAxis.m_InputAxisName = MouseYstr;
        }
    }

    public void OnPointerDown(PointerEventData eventData) //누를때 카메라에 값을 전달해서 회전시킴
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) // 떄면 카메라값 초기화로 카메라 회전 멈춤
    {
        cam.m_XAxis.m_InputAxisName = null;
        cam.m_XAxis.m_InputAxisValue = 0;
        cam.m_YAxis.m_InputAxisName = null;
        cam.m_YAxis.m_InputAxisValue = 0;
    }
}
