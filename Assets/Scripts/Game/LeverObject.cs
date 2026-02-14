using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LeverObject : MonoBehaviour
{
    private XRGrabInteractable _xRGrabInteractable;

    private void Awake()
    {
        _xRGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ReleaseLever()
    {
        _xRGrabInteractable.enabled = false;
        _xRGrabInteractable.enabled = true;
    }
}
