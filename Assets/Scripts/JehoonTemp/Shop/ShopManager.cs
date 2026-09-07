using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Profiling;

public class ShopManager : MonoBehaviour
{
    [Tooltip("패널 출력 여부 관리 - OnoffManager")]
    [SerializeField] private OnOffManager onOffManager;
    [Tooltip("플레이어")]
    [SerializeField] private GameObject player;
    [Tooltip("플레이어 웨폰 헨들러")]
    [SerializeField] private WeaponHandler wHandler;

    [Tooltip("음료수 가격")]
    [SerializeField] private float drinkPrice = 25.00f;
    [Tooltip("버터 가격")]
    [SerializeField] private float butterPrice = 50.00f;
    [Tooltip("에어컨 가격")]
    [SerializeField] private float airConditionerPrice = 5600.00f;


    [Tooltip("현재 돈 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI curMoneyText;


    #region 스탯 강화 관련 변수
    [Tooltip("최대 체력 강화 구매 버튼")]
    [SerializeField] private Button healthButton;
    [Tooltip("최대 체력 강화 레벨")]
    [SerializeField] private int healthLevel = 1;
    [Tooltip("최대 체력 강화 가격")]
    [SerializeField] private float healthPrice = 50.00f;
    [Tooltip("최대 체력 강화 가격 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI healthPriceText;
    [Tooltip("현재 체력 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [Tooltip("레벨업 시 체력 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI healthUpgradeText;


    [Tooltip("빵 소지 최대치 구매 버튼")]
    [SerializeField] private Button breadButton;
    [Tooltip("빵 소지 최대치 강화 레벨")]
    [SerializeField] private int breadLevel = 1;
    [Tooltip("빵 소지 최대치 강화 가격")]
    [SerializeField] private float breadPrice = 50.00f;
    [Tooltip("빵 소지 최대치 강화 가격 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI breadPriceText;
    [Tooltip("현재 빵 소지 최대치 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI breadText;
    [Tooltip("레벨업 시 빵 소지 최대치 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI breadUpgradeText;

    [Tooltip("이동속도 강화 구매 버튼")]
    [SerializeField] private Button speedButton;
    [Tooltip("이동속도 강화 레벨")]
    [SerializeField] private int speedLevel = 1;
    [Tooltip("이동속도 강화 가격")]
    [SerializeField] private float speedPrice = 50.00f;
    [Tooltip("이동속도 강화 가격 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI speedPriceText;
    [Tooltip("현재 이동속도 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI speedText;
    [Tooltip("레벨업 시 이동속도 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI speedUpgradeText;
    #endregion

    #region 소모품 강화 관련 변수
    [Tooltip("음료수 구매 버튼")]
    [SerializeField] private Button drinkButton;

    [Tooltip("음료수 개수 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI drinkEachText;
    [Tooltip("버터 구매 버튼")]
    [SerializeField] private Button butterButton;

    [Tooltip("버터 개수 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI butterEachText;
    #endregion

    #region 에어컨 관련 변수
    [Tooltip("에어컨 구매 버튼")]
    [SerializeField] private Button airConditionerButton;
    #endregion

    public UnityEvent<int> onDrinkChanged;
    public UnityEvent<int> onButterChanged;
    public UnityEvent onAirConditionerPurchased;

    void Start()
    {
        SetValueText();
        ButtonInitiate();
    }

    /// <summary>
    /// 현재 소지한 돈, 전체 텍스트 값 세팅
    /// </summary>
    public void SetValueText()
    {
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        SetHealthValueText();
        SetBreadValueText();
        SetSpeedValueText();
        drinkEachText.text = Managers.Player.PlayerStat.MonsterAmount.ToString();
        butterEachText.text = Managers.Player.PlayerStat.ButterAmount.ToString();
    }
    /// <summary>
    /// 현재 소지한 체력 텍스트 값 세팅
    /// </summary>
    public void SetHealthValueText()
    {
        healthText.text = Managers.Player.PlayerStat.MaxHp.ToString();
        healthUpgradeText.text = (Managers.Player.PlayerStat.MaxHp + 1).ToString();
        healthPriceText.text = "€ " + healthPrice.ToString("F0");

        if (healthLevel >= 11)
        {
            healthPriceText.text = "MAX";
            healthButton.interactable = false;
        }
    }
    /// <summary>
    /// 현재 소지한 빵 소지 최대치 텍스트 값 세팅
    /// </summary>
    public void SetBreadValueText()
    {
        breadText.text = Managers.Player.PlayerStat.MaxBread.ToString();
        breadUpgradeText.text = (Managers.Player.PlayerStat.MaxBread + 2).ToString();
        breadPriceText.text = "€ " + breadPrice.ToString("F0");

        if (breadLevel >= 11)
        {
            breadPriceText.text = "MAX";
            breadButton.interactable = false;
        }
    }
    /// <summary>
    /// 현재 소지한 이동속도 텍스트 값 세팅
    /// </summary>
    public void SetSpeedValueText()
    {
        speedText.text = (Managers.Player.PlayerController.WalkSpeed / 10).ToString("F1");
        speedUpgradeText.text = (Managers.Player.PlayerController.WalkSpeed / 10 + 0.1f).ToString("F1");
        speedPriceText.text = "€ " + speedPrice.ToString("F0");

        if (speedLevel >= 6)
        {
            speedPriceText.text = "MAX";
            speedButton.interactable = false;
            return;
        }
    }


    /// <summary>
    /// 조건에 맞춰 초기 버튼 활성화
    /// </summary>
    public void ButtonInitiate()
    {
        // Debug.Log("ButtonInitiate() 호출");
        if (Managers.Money.Money < healthPrice || healthLevel >= 11)
        {
            healthButton.interactable = false;
        }
        else
        {
            healthButton.interactable = true;
        }

        if (Managers.Money.Money < breadPrice || breadLevel >= 11)
        {
            breadButton.interactable = false;
        }
        else
        {
            breadButton.interactable = true;
        }

        if (Managers.Money.Money < speedPrice || speedLevel >= 6)
        {
            speedButton.interactable = false;
        }
        else
        {
            speedButton.interactable = true;
        }

        if (Managers.Money.Money < drinkPrice)
        {
            drinkButton.interactable = false;
        }
        else
        {
            drinkButton.interactable = true;
        }

        if (Managers.Money.Money < butterPrice)
        {
            butterButton.interactable = false;
        }
        else
        {
            butterButton.interactable = true;
        }

        if (Managers.Money.Money < airConditionerPrice)
        {
            airConditionerButton.interactable = false;
        }
        else
        {
            airConditionerButton.interactable = true;
        }
    }

    #region 레벨에 따른 가격, 능력치 세팅 함수
    /// <summary>
    /// 최대 체력 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void SetHealthValue()
    {
        // 체력은 레벨별로 5 + 레벨 * 1, 최대 11레벨까지(최대치 15). 업그레이드 가격은 레벨별로 5 + 레벨 * 2.5
        if (healthLevel >= 11)
        {
            SetHealthValueText();
            ButtonInitiate();
            return;
        }
        Managers.Player.AcquireMaxHp(1);

        Managers.Money.Money -= healthPrice;
        healthLevel += 1;

        healthPrice = 100 + (healthLevel - 1) * 25f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        SetHealthValueText();
        ButtonInitiate();
    }
    /// <summary>
    /// 빵 소지 최대치 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void SetBreadValue()
    {
        // 빵 소지 최대치는 레벨별로 5 + 레벨 * 2, 최대 11레벨까지(최대치 25). 업그레이드 가격은 레벨별로 5 + 레벨 * 2.5
        if (breadLevel >= 11)
        {
            SetBreadValueText();
            ButtonInitiate();
            return;
        }
        //플레이어 빵 최대 갯수 증가
        Managers.Player.PlayerStat.MaxBread += 2;
        Managers.Player.PlayerStat.MaxBread += 2;

        //UI에 표시 글 수정
        Managers.Money.Money -= breadPrice;
        breadLevel += 1;

        breadPrice = 100 + (breadLevel - 1) * 25f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        SetBreadValueText();
        ButtonInitiate();
    }
    public void SetSpeedValue()
    {
        // 빵 소지 최대치는 레벨별로 5 + 레벨 * 2, 최대 11레벨까지(최대치 25). 업그레이드 가격은 레벨별로 5 + 레벨 * 2.5
        if (speedLevel >= 6)
        {
            SetSpeedValueText();
            ButtonInitiate();
            return;
        }
        //플레이어 빵 최대 갯수 증가
        Managers.Player.PlayerController.WalkSpeed += 1f;

        //UI에 표시 글 수정
        Managers.Money.Money -= speedPrice;
        speedLevel += 1;

        speedPrice = 50 + (speedLevel - 1) * 25f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        SetSpeedValueText();
        ButtonInitiate();
    }

    public void SetDrinkValue()
    {
        drinkEachText.text = Managers.Player.PlayerStat.MonsterAmount.ToString();
    }

    public void AddDrinkValue()
    {
        Managers.Money.Money -= drinkPrice;
        Managers.Player.PlayerStat.MonsterAmount += 1;
        int drinkCount = Managers.Player.PlayerStat.MonsterAmount;
        onDrinkChanged.Invoke(drinkCount);
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        drinkEachText.text = Managers.Player.PlayerStat.MonsterAmount.ToString();
        ButtonInitiate();
    }

    public void SetButterValue()
    {
        butterEachText.text = Managers.Player.PlayerStat.ButterAmount.ToString();
    }

    public void AddButterValue()
    {
        Managers.Money.Money -= butterPrice;
        Managers.Player.PlayerStat.ButterAmount += 1;
        int butterCount = Managers.Player.PlayerStat.ButterAmount;
        onButterChanged.Invoke(butterCount);
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        butterEachText.text = Managers.Player.PlayerStat.ButterAmount.ToString();
        ButtonInitiate();
    }

    public void SetMoneyValue()
    {
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
    }

    public void SetAirConditionerValue()
    {
        Managers.Money.Money -= airConditionerPrice;
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F0");
        ButtonInitiate();
        onAirConditionerPurchased.Invoke();
    }
    #endregion

    #region 상점 활성화시 다른 오브젝트 멈춤, 닫으면 재개
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    #endregion
}
