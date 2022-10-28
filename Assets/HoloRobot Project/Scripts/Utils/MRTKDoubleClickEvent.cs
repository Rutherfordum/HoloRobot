using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class MRTKDoubleClickEvent : MonoBehaviour, IMixedRealityPointerHandler
{
    public UnityEvent OnClickDoubleEvent;
    public UnityEvent<Handedness> OnSetHandednessEvent;

    private float clickDelay = 1f;
    private float clicked = 0;
    private float clickTime = 0;

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        clicked++;
        if (clicked == 1) clickTime = Time.time;

        if (clicked > 1 && Time.time - clickTime < clickDelay)
        {
            clicked = 0;
            clickTime = 0;
            OnClickDoubleEvent?.Invoke();
            OnSetHandednessEvent?.Invoke(eventData.Pointer.Controller.ControllerHandedness);
        }
        else if (clicked > 2 || Time.time - clickTime > 1) clicked = 0;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }
}
