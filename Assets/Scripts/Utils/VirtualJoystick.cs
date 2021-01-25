using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour , IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        if( Input.GetMouseButtonDown(0))
        {
            BaseTransform.gameObject.SetActive(true);
            BaseTransform.transform.position = Input.mousePosition;
            Debug.LogWarning("Begin");
        }

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
        Debug.LogWarning("------------");
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
    }
}
