using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] RawImage _contour;
    [SerializeField] RawImage _image;
    [SerializeField] Color _unactiveColor;

    bool _state = false;

    public void SetState(bool state)
    {
        _state = state;
        _image.color = state ? Color.white : _unactiveColor;
    }
}
