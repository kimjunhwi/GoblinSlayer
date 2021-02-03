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

    void Awake()
    {
         BaseTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        // if(Input.touchCount > 0)
        // {
        //     Touch touch =Input.GetTouch(0);

        //     if(touch.phase == TouchPhase.Began)
        //     {
        //         Debug.LogWarning("Begin");
        //     }
        // }

        if(isInput)
        {
            InputControlVector();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BaseTransform.transform.position = eventData.position;
        BaseTransform.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BaseTransform.gameObject.SetActive(false);
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
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        player.Move(Vector2.zero);

        BaseTransform.gameObject.SetActive(false);
    }

    void ControlJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - BaseTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;

        lever.anchoredPosition = inputVector;
        InputDirection = inputVector / leverRange;
    }

    void InputControlVector()
    {
        player.Move(InputDirection);
        player.UpdateRollMoving();
    }
}
