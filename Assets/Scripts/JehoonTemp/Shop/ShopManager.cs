using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Tooltip("현재 소지한 돈, 자원을 관리 - MoneyManager")]
    [SerializeField] private MoneyManager moneyManager;
    [Tooltip("현재 소지한 음료, 버터를 관리 - SupplyManager")]
    [SerializeField] private SupplyManager supplyManager;
    [Tooltip("체력 관리 HealthCounter")]
    [SerializeField] private HealthCounter healthCounter;
    [Tooltip("빵 개수 관리 BreadCounter")]
    [SerializeField] private BreadCounter breadCounter;
    [Tooltip("플레이어")]
    [SerializeField] private GameObject player;

    private float curMoney;
    private int curDrink;
    private int curButter;

    [Tooltip("음료수 가격")]
    [SerializeField] private float DrinkPrice = 2.00f;
    [Tooltip("버터 가격")]
    [SerializeField] private float ButterPrice = 3.50f;
    [Tooltip("에어컨 가격")]
    [SerializeField] private float AirConditionerPrice = 10.00f;


    [Tooltip("현재 가격 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI priceText;


    #region 스탯 강화 관련 변수
    [Tooltip("최대 체력 강화 구매 버튼")]
    [SerializeField] private GameObject HealthButton;
    [Tooltip("최대 체력 강화 레벨")]
    [SerializeField] private int HealthLevel = 1;
    [Tooltip("최대 체력 강화 가격")]
    [SerializeField] private float HealthPrice = 5.00f;
    [Tooltip("현재 체력 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [Tooltip("레벨업 시 체력 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI healthUpgradeText;


    [Tooltip("빵 소지 최대치 구매 버튼")]
    [SerializeField] private GameObject BreadButton;
    [Tooltip("빵 소지 최대치 강화 레벨")]
    [SerializeField] private int BreadLevel = 1;
    [Tooltip("빵 소지 최대치 강화 가격")]
    [SerializeField] private float BreadPrice = 5.00f;
    [Tooltip("현재 빵 소지 최대치 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI breadText;
    [Tooltip("레벨업 시 빵 소지 최대치 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI breadUpgradeText;
    [Tooltip("이동속도 강화 구매 버튼")]
    [SerializeField] private GameObject SpeedButton;
    [Tooltip("이동속도 강화 레벨")]
    [SerializeField] private int SpeedLevel = 1;
    [Tooltip("이동속도 강화 가격")]
    [SerializeField] private float SpeedPrice = 5.00f;
    [Tooltip("현재 이동속도 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI speedText;
    [Tooltip("레벨업 시 이동속도 수치 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI speedUpgradeText;
    #endregion

    #region 소모품 강화 관련 변수
    [Tooltip("음료수 구매 버튼")]
    [SerializeField] private GameObject DrinkButton;
    [Tooltip("음료수 개수")]

    [SerializeField] private int DrinkEach = 1;
    [Tooltip("버터 구매 버튼")]
    [SerializeField] private GameObject ButterButton;
    [Tooltip("버터 개수")]

    [SerializeField] private int ButterEach = 1;
    #endregion

    #region 에어컨 관련 변수
    [Tooltip("에어컨 구매 버튼")]
    [SerializeField] private GameObject AirConditionerButton;
    #endregion

    void Start()
    {
        moneyManager = GetComponent<MoneyManager>();
        supplyManager = GetComponent<SupplyManager>();
        curMoney = moneyManager.GetCurrentMoney();
        curDrink = supplyManager.GetDrinkCount();
        curButter = supplyManager.GetButterCount();
        ButtonInitiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 모든 버튼을 활성화합니다.
    /// </summary>
    public void ButtonInitiate()
    {
        if(curMoney < HealthPrice || HealthLevel >= 11)
        {
            HealthButton.SetActive(false);
        }
        else
        {
            HealthButton.SetActive(true);
        }

        if(curMoney < BreadPrice || BreadLevel >= 11)
        {
            BreadButton.SetActive(false);
        }
        else
        {
            BreadButton.SetActive(true);
        }

        if(curMoney < SpeedPrice || SpeedLevel >= 5)
        {
            SpeedButton.SetActive(false);
        }
        else
        {
            SpeedButton.SetActive(true);
        }

        if(curMoney < DrinkPrice)
        {
            DrinkButton.SetActive(false);
        }
        else
        {
            DrinkButton.SetActive(true);
        }

        if(curMoney < ButterPrice)
        {
            ButterButton.SetActive(false);
        }
        else
        {
            ButterButton.SetActive(true);
        }

        if(curMoney < AirConditionerPrice)
        {
            AirConditionerButton.SetActive(false);
        }
        else
        {
            AirConditionerButton.SetActive(true);
        }
    }

    #region 레벨에 따른 가격, 능력치 세팅 함수
    /// <summary>
    /// 최대 체력 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void setHealthValue()
    {
        // 체력은 레벨별로 4 + 레벨, 최대 11레벨까지(최대치 15). 업그레이드 가격은 레벨별로 5 + 레벨 * 2.5
        if (HealthLevel >= 11)
        {
            return;
        }
        player.GetComponent<PlayerController>().SetMaxHealth(4 + HealthLevel);
        HealthPrice = 5 + HealthLevel * 2.5f;
    }
    /// <summary>
    /// 빵 소지 최대치 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void setBreadValue()
    {
        // 빵 소지 최대치는 레벨별로 5 + 레벨 * 2, 최대 11레벨까지(최대치 25). 업그레이드 가격은 레벨별로 5 + 레벨 * 2.5
        if (BreadLevel >= 11)
        {
            return;
        }
        breadCounter.SetMaxBread(5 + BreadLevel * 2);
        BreadPrice = 5 + BreadLevel * 2.5f;
    }
    /// <summary>
    /// 이동속도 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void setSpeedValue()
    {
        // 이동속도는 레벨별로 1 + 레벨 * 0.1, 최대 5레벨까지(최대치 1.5). 업그레이드 가격은 레벨별로 15 + 레벨 * 7.5
        if (SpeedLevel >= 5)
        {
            return;
        }
        player.GetComponent<PlayerController>().setPlayerSpeed(1 + SpeedLevel * 0.1f);
        SpeedPrice = 15 + SpeedLevel * 7.5f;
    }
    #endregion
}
