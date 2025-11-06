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
    [SerializeField] BulletsUI _bulletsUI;
    public RawImage CrossHairImage { get { return _crossHairImage;  } }
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        transform.position = Input.mousePosition;
        _crossHairImage.transform.localScale = Vector3.Lerp(_crossHairImage.transform.localScale, Vector3.one, Time.deltaTime * 10f);

        Cursor.visible = !Game.Instance.IsPlaying;
        _bulletsUI.gameObject.SetActive(Game.Instance.IsPlaying);
        _crossHairImage.gameObject.SetActive(Game.Instance.IsPlaying);
    }

    
}
