using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] Shop _shop;

    [Header("Stats")]
    public int EnemyEliminated = 0;
    public int TotalGold = 0;
    public float TimeSpent = 0;
    public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }    
    public bool IsShop { get { return _shop.ShopCanvas.gameObject.activeInHierarchy; } }

    [SerializeField] GameOverUI _gameOverCanvas;
    [SerializeField] GameOverUI _winCanvas;

    void Start()
    {
        _statsUI.gameObject.SetActive(_isPlaying);
        _titleCanvas.gameObject.SetActive(!_isPlaying);
        PlayerBoat.Instance.Ship.OnDie += GameOver;
    }

    void Update()
    {
        if (IsPlaying) TimeSpent += Time.deltaTime;
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

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GameOver()
    {
        _gameOverCanvas.UpdateStats();
        _gameOverCanvas.gameObject.SetActive(true);
        _statsUI.gameObject.SetActive(false);
        _isPlaying = false;
    }

    public void Win()
    {
        _winCanvas.UpdateStats();
        _winCanvas.gameObject.SetActive(true);
        _statsUI.gameObject.SetActive(false);
        _isPlaying = false;
    }
}