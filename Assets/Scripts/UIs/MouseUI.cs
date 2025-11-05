using UnityEngine;

public class MouseUI : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
