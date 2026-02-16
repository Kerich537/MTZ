using TMPro;
using UnityEngine;

public class TractorSpeedometerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;

    public void UpdateSpeed(float speed)
    {
        _speedText.text = ((int)speed).ToString();
    }

    private void OnEnable()
    {
        _speedText.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _speedText.gameObject.SetActive(false);
    }
}
