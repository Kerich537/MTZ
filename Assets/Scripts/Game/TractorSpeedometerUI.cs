using TMPro;
using UnityEngine;

public class TractorSpeedometerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;

    public void UpdateSpeed(float speed)
    {
        _speedText.text = ((int)speed).ToString();
    }
}
