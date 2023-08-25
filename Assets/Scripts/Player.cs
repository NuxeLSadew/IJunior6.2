using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private const int MinCurrentHealth = 0;
    private const int MinimumMaxHealth = 10;
    private const int MaximumMaxHealth = 200;

    [SerializeField] private int _maxHealth;
    [SerializeField] private UnityEvent _onCurrentHealthChangedEvent;
    [SerializeField] private UnityEvent _onMaxHealthChangedEvent;

    private int _currentHealth;

    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (value < MinimumMaxHealth)
            {
                value = MinimumMaxHealth;
            }
            else if (value > MaximumMaxHealth)
            {
                value = MaximumMaxHealth;
            }

            _maxHealth = value;
        }
    }
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value < MinCurrentHealth)
            {
                value = MinCurrentHealth;
            }
            else if (value > MaxHealth)
            {
                value = MaxHealth;
            }

            _currentHealth = value;
        }
    }

    public event UnityAction OnCurrentHealthChangedEvent
    {
        add => _onCurrentHealthChangedEvent.AddListener(value);
        remove => _onCurrentHealthChangedEvent.RemoveListener(value);
    }

    public event UnityAction OnMaxHealthChangedEvent
    {
        add => _onCurrentHealthChangedEvent.AddListener(value);
        remove => _onCurrentHealthChangedEvent.RemoveListener(value);
    }

    private void OnValidate()
    {
        _maxHealth = Mathf.Clamp(_maxHealth, MinCurrentHealth, MaximumMaxHealth);
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int ammount)
    {
        CurrentHealth -= ammount;
        _onCurrentHealthChangedEvent.Invoke();
    }

    public void TakeHeal(int ammount)
    {
        CurrentHealth += ammount;
        _onCurrentHealthChangedEvent.Invoke();
    }

    public void IncreaseHealth(int ammount)
    {
        MaxHealth += ammount;
        _onCurrentHealthChangedEvent.Invoke();
    }

    public void DecreaseHealth(int ammount)
    {
        MaxHealth -= ammount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinCurrentHealth, MaxHealth);
        _onCurrentHealthChangedEvent.Invoke();
    }
}
