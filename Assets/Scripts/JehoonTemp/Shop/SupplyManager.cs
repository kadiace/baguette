using UnityEngine;

public class SupplyManager : MonoBehaviour
{
    [Tooltip("현재 소지한 음료")]
    [SerializeField] private int DrinkCount;
    [Tooltip("현재 소지한 버터")]
    [SerializeField] private int ButterCount;

    void Start()
    {
        SetDrinkCount(0);
        SetButterCount(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Getter, Setter, 더하기, 빼기
    /// <summary>
    /// 현재 소지한 음료 개수
    /// </summary>
    /// <returns>현재 음료 수량</returns>
    public int GetDrinkCount()
    {
        return DrinkCount;
    }
    /// <summary>
    /// 현재 소지한 음료 설정
    /// </summary>
    /// <param name="count">설정 수량</param>
    public void SetDrinkCount(int count)
    {
        DrinkCount = count;
    }

    /// <summary>
    /// 현재 소지한 버터 개수
    /// </summary>
    /// <returns>현재 버터 수량</returns>
    public int GetButterCount()
    {
        return ButterCount;
    }

    /// <summary>
    /// 현재 소지한 버터 설정
    /// </summary>
    /// <param name="count">설정 수량</param>
    public void SetButterCount(int count)
    {
        ButterCount = count;
    }
    #endregion
}
