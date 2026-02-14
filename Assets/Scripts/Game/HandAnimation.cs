using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionProperty _trigger, _grip;

    private Animator _handAnimator;

    private void Awake()
    {
        _handAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float triggerValue = _trigger.action.ReadValue<float>();
        float gripValue = _grip.action.ReadValue<float>();

        if (_handAnimator == null)
            return;

        _handAnimator.SetFloat("Trigger", triggerValue);
        _handAnimator.SetFloat("Grip", gripValue);
    }

}
