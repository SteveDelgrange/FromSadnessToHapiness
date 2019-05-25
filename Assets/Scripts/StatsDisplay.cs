using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _turnsText;
    [SerializeField] protected TextMeshProUGUI _strangersText;
    [SerializeField] protected TextMeshProUGUI _friendsText;
    [SerializeField] protected TextMeshProUGUI _enemiesText;

    void Start()
    {
        GameManager.Instance.TurnNumberChanged += OnTurnsChanged;
        GameManager.Instance.StrangerNumberChanged += OnTStrangersChanged;
        GameManager.Instance.FriendsNumberChanged += OnFriendsChanged;
        GameManager.Instance.EnemiesNumberChanged += OnEnemiesChanged;
    }

    protected void OnTurnsChanged(int turns)
    {
        _turnsText.text = turns.ToString();
    }

    protected void OnTStrangersChanged(int strangers)
    {
        _strangersText.text = strangers.ToString();
    }

    protected void OnFriendsChanged(int friends)
    {
        _friendsText.text = friends.ToString();
    }

    protected void OnEnemiesChanged(int enemies)
    {
        _enemiesText.text = enemies.ToString();
    }
}
