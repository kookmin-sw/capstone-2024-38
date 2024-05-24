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
    public float rotationSpeed = 360.0f;

    void Start()
    {
        CamControlArea = GetComponent<Image>();
        cam = FindObjectOfType<CinemachineFreeLook>();
        cam.m_XAxis.m_MaxSpeed = rotationSpeed;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cam.m_XAxis.m_InputAxisName = null;
        cam.m_XAxis.m_InputAxisValue = 0;
        cam.m_YAxis.m_InputAxisName = null;
        cam.m_YAxis.m_InputAxisValue = 0;
    }
}
