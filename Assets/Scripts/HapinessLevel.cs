using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapinessLevel : MonoBehaviour
{
    [SerializeField] protected Image _fillBar;
    [SerializeField] protected Color _sadColor;
    [SerializeField] protected Color _happyColor;

    private void Start()
    {
        GameManager.Instance.HapinessChanged += OnHapinessChanged;
    }

    protected void OnHapinessChanged(float hapiness)
    {
        _fillBar.rectTransform.localScale = new Vector3(hapiness, 1, 1);
        _fillBar.color = Color.Lerp(_sadColor, _happyColor, hapiness);
    }

}
