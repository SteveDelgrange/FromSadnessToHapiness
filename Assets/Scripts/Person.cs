using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PersonType { Stranger, Friend, Enemy }

public class Person : MonoBehaviour
{
    protected PersonType _personType = PersonType.Stranger;
    public PersonType PersonType
    {
        get => _personType;
        set {
            _personType = value;
            _personImage.sprite = GameManager.Instance.GetSpriteOfPersonType(_personType);
        }
    }

    public RectTransform rectTransform;
    [SerializeField] protected Image _personImage;
    [SerializeField] protected float _chanceToBecomeEnemy = 0.1f;
    [SerializeField] protected float _chanceToBecomeFriend = 0.15f;

    void Start()
    {
        _personImage.sprite = GameManager.Instance.GetSpriteOfPersonType(_personType);
    }

    public void TryMutate(float ratioEnemiesOverFriends)
    {
        float rand = Random.Range(0f, 1f);

        if(_personType == PersonType.Stranger) {
            float adjustedChanceOfBecomingEnemy = _chanceToBecomeEnemy + (0.2f * ratioEnemiesOverFriends);
            float adjustedChanceOfBecomingFriend = _chanceToBecomeFriend + (0.2f / ratioEnemiesOverFriends);
            if (rand <= adjustedChanceOfBecomingEnemy) {
                PersonType = PersonType.Enemy;
            } else if (rand <= adjustedChanceOfBecomingEnemy + adjustedChanceOfBecomingFriend) {
                PersonType = PersonType.Friend;
            }
        } else if(_personType == PersonType.Enemy) {
            if(rand < 0.05f) {
                PersonType = PersonType.Friend;
            }
        } else if(_personType == PersonType.Friend) {
            if (rand < 0.1f) {
                PersonType = PersonType.Enemy;
            }
        }

    }

    public void Remove()
    {
        GameManager.Instance.RemovePerson(this);
    }
}
