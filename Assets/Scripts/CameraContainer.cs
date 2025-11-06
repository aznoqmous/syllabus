using UnityEngine;

public class CameraContainer : MonoBehaviour
{
    [SerializeField] Transform _gameCameraPosition;
    [SerializeField] Transform _titleCameraPosition;


    void Start()
    {
        Camera.main.transform.rotation = Game.Instance.IsPlaying ? _gameCameraPosition.rotation : _titleCameraPosition.rotation;
        Camera.main.transform.position = Game.Instance.IsPlaying ? _gameCameraPosition.position : _titleCameraPosition.position;

    }

    void Update()
    {
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Game.Instance.IsPlaying ? _gameCameraPosition.rotation : _titleCameraPosition.rotation, Time.deltaTime * 2f);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Game.Instance.IsPlaying ? _gameCameraPosition.position : _titleCameraPosition.position, Time.deltaTime * 2f);
        if (Game.Instance.IsPlaying) Camera.main.transform.LookAt(PlayerBoat.Instance.transform.position);
    }
}
