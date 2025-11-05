using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    [SerializeField] Transform _gameCameraPosition;
    [SerializeField] Transform _titleCameraPosition;

    [SerializeField] bool _isPlaying = false;

    void Start()
    {
        
    }

    void Update()
    {
        Camera.main.transform.eulerAngles = Vector3.Lerp(Camera.main.transform.eulerAngles, _isPlaying ? _gameCameraPosition.eulerAngles : _titleCameraPosition.eulerAngles, Time.deltaTime * 2f);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _isPlaying ? _gameCameraPosition.position : _titleCameraPosition.position, Time.deltaTime * 2f);
    }
}
