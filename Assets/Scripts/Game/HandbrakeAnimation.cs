using UnityEngine;
using UnityEngine.Events;

public class HandbrakeAnimation : MonoBehaviour
{
    [SerializeField] private TESTTractorMovementController _tractorMovementController;
    private Animator _handbrakeAnimator;
    private bool _handbrake;

    private void Awake()
    {
        _handbrakeAnimator = GetComponent<Animator>();
    }

    public void SetHandbrake()
    {
        _handbrake = !_handbrake;
        _handbrakeAnimator.SetBool("Handbrake", _handbrake);

        _tractorMovementController.SetHandbrake(_handbrake);
    }
}
