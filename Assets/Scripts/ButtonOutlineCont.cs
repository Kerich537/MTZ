using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ButtonOutlineCont : MonoBehaviour
{
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void SetOutline(bool isEnabled) { _outline.enabled = isEnabled; }
}
