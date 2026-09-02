using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Profiling;

public class ShopManager : MonoBehaviour
{
    [Tooltip("현재 소지한 음료, 버터를 관리 - SupplyManager")]
    [SerializeField] private SupplyManager supplyManager;
    [Tooltip("체력 관리 HealthCounter")]
    [SerializeField] private HealthCounter healthCounter;
    [Tooltip("빵 개수 관리 BreadCounter")]
    [SerializeField] private BreadCounter breadCounter;
    [Tooltip("플레이어")]
    [SerializeField] private GameObject player;
    [Tooltip("파워업 적용 도중 이속 업그레이드 시 복귀 이동속도 수정용")]
    [SerializeField] private PowerUpManager powerUpManager;

    private int curDrink;
    private int curButter;

    [Tooltip("음료수 가격")]
    [SerializeField] private float drinkPrice = 2.00f;
    [Tooltip("버터 가격")]
    [SerializeField] private float butterPrice = 3.50f;
    [Tooltip("에어컨 가격")]
    [SerializeField] private float airConditionerPrice = 10.00f;


    [Tooltip("현재 돈 Text")]
    [SerializeField] private TMPro.TextMeshProUGUI curMoneyText;


    #region 스탯 강화 관련 변수
    [Tooltip("최대 체력 강화 구매 버튼")]
    [SerializeField] private Button healthButton;
    [Tooltip("최대 체력 강화 레벨")]
    [SerializeField] private int healthLevel = 1;
    [Tooltip("최대 체력 강화 가격")]
    [SerializeField] private float healthPrice = 5.00f;
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
    [SerializeField] private float breadPrice = 5.00f;
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
    [SerializeField] private float speedPrice = 22.50f;
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

    public UnityEvent<float> InitialSpeedChanged;
    public UnityEvent<int> onDrinkChanged;
    public UnityEvent<int> onButterChanged;
    public UnityEvent onAirConditionerPurchased;

