using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;
    private int _currentMenu;

    private void Start()
    {
        ChangeMenu(0);
    }

    public void ChangeMenu(int changeMenuIndex)
    {
        _menus[_currentMenu].GetComponent<UserpanelController>().isActive = false;
        _menus[_currentMenu].SetActive(false);
        _currentMenu = changeMenuIndex;
        StartCoroutine("WaitTime");
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.1f);
        _menus[_currentMenu].SetActive(true);
        _menus[_currentMenu].GetComponent<UserpanelController>().isActive = true;
    }
}
