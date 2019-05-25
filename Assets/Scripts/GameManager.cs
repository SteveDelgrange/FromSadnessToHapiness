using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    protected static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance;
    }

    public Sprite strangerImage;
    public Sprite friendImage;
    public Sprite enemyImage;

    [SerializeField] protected RectTransform _gameZone;
    [SerializeField] protected RectTransform _player;
    [SerializeField] protected GameObject _personPrefab;
    [SerializeField] protected GameObject _lostScreen;
    [SerializeField] protected GameObject _winScreen;

    [SerializeField] protected float _margin = 10f;
    [SerializeField] [Range(0, 1)] protected float _hapinessAtStart = 0.3f;
    [SerializeField] protected float _friendValue = 5f;
    [SerializeField] protected float _enemyValue = 5f;

    protected float _hapiness;
    public float Hapiness
    {
        get => _hapiness;
        set {
            _hapiness = Mathf.Clamp01(value);
            HapinessChanged?.Invoke(_hapiness);
            if (_hapiness == 0) {
                Lost();
            } else if (_hapiness == 1) {
                Won();
            }
        }
    }
    public System.Action<float> HapinessChanged;

    protected int _turnNumber = 0;
    public int TurnNumber
    {
        get => _turnNumber;
        set {
            _turnNumber = value;
            TurnNumberChanged?.Invoke(_turnNumber);
        }
    }
    public System.Action<int> TurnNumberChanged;

    protected int _strangerNumber = 0;
    public int StrangerNumber
    {
        get => _strangerNumber;
        set {
            _strangerNumber = value;
            StrangerNumberChanged?.Invoke(_strangerNumber);
        }
    }
    public System.Action<int> StrangerNumberChanged;

    protected int _friendsNumber = 0;
    public int FriendsNumber
    {
        get => _friendsNumber;
        set {
            _friendsNumber = value;
            FriendsNumberChanged?.Invoke(_friendsNumber);
        }
    }
    public System.Action<int> FriendsNumberChanged;

    protected int _enemiesNumber = 0;
    public int EnemiesNumber
    {
        get => _enemiesNumber;
        set {
            _enemiesNumber = value;
            EnemiesNumberChanged?.Invoke(_enemiesNumber);
        }
    }
    public System.Action<int> EnemiesNumberChanged;

    protected List<Person> _people;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _people = new List<Person>();
        Hapiness = _hapinessAtStart;
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }

    void Lost()
    {
        foreach(Person p in _people) {
            Destroy(p.gameObject);
        }
        _people.Clear();
        _lostScreen.SetActive(true);
    }

    void Won()
    {
        _winScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void NextTurn()
    {
        TurnNumber++;
        int friends = 0;
        int enemies = 0;
        List<Person> neutrals = new List<Person>();
        foreach(Person p in _people) {
            if(p.PersonType == PersonType.Friend) {
                friends++;
            } else if(p.PersonType == PersonType.Enemy) {
                enemies++;
            } else {
                neutrals.Add(p);
            }
        }
        float ratioEnemiesOverFriends = 0;
        if (friends == 0) {
            ratioEnemiesOverFriends = enemies > 0 ? 1 : 0.1f; 
        } else {
            ratioEnemiesOverFriends = enemies / (float)friends;
        }
        float change = ((friends * _friendValue) - (enemies * _enemyValue)) * 0.01f;
        foreach(Person p in _people) {
            p.TryMutate(ratioEnemiesOverFriends);
        }
        Hapiness += change;
    }

    public void AddStranger()
    {
        if (_hapiness == 0) {
            return;
        }

        NextTurn();

        if (_hapiness == 0) {
            return;
        }

        Person newPerson = Instantiate(_personPrefab, _gameZone).GetComponent<Person>();
        SearchFreePlace(newPerson.rectTransform);
        _people.Add(newPerson);
        RefreshNumbersOfPeople();
    }

    protected void RefreshNumbersOfPeople()
    {
        int tmpFriends = 0;
        int tmpEnemies = 0;
        int tmpStrangers = 0;
        foreach (Person p in _people) {
            if (p.PersonType == PersonType.Friend) {
                tmpFriends++;
            } else if (p.PersonType == PersonType.Enemy) {
                tmpEnemies++;
            } else {
                tmpStrangers++;
            }
        }
        EnemiesNumber = tmpEnemies;
        FriendsNumber = tmpFriends;
        StrangerNumber = tmpStrangers;
    }

    public void RemovePerson(Person personToRemove)
    {
        _people.Remove(personToRemove);
        Destroy(personToRemove.gameObject);

        NextTurn();

        RefreshNumbersOfPeople();
    }

    protected void SearchFreePlace(RectTransform rectTransform)
    {
        bool foundFreeZone = false;
        float minX = _gameZone.rect.xMin + _margin;
        float maxX = _gameZone.rect.xMax - _margin;
        float minY = _gameZone.rect.yMin + _margin;
        float maxY = _gameZone.rect.yMax - _margin;

        int triesRemaining = 100;

        while (!foundFreeZone && triesRemaining > 0) {
            rectTransform.localPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            bool overlap = false;
            if (!rectTransform.rect.Overlaps(_player.rect)) {
                foreach(Person p in _people) {
                    if (rectTransform.rect.Overlaps(p.rectTransform.rect)) {
                        overlap = true;
                        break;
                    }
                }
            } else {
                overlap = true;
            }
            if (!overlap) {
                foundFreeZone = true;
            }
            triesRemaining--;
        }
    }

    public Sprite GetSpriteOfPersonType(PersonType personType)
    {
        switch (personType) {
            case PersonType.Stranger: return strangerImage;
            case PersonType.Friend: return friendImage;
            case PersonType.Enemy: return enemyImage;
            default: return null;
        }
    }

}
