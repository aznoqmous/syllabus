using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Transform _handle;
    [SerializeField] float _timeBeforeBoss;
    [SerializeField] Ship _bossShipPrefab;

    [SerializeField] ParticleSystem _rainParticles;
    [SerializeField] Transform _directionalLightTransform;
    [SerializeField] Transform _bossDirectionalLightTransform;
    Ship _bossShip;


    void Update()
    {
        if (!Game.Instance.IsPlaying) return;
        if (_bossShip)
        {
            _directionalLightTransform.rotation = Quaternion.Lerp(_directionalLightTransform.rotation, _bossDirectionalLightTransform.rotation, Time.deltaTime);
            _slider.value = _bossShip.CurrentHp / _bossShip.MaxHp;
        }
        else
        {
            if (_slider.value >= 1)
            {
                _bossShip = Instantiate(_bossShipPrefab, PlayerBoat.Instance.transform.position + Vector3.forward * 200f, Quaternion.identity);
                _bossShip.OnDie += Game.Instance.Win;
                _handle.gameObject.SetActive(false);
                _rainParticles.gameObject.SetActive(true);
            }
            else
            {
                _slider.value = Game.Instance.TimeSpent / _timeBeforeBoss;
            }
        }
    }
}
