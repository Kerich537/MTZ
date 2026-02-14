using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserpanelController : MonoBehaviour
{
    [SerializeField] private MenuController _menuController;
    [SerializeField] private ButtonController[] _buttons;
    [SerializeField] private Color _normalButtonColor, _selectedButtonColor;
    private int _currentButton;
    public bool isActive = false;

    private void Start()
    {
        _buttons[_currentButton].GetComponent<Image>().color = _selectedButtonColor;
    }

    public void SelectButton(bool stepIsAbove)
    {
        if (!isActive)
            return;

        _buttons[_currentButton].GetComponent<Image>().color = _normalButtonColor;

        if (_currentButton < _buttons.Length - 1 && stepIsAbove)
            _currentButton++;
        else if (_currentButton > 0 && !stepIsAbove)
            _currentButton--;

        _buttons[_currentButton].GetComponent<Image>().color = _selectedButtonColor;
    }

    public void ActivateButton()
    {
        if (!isActive)
            return;

        _buttons[_currentButton].Activate();

    }
    public void ReturnButton()
    {
        _menuController.ChangeMenu(0);
    }
}
