using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour , IPointerDownHandler, IPointerUpHandler , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    [SerializeField]
    private RectTransform BaseTransform;

    [SerializeField, Range(10, 150)]
    float leverRange;

    Vector2 InputDirection;
    bool isInput;

    [SerializeField]
    Player player;


    #region 조이스틱 사이드 빛

    enum E_DIRECTION
    {
        E_LEFT_UP,
        E_RIGHT_UP,
        E_LEFT_DOWN,
        E_RIGHT_DOWN,
        E_NONE,
    }

    E_DIRECTION e_Direction = E_DIRECTION.E_NONE;

    [SerializeField]
    GameObject[] SideLight;

    #endregion

    void Awake()
    {
        AllDisableSideLight();
    }

    void AllDisableSideLight()
    {
        e_Direction = E_DIRECTION.E_NONE;

        foreach (var LightObj in SideLight)
        {
            LightObj.SetActive(false);
        }
    }

    void SideLightCheck(Vector2 vecPosition)
    {
        var checkDirectionEnum = E_DIRECTION.E_NONE;

        if(vecPosition.x < 0 && vecPosition.y > 0)
        {
            checkDirectionEnum = E_DIRECTION.E_LEFT_UP;
        }
        else if(vecPosition.x > 0 && vecPosition.y > 0)
        {
            checkDirectionEnum = E_DIRECTION.E_RIGHT_UP;
        }
        else if(vecPosition.x < 0 && vecPosition.y < 0)
        {
            checkDirectionEnum = E_DIRECTION.E_LEFT_DOWN;
        }
        else if(vecPosition.x > 0 && vecPosition.y < 0)
        {
            checkDirectionEnum = E_DIRECTION.E_RIGHT_DOWN;
        }

        if(e_Direction == checkDirectionEnum || checkDirectionEnum == E_DIRECTION.E_NONE)
            return;

        if(e_Direction != E_DIRECTION.E_NONE)
            SideLight[(int)e_Direction].SetActive(false);
        
        SideLight[(int)checkDirectionEnum].SetActive(true);

        e_Direction = checkDirectionEnum;
    }

    void Update()
    {
        if(isInput)
        {
            InputControlVector();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isInput = false;

        AllDisableSideLight();

        player.Move(Vector2.zero);
        
        lever.anchoredPosition = Vector2.zero;
    }

    void ControlJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - BaseTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;

        lever.anchoredPosition = inputVector;
        InputDirection = inputVector / leverRange;

        SideLightCheck(lever.anchoredPosition);
    }

    void InputControlVector()
    {
        player.Move(InputDirection);
        player.UpdateRollMoving();
    }
}
