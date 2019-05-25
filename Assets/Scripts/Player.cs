using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum HapinessEnum { Dead, VerySad, Sad, Bof, Smile, Happy }

    [SerializeField] protected Image _playerImage;
    [SerializeField] protected HapinessEnum _hapiness;
    public HapinessEnum Hapiness
    {
        get => _hapiness;
        set {
            _hapiness = value;
            _playerImage.sprite = GetSpriteFromHapiness(_hapiness);
        }
    }

    [SerializeField] protected Sprite _deadImage;
    [SerializeField] protected Sprite _verySadImage;
    [SerializeField] protected Sprite _sadImage;
    [SerializeField] protected Sprite _bofImage;
    [SerializeField] protected Sprite _smileImage;
    [SerializeField] protected Sprite _happyImage;


    void Start()
    {
        GameManager.Instance.HapinessChanged += OnHapinessChanged;
        OnHapinessChanged(GameManager.Instance.Hapiness);
    }

    public Sprite GetSpriteFromHapiness(HapinessEnum hapiness)
    {
        switch (hapiness) {
            case HapinessEnum.Dead: return _deadImage;
            case HapinessEnum.VerySad: return _verySadImage;
            case HapinessEnum.Sad: return _sadImage;
            case HapinessEnum.Bof: return _bofImage;
            case HapinessEnum.Smile: return _smileImage;
            case HapinessEnum.Happy: return _happyImage;
            default: return null;
        }
    }

    public void OnHapinessChanged(float hapiness)
    {
        if (hapiness == 0) {
            Hapiness = HapinessEnum.Dead;
        } else if (hapiness <= 0.2f) {
            Hapiness = HapinessEnum.VerySad;
        } else if (hapiness <= 0.4f) {
            Hapiness = HapinessEnum.Sad;
        } else if (hapiness <= 0.6f) {
            Hapiness = HapinessEnum.Bof;
        } else if (hapiness <= 0.8f) {
            Hapiness = HapinessEnum.Smile;
        } else if (hapiness <= 1f) {
            Hapiness = HapinessEnum.Happy;
        }
    }
}
