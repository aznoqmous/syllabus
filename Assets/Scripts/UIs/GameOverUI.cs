using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timeSpentTmp;
    [SerializeField] TextMeshProUGUI _enemyEliminatedTmp;
    [SerializeField] TextMeshProUGUI _goldEarnedTmp;

    public void UpdateStats()
    {
        TimeSpan t = TimeSpan.FromSeconds(Game.Instance.TimeSpent);

        string humanReadableTime = string.Format("{1:D2}m:{2:D2}s",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
        _timeSpentTmp.text = humanReadableTime;
        _enemyEliminatedTmp.text = Game.Instance.EnemyEliminated.ToString();
        _goldEarnedTmp.text = Game.Instance.TotalGold.ToString();
    }
}
