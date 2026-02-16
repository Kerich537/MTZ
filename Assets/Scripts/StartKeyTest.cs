using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StartKeyTest : MonoBehaviour
{
    [SerializeField] private TESTTractorMovementController _tractorController;
    private Animator _animator;
    private bool _ignition, _canDrive, _hoverEntered;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void NextAction()
    {
        if (!_ignition)
        {
            ChangeIgnition(true);
        }
        else
        {
            ChangeCanDrive(true);
        }
    }

    private void PreviousAction()
    {
        if (_canDrive)
        {
            ChangeCanDrive(false);
        }
        else
        {
            ChangeIgnition(false);
        }
    }

    private void ChangeIgnition(bool ignition)
    {
        _ignition = ignition;
        _tractorController.SetIgnition(_ignition);
        _animator.SetBool("Ignition", _ignition);
    }
    private void ChangeCanDrive(bool canDrive)
    {
        _canDrive = canDrive;
        _tractorController.SetCanDrive(_canDrive);
        _animator.SetBool("CanDrive", _canDrive);
    }
}
