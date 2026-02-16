using UnityEngine;
using UnityEngine.Events;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private string _buttonName;
    [SerializeField] private bool _holdButton;
    
    private Animator _animator;
    private bool _buttonDown;

    public UnityEvent OnButtonDownEvent;
    public UnityEvent OnButtonUpEvent;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SelectEnter()
    {
        if (_holdButton)
        {
            SetAnimation(true);
            OnButtonDownEvent.Invoke();
        }
        else
        {
            SetAnimation(!_buttonDown);

            if (_buttonDown)
                OnButtonDownEvent.Invoke();
            else
                OnButtonUpEvent.Invoke();
        }
    }

    public void SelectExit()
    {
        if (_holdButton)
        {
            SetAnimation(false);
            OnButtonUpEvent.Invoke();
        }
    }

    private void SetAnimation(bool buttonDown)
    {
        _buttonDown = buttonDown;
        _animator.SetBool(_buttonName, buttonDown);
    }
}
