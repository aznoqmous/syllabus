using UnityEngine;
using UnityEngine.UI;

public class UpgradeTickUI : MonoBehaviour
{
    [SerializeField] Color _activeColor;
    [SerializeField] Color _unactiveColor;
    [SerializeField] RawImage _image;
    public void SetActive(bool active)
    {
        _image.color = active ? _activeColor : _unactiveColor;
    }
}
