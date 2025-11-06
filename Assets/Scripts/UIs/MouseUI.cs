using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    public static MouseUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] RawImage _crossHairImage;
    public RawImage CrossHairImage { get { return _crossHairImage;  } }
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        transform.position = Input.mousePosition;
        _crossHairImage.transform.localScale = Vector3.Lerp(_crossHairImage.transform.localScale, Vector3.one, Time.deltaTime * 10f);
    }

    
}
