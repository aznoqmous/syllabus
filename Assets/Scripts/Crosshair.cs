using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.localEulerAngles = transform.localEulerAngles  + Vector3.up * Time.deltaTime;
    }
}
