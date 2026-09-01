using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [Tooltip("현재 소지한 돈")]
    [SerializeField] private float currentMoney;
    void Start()
    {
        SetCurrentMoney(3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Getter, Setter, 더하기, 빼기
    /// <summary>
    /// 현재 소지한 돈 반환
    /// </summary>
    /// <returns>현재 돈</returns>
    public float GetCurrentMoney()
    {
        return currentMoney;
    }
    /// <summary>
    /// 현재 소지한 돈 설정
    /// </summary>
    /// <param name="money">설정 금액</param>
    public void SetCurrentMoney(float money)
    {
        currentMoney = money;
    }

    /// <summary>
    /// 돈 더하기
    /// </summary>
    /// <param name="money">더할 금액</param>
    public void AddMoney(float money)
    {
        currentMoney += money;
    }

    /// <summary>
    /// 돈 빼기
    /// </summary>
    /// <param name="money">뺄 금액</param>
    public void SubtractMoney(float money)
    {
        currentMoney -= money;
    }
    #endregion
}
