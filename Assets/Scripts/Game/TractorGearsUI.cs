using TMPro;
using UnityEngine;

public class TractorGearsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gearText;

    public void UpdateGear(string gear)
    {
        _gearText.text = gear;
    }
}