    void Start()
    {
        GetCurrentValues();
        //InitValueText();
        SetValueText();
        ButtonInitiate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 값 가져오고 초기 세팅
    /// <summary>
    /// 현재 소지한 돈, 음료, 버터 값 가져오기
    /// </summary>
    public void GetCurrentValues()
    {
        curDrink = supplyManager.GetDrinkCount();
        curButter = supplyManager.GetButterCount();
    }
    /// <summary>
    /// 현재 소지한 돈, 전체 텍스트 값 세팅
    /// </summary>
    public void SetValueText()
    {
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        SetHealthValueText();
        SetBreadValueText();
        SetSpeedValueText();
        drinkEachText.text = curDrink.ToString();
        butterEachText.text = curButter.ToString();
    }
    /// <summary>
    /// 현재 소지한 체력 텍스트 값 세팅
    /// </summary>
    public void SetHealthValueText()
    {
        healthText.text = player.GetComponent<PlayerController>().GetMaxHealth().ToString();
        healthUpgradeText.text = (player.GetComponent<PlayerController>().GetMaxHealth() + 1).ToString();
        healthPriceText.text = "€ " + healthPrice.ToString("F2");

        if (healthLevel >= 11)
        {
            healthText.text = 15.ToString();
            healthUpgradeText.text = 15.ToString();
            healthPriceText.text = "MAX";
            healthButton.interactable = false;
        }
    }
    /// <summary>
    /// 현재 소지한 빵 소지 최대치 텍스트 값 세팅
    /// </summary>
    public void SetBreadValueText()
    {
        breadText.text = breadCounter.GetMaxBread().ToString();
        breadUpgradeText.text = (breadCounter.GetMaxBread() + 2).ToString();
        breadPriceText.text = "€ " + breadPrice.ToString("F2");

        if (breadLevel >= 11)
        {
            breadText.text = 25.ToString();
            breadUpgradeText.text = 25.ToString();
            breadPriceText.text = "MAX";
            breadButton.interactable = false;
        }
    }
    /// <summary>
    /// 현재 소지한 이동속도 텍스트 값 세팅
    /// </summary>
    public void SetSpeedValueText()
    {
        speedText.text = (1 + (speedLevel - 1) * 0.1f).ToString("F1");
        speedUpgradeText.text = (1 + speedLevel * 0.1f).ToString("F1");
        speedPriceText.text = "€ " + speedPrice.ToString("F2");

        if (speedLevel >= 6)
        {
            speedText.text = 1.5.ToString();
            speedUpgradeText.text = 1.5.ToString();
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
    #endregion

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
            Debug.Log("Max HP: " + player.GetComponent<PlayerController>().GetMaxHealth());
            Debug.Log("Current HP: " + player.GetComponent<PlayerController>().GetCurrentHealth());
            return;
        }

        Managers.Money.Money -= healthPrice;
        healthLevel += 1;

        player.GetComponent<PlayerController>().SetMaxHealth(5 + (healthLevel - 1));
        healthPrice = 5 + (healthLevel - 1) * 2.5f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        SetHealthValueText();
        ButtonInitiate();

        Debug.Log("Max HP: " + player.GetComponent<PlayerController>().GetMaxHealth());
        Debug.Log("Current HP: " + player.GetComponent<PlayerController>().GetCurrentHealth());
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
            Debug.Log("Max Bread: " + breadCounter.GetMaxBread());
            Debug.Log("Current Bread: " + breadCounter.GetCurrentBread());
            return;
        }
        Managers.Money.Money -= breadPrice;
        breadLevel += 1;

        breadCounter.SetMaxBread(5 + (breadLevel - 1) * 2);
        breadPrice = 5 + (breadLevel - 1) * 2.5f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        SetBreadValueText();
        ButtonInitiate();

        Debug.Log("Max Bread: " + breadCounter.GetMaxBread());
        Debug.Log("Current Bread: " + breadCounter.GetCurrentBread());
    }
    /// <summary>
    /// 이동속도 레벨에 따른 능력치 및 가격 세팅
    /// </summary>
    public void SetSpeedValue()
    {
        // 이동속도는 레벨별로 1 + 레벨 * 0.1, 최대 5레벨까지(최대치 1.5). 업그레이드 가격은 레벨별로 15 + 레벨 * 7.5
        if (speedLevel >= 6)
        {
            SetSpeedValueText();
            ButtonInitiate();
            return;
        }
        Managers.Money.Money -= speedPrice;
        speedLevel += 1;


        float newPlayerSpeed = 10 * (1 + (speedLevel - 1) * 0.1f);
        // 파워업 여부에 따라 바로 플레이어 스피드를 설정할지, 혹은 파워업 종료 후 복귀속도를 바꿀지 결정
        Debug.Log("IsDrinkPowerUp: " + powerUpManager.GetIsDrinkPowerUp());
        if (powerUpManager.GetIsDrinkPowerUp())
        {
            // 파워업이 되어 있다면 파워업 종료 후 복귀 속도를 바꾼다
            powerUpManager.SetPlayerInitialSpeed(newPlayerSpeed);
        }
        else
        {
            // 파워업이 안돼 있으면 바로 플레이어 이동속도를 바꾼다
            player.GetComponent<PlayerController>().SetPlayerSpeed(newPlayerSpeed);
            powerUpManager.SetPlayerInitialSpeed(newPlayerSpeed);
        }
        speedPrice = 15 + (speedLevel - 1) * 7.5f;

        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        SetSpeedValueText();
        ButtonInitiate();
    }

    public void SetDrinkValue()
    {
        drinkEachText.text = supplyManager.GetDrinkCount().ToString();
    }

    public void AddDrinkValue()
    {
        Managers.Money.Money -= drinkPrice;
        supplyManager.SetDrinkCount(supplyManager.GetDrinkCount() + 1);
        int drinkCount = supplyManager.GetDrinkCount();
        Debug.Log("음료수 구매: " + drinkCount);
        onDrinkChanged.Invoke(drinkCount);
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        drinkEachText.text = supplyManager.GetDrinkCount().ToString();
        ButtonInitiate();
    }

    public void SetButterValue()
    {
        butterEachText.text = supplyManager.GetButterCount().ToString();
    }

    public void AddButterValue()
    {
        Managers.Money.Money -= butterPrice;
        supplyManager.SetButterCount(supplyManager.GetButterCount() + 1);
        int butterCount = supplyManager.GetDrinkCount();
        onButterChanged.Invoke(butterCount);
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
        butterEachText.text = supplyManager.GetButterCount().ToString();
        ButtonInitiate();
    }

    public void SetMoneyValue()
    {
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
    }

    public void SetAirConditionerValue()
    {
        Managers.Money.Money -= airConditionerPrice;
        curMoneyText.text = "€ " + Managers.Money.Money.ToString("F2");
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
