using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class HealthView : MonoBehaviour
{
    private const string IncreasedHealthStateHexColor = "#008C005C";
    private const string DecreasedHealthStateHexColor = "#8C00005C";

    [SerializeField] private Player _player;
    [SerializeField] private Slider _backgroundBar;
    [SerializeField] private Slider _foregroundBar;
    [SerializeField] private TextMeshProUGUI _maxHealthNumber;
    [SerializeField] private TextMeshProUGUI _currentHealthNumber;

    private float _curreentHealthRelation;
    private float _lastCurrentHealthRelation;
    private Image _backgroundBarImage;
    private Color _increaseHealthStateColor;
    private Color _decreaseHealthStateColor;
    private LastHealthChange _lastHealthChange;
    private TweenerCore<float, float, FloatOptions> _backgroundBarChangeTweener;
    private TweenerCore<float, float, FloatOptions> _foregroundBarChangeTweener;

    private void Awake()
    {
        _lastCurrentHealthRelation = CalculateCurrentHealthRelation();
        _backgroundBarImage = _backgroundBar.GetComponentInChildren<Image>();

        if (ColorUtility.TryParseHtmlString(IncreasedHealthStateHexColor, out Color healedColor))
        {
            _increaseHealthStateColor = healedColor;
        }

        if (ColorUtility.TryParseHtmlString(DecreasedHealthStateHexColor, out Color damagedColor))
        {
            _decreaseHealthStateColor = damagedColor;
        }

        _backgroundBar.value = _foregroundBar.value = _lastCurrentHealthRelation;
        _maxHealthNumber.text = _player.MaxHealth.ToString();
        _currentHealthNumber.text = _player.CurrentHealth.ToString();

        _player.OnCurrentHealthChangedEvent += ChangeHealthBars;
    }

    private void ChangeBackgroundBar()
    {
        float fillingTime = 0.05f;
        float desolationTime = 0.8f;

        _backgroundBarChangeTweener.Kill();

        switch (_lastHealthChange)
        {
            case (LastHealthChange.Increase):
                _backgroundBarChangeTweener = _backgroundBar.DOValue(_curreentHealthRelation, fillingTime);
                break;

            case (LastHealthChange.Decrease):
                _backgroundBarChangeTweener = _backgroundBar.DOValue(_curreentHealthRelation, desolationTime);
                break;
        }
    }

    private void ChangeForegroundBar()
    {
        float fillingTime = 0.8f;
        float desolationTime = 0.05f;

        _foregroundBarChangeTweener.Kill();

        switch (_lastHealthChange)
        {
            case (LastHealthChange.Increase):
                _foregroundBarChangeTweener = _foregroundBar.DOValue(_curreentHealthRelation, fillingTime);
                break;

            case (LastHealthChange.Decrease):
                _foregroundBarChangeTweener = _foregroundBar.DOValue(_curreentHealthRelation, desolationTime);
                break;
        }
    }

    private void ChangeHealthBars()
    {
        _curreentHealthRelation = CalculateCurrentHealthRelation();

        _currentHealthNumber.text = _player.CurrentHealth.ToString();
        _maxHealthNumber.text = _player.MaxHealth.ToString();

        if (_curreentHealthRelation > _lastCurrentHealthRelation)
        {
            _lastHealthChange = LastHealthChange.Increase;
            _backgroundBarImage.color = _increaseHealthStateColor;
        }
        else if (_curreentHealthRelation < _lastCurrentHealthRelation)
        {
            _lastHealthChange = LastHealthChange.Decrease;
            _backgroundBarImage.color = _decreaseHealthStateColor;
        }

        ChangeBackgroundBar();
        ChangeForegroundBar();

        _lastCurrentHealthRelation = _curreentHealthRelation;
    }

    private float CalculateCurrentHealthRelation()
    {
        float currentHealthRealtion = (float)_player.CurrentHealth / _player.MaxHealth;
        return currentHealthRealtion;
    }

    private enum LastHealthChange
    {
        Increase,
        Decrease
    }
}
