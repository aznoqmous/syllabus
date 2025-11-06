using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] StatsUI _statsUI;
    [SerializeField] bool _isPlaying = false;
    [SerializeField] Canvas _titleCanvas;
    public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }    

    void Start()
    {
        _statsUI.gameObject.SetActive(_isPlaying);
    }

    void Update()
    {
        SetTimeScale(_targetTimeScale, Time.unscaledDeltaTime * 5f);
    }

    public void ApplySharedMaterial(Material material, MeshRenderer parentModel)
    {
        parentModel.material = material;
        foreach (Transform t in parentModel.transform)
        {
            t.GetComponent<MeshRenderer>().material = material;
        }
    }

    float _targetTimeScale = 1f;
    public void SetTargetTimeScale(float value)
    {
        _targetTimeScale = value;
    }
    public void SetTimeScale(float value, float lerp = 1f)
    {
        Time.timeScale = Mathf.Min(1f, Mathf.Lerp(Time.timeScale, value, lerp));
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void Play()
    {
        _isPlaying = true;
        _titleCanvas.gameObject.SetActive(false);
        _statsUI.gameObject.SetActive(true);
    }
}