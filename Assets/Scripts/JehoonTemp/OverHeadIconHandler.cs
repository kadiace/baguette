using System.Collections;
using UnityEngine;

public class OverHeadIconHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Tooltip("머리위 빵")]
    [SerializeField] private GameObject OverHeadBread;
    [Tooltip("머리위 유로")]
    [SerializeField] private GameObject OverHeadEuro;

    public bool isBreadShown = false;
    public bool isEuroShown = false;

    private IEnumerator ShowBreadCoroutine()
    {
        OverHeadBread.SetActive(true);
        isBreadShown = true;
        yield return new WaitForSeconds(1f);

        OverHeadBread.SetActive(false);
        isBreadShown = false;
    }
    /// <summary>
    /// ShowBreadCoroutine을 실행한다.
    /// </summary>
    public void StartShowBread()
    {
        StartCoroutine(ShowBreadCoroutine());
    }

    private IEnumerator ShowEuroCoroutine()
    {
        OverHeadEuro.SetActive(true);
        isEuroShown = true;
        yield return new WaitForSeconds(1f);

        OverHeadEuro.SetActive(false);
        isEuroShown = false;
    }

    public void StartShowEuro()
    {
        StartCoroutine(ShowEuroCoroutine());
    }
}
