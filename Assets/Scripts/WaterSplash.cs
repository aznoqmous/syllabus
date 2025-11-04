using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles = new Vector3(0, Random.value * 360f, 0);
    }


    private void Erase()
    {
        Destroy(gameObject);
    }
}
