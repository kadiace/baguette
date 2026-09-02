using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PowerUpManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("소모품 개수를 관리하는 SupplyManager")]
    [SerializeField] private SupplyManager supplyManager;
    [Tooltip("플레이어의 이동속도를 관리하는 PlayerController")]
    [SerializeField] private PlayerController playerController;
    [Tooltip("상점에서 소모품 구매 이벤트를 전달하는 ShopManager(매니저 아님)")]
    [SerializeField] private ShopManager shopManager;

    [Tooltip("몬스터 음료수 개수")]
    [SerializeField] private int drinkAmount = 0;
    [Tooltip("몬스터 음료수 개수 표시 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI drinkAmountText;
    [Tooltip("버터 개수")]
    [SerializeField] private int butterAmount = 0;
    [Tooltip("버터 개수 표시 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI butterAmountText;

    [Tooltip("플레이어 초기 이동속도")]
    [SerializeField] private float playerInitialSpeed = 0f;
    [Tooltip("플레이어 파워업 이동속도")]
    [SerializeField] private float playerPowerUpSpeed = 0f;

    // 평소에는 1배수, 아이템으로 파워업된 상태에서는 1.2배수로
    // 보상 돈을 지급할 때 금액 배수를 전달하는 용도
    [Tooltip("현재 보상 배수")]
    [SerializeField] private float rewardMultipler = 1.0f;

    [Tooltip("몬스터 음료수 파워업 버프 오브젝트")]
    [SerializeField] private GameObject drinkBuffPanel;
    [Tooltip("몬스터 음료수 파워업 버프 남은시간 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI drinkBuffTimeText;
    [Tooltip("버터 파워업 버프 오브젝트")]
    [SerializeField] private GameObject butterBuffPanel;
    [Tooltip("버터 파워업 버프 남은시간 텍스트")]
    [SerializeField] private TMPro.TextMeshProUGUI butterBuffTimeText;

    [Tooltip("몬스터 음료수 파워업 적용 시간")]
    [SerializeField] private float drinkBuffTime = 3.0f;
    [Tooltip("버터 파워업 적용 시간")]
    [SerializeField] private float butterBuffTime = 6.0f;

    [Tooltip("현재 몬스터 파워업중인지 체크하기 위한 bool")]
    [SerializeField] private bool isDrinkPowerUp = false;
    [Tooltip("현재 버터 파워업중인지 체크하기 위한 bool")]
    [SerializeField] private bool isButterPowerUp = false;


    void Start()
    {
        // 처음에 어떤거 받아올까?
        drinkAmount = supplyManager.GetDrinkCount();
        butterAmount = supplyManager.GetButterCount();
        playerInitialSpeed = playerController.GetPlayerSpeed();
        playerPowerUpSpeed = playerInitialSpeed * 1.2f;

        drinkAmountText.text = drinkAmount.ToString();
        butterAmountText.text = butterAmount.ToString();

        drinkBuffPanel.SetActive(false);
        butterBuffPanel.SetActive(false);

        shopManager.GetComponent<ShopManager>().onDrinkChanged.AddListener(UpdateDrinkCounter);
        shopManager.GetComponent<ShopManager>().onButterChanged.AddListener(UpdateButterCounter);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isDrinkPowerUp)
        {
            Debug.Log("Q 입력 당시 drinkAmount: " + drinkAmount);
            if (drinkAmount > 0)
            {
                drinkAmount -= 1;
                drinkAmountText.text = drinkAmount.ToString();
                supplyManager.SetDrinkCount(drinkAmount);
                Debug.Log("키 입력 확인: Q");
                StartCoroutine(DrinkPowerUp());
            }

            else
            {
                Debug.Log("몬스터 개수 부족: " + drinkAmount);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isButterPowerUp)
        {
            if (butterAmount > 0)
            {
                butterAmount -= 1;
                butterAmountText.text = butterAmount.ToString();
                supplyManager.SetButterCount(butterAmount);
                Debug.Log("키 입력 확인: E");
                StartCoroutine(ButterPowerUp());
            }
            else
            {
                Debug.Log("버터 개수 부족: " + butterAmount);
            }
        }
    }

    // 받아온 값들로 무엇을 할까?
    /// <summary>
    /// 파워업 후 복귀할 이동속도 설정.
    /// ShopManager에서 파워업 도중 이속 업글했을 때 수정해야 하므로 만들었다
    /// </summary>
    /// <param name="speed">PlayerController 혹은 ShopManager에서 설정하는 파워업 종료 후 복귀 속도</param>
    public void SetPlayerInitialSpeed(float speed)
    {
        playerInitialSpeed = speed;
        return;
    }

    public void UpdateDrinkCounter(int drinkCount)
    {
        Debug.Log("음료수 구매 이벤트 확인 및 전달: " + drinkCount);
        drinkAmount = drinkCount;
    }

    public void UpdateButterCounter(int butterCount)
    {
        butterAmount = butterCount;
    }

    #region 파워업 여부 확인, 설정 함수
    /// <summary>
    /// 몬스터 파워업 중인지 확인
    /// ShopManager가 이속 업글 시 원래 파워업 도중이었는지 체크하는 용도
    /// </summary>
    /// <returns>몬스터 파워업 상태를 알려주는 bool</returns>
    public bool GetIsDrinkPowerUp()
    {
        return isDrinkPowerUp;
    }
    /// <summary>
    /// 몬스터 파워업 여부 수정
    /// 파워업 코루틴이 시작, 끝날 때 호출하여 파워업 여부를 설정한다
    /// </summary>
    /// <param name="state"></param>
    public void SetIsDrinkPowerUp(bool state)
    {
        isDrinkPowerUp = state;
    }
    /// <summary>
    /// 버터 파워업 중인지 확인
    /// 보상 받는 함수에서 파워업 여부를 확인하고 지금 제공할 보상에 배율을 적용할지 판단하는 용도
    /// </summary>
    /// <returns>버터 파워업 상태를 알려주는 bool</returns>
    public bool GetIsButterPowerUp()
    {
        return isButterPowerUp;
    }
    /// <summary>
    /// 버터 파워업 여부 수정
    /// 파워업 코루틴이 시작, 끝날 때 호출하여 파워업 여부를 설정한다
    /// </summary>
    /// <param name="state"></param>
    public void SetIsButterPowerUp(bool state)
    {
        isButterPowerUp = state;
    }
    #endregion

    #region 파워업 코루틴
    IEnumerator DrinkPowerUp()
    {
        float remainingTime = drinkBuffTime;

        while (remainingTime > 0)
        {
            isDrinkPowerUp = true;
            drinkBuffPanel.SetActive(true);

            drinkBuffTimeText.text = Mathf.CeilToInt(remainingTime).ToString() + "s";

            playerPowerUpSpeed = playerInitialSpeed * 1.2f;
            playerController.SetPlayerSpeed(playerPowerUpSpeed);
            Debug.Log("파워업 시작 시 초기 속도: " + playerInitialSpeed + "\n파워업 적용 속도: " + playerPowerUpSpeed);

            remainingTime -= Time.deltaTime;

            yield return null;
        }

        drinkBuffPanel.SetActive(false);
        playerController.SetPlayerSpeed(playerInitialSpeed);
        Debug.Log("파워업 종료 후 복귀 속도: " + playerInitialSpeed);
        isDrinkPowerUp = false;
    }

    IEnumerator ButterPowerUp()
    {
        float remainingTime = butterBuffTime;

        while (remainingTime > 0)
        {
            isButterPowerUp = true;
            butterBuffPanel.SetActive(true);

            butterBuffTimeText.text = Mathf.CeilToInt(remainingTime).ToString() + "s";
            rewardMultipler = 1.25f;

            remainingTime -= Time.deltaTime;

            yield return null;
        }


        butterBuffPanel.SetActive(false);
        rewardMultipler = 1.0f;
        isButterPowerUp = false;

    }
    #endregion
    /// <summary>
    /// 음료수 개수 텍스트 업데이트
    /// </summary>
    public void SetDrinkValue()
    {
        drinkAmount = supplyManager.GetDrinkCount();
        drinkAmountText.text = drinkAmount.ToString();
    }

    /// <summary>
    /// 버터 개수 텍스트 업데이트
    /// </summary>
    public void SetButterValue()
    {
        butterAmount = supplyManager.GetButterCount();
        butterAmountText.text = butterAmount.ToString();
    }
}
