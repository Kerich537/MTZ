using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private enum ButtonEnum
    {
        ChangeScene,
        ChangeMenu,
        Exit
    }

    [SerializeField] private ButtonEnum _button;
    [SerializeField] private MenuController __menuController;
    [SerializeField] private int _menuToChange;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private bool _orderGame;
    public static bool orderGame;

    public void Activate()
    {
        if (_button == ButtonEnum.ChangeScene)
        {
            orderGame = _orderGame;
            //SceneManager.LoadScene(_sceneIndex);
            Debug.Log(_sceneIndex + " " + orderGame);
        }
        if (_button == ButtonEnum.ChangeMenu)
        {
            __menuController.ChangeMenu(_menuToChange);
        }
        else if (_button == ButtonEnum.Exit)
            Application.Quit();
    }
}
