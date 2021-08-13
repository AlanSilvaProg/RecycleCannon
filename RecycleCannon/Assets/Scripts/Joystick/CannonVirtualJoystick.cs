using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class CannonVirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static CannonVirtualJoystick Instance;
    [SerializeField] Image joyBackgroundImage;
    [SerializeField] Image joyStickImage;
    Vector3 inputVector;

    private void Awake()
    {
        Instance = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joyBackgroundImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = pos.x / joyBackgroundImage.rectTransform.sizeDelta.x;
            pos.y = pos.y / joyBackgroundImage.rectTransform.sizeDelta.y;

            inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);

            if (inputVector.magnitude > 1) inputVector = inputVector.normalized;

            joyStickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (joyBackgroundImage.rectTransform.sizeDelta.x / 3), inputVector.z * (joyBackgroundImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        joyStickImage.rectTransform.anchoredPosition = inputVector;
    }

    public float HorizontalAxis() { return inputVector.x; }
    public float VerticalAxis() { return inputVector.z; }

}
