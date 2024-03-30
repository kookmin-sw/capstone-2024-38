using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    MovementInput input;

    [SerializeField] ParticleSystem inkParticle;
    [SerializeField] Transform parentController;
    [SerializeField] Transform splatGunNozzle;
    [SerializeField] CinemachineFreeLook freeLookCamera;
    [SerializeField] Image FireButton;
    CinemachineImpulseSource impulseSource;

    void Start()
    {
        input = GetComponent<MovementInput>();
        impulseSource = freeLookCamera.GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        Vector3 angle = parentController.localEulerAngles;
        input.blockRotationPlayer = IsPointerOverUIObject(FireButton); 

        if (IsPointerOverUIObject(FireButton)) 
        {
            VisualPolish();
            input.RotateToCamera(transform);
        }

        if (IsPointerOverUIObject(FireButton)) 
            inkParticle.Play();
        else if (!IsPointerOverUIObject(FireButton))
            inkParticle.Stop();

        parentController.localEulerAngles
            = new Vector3(Mathf.LerpAngle(parentController.localEulerAngles.x, (IsPointerOverUIObject(FireButton)) ? RemapCamera(freeLookCamera.m_YAxis.Value, 0, 1, -25, 25) : 0, .3f), angle.y, angle.z);
    }

    void VisualPolish()
    {
        if (!DOTween.IsTweening(parentController))
        {
            parentController.DOComplete();
            Vector3 forward = -parentController.forward;
            Vector3 localPos = parentController.localPosition;
            parentController.DOLocalMove(localPos - new Vector3(0, 0, .2f), .03f)
                .OnComplete(() => parentController.DOLocalMove(localPos, .1f).SetEase(Ease.OutSine));

            impulseSource.GenerateImpulse();
        }

        if (!DOTween.IsTweening(splatGunNozzle))
        {
            splatGunNozzle.DOComplete();
            splatGunNozzle.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);
        }
    }

    float RemapCamera(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    bool IsPointerOverUIObject(Image image)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == image.gameObject)
                return true;
        }
        return false;
    }
}
