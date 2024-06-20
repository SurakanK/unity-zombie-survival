using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserMain : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _textKillAmount;
    [SerializeField] private TextAmountTween _textMoney;
    [SerializeField] private TextMeshProUGUI _textLevel;

    [Header("Progress")]
    [SerializeField] private ProgressBarTween _expProgress;

    public BaseCharacter userArmy { get; set; }

    private HealthBar[] _healthBars;

    private int _exp;
    private int _level;
    private int _killAmount;
    private int _moneyAmount;

    public int MaxExp;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            _textLevel.text = _level.ToString();

            MaxExp = CalculateMaxExp();
            Exp = 0;
        }
    }

    public int Exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            var progress = (float)_exp / (float)MaxExp;
            _expProgress.Value = progress;

            if (progress >= 1)
            {
                Level += 1;
            }
        }
    }

    public int KillAmount
    {
        get { return _killAmount; }
        set
        {
            _killAmount = value;
            _textKillAmount.text = GameUtils.AmountFormat(_killAmount);
        }
    }

    public int MoneyAmount
    {
        get { return _moneyAmount; }
        set
        {
            _moneyAmount = value;
            _textMoney.Value = _moneyAmount;
        }
    }

    private static UserMain _instance;
    public static UserMain Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserMain>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Level = 1;
        MoneyAmount = 0;
        KillAmount = 0;
    }

    public void Initialize(BaseCharacter baseCharacter)
    {
        userArmy = baseCharacter;
        _healthBars = FindObjectsOfType<HealthBar>();
    }

    public void UpdateHealth(float value, float health, float maxHealth)
    {
        foreach (var HealthBar in _healthBars)
        {
            HealthBar.UpdateHealth(value, health, maxHealth);
        }
    }

    private int CalculateMaxExp()
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(1.5f, Level - 1));
    }
}