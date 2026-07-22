using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoldDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool buttonHeld = false;

    public bool IsButtonHeld
    {
        get { return buttonHeld; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Button is pressed, set the boolean to true
        buttonHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Button is released, set the boolean to false
        buttonHeld = false;
    }
}
