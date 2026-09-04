
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : UI_Base
{
    enum Texts
    {
        BreadCount,
        CurrentMoney,
        MonsterBuffTime,
        ButterBuffTime,
        Level,
        MonsterAmount,
        ButterAmount,
    }

    enum Sliders
    {
        EXPBar,
    }

    private GameObject _monsterBuffPanel;
    private GameObject _butterBuffPanel;
    private float _monsterBuffDuration = 60f;
    private float _monsterBuffRemain;
    private float _butterBuffDuration = 30f;
    private float _butterBuffRemain;

    public override void Init()
    {
        _monsterBuffPanel = transform.Find("StatLayout/MonsterBuffPanel").gameObject;
        _butterBuffPanel = transform.Find("StatLayout/ButterBuffPanel").gameObject;

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));

        GetText((int)Texts.BreadCount).GetComponent<TextMeshProUGUI>().text =
            $"Bread: {Managers.Player.PlayerStat.Bread} / {Managers.Player.PlayerStat.MaxBread}";
        GetText((int)Texts.Level).GetComponent<TextMeshProUGUI>().text =
            $"Lv.{Managers.Player.PlayerStat.Level}";
        GetText((int)Texts.MonsterAmount).GetComponent<TextMeshProUGUI>().text =
            $"x{Managers.Player.PlayerStat.MonsterAmount}";
        GetText((int)Texts.ButterAmount).GetComponent<TextMeshProUGUI>().text =
            $"x{Managers.Player.PlayerStat.ButterAmount}";

        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().minValue = 0;
        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().maxValue = Managers.Player.GetMaxExp();
        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().value = Managers.Player.PlayerStat.Exp;
    }

    private void Update()
    {
        GetText((int)Texts.BreadCount).GetComponent<TextMeshProUGUI>().text =
            $"Bread: {Managers.Player.PlayerStat.Bread} / {Managers.Player.PlayerStat.MaxBread}";
        GetText((int)Texts.Level).GetComponent<TextMeshProUGUI>().text =
            $"Lv.{Managers.Player.PlayerStat.Level}";
        GetText((int)Texts.MonsterAmount).GetComponent<TextMeshProUGUI>().text =
            $"x{Managers.Player.PlayerStat.MonsterAmount}";
        GetText((int)Texts.ButterAmount).GetComponent<TextMeshProUGUI>().text =
            $"x{Managers.Player.PlayerStat.ButterAmount}";
        GetText((int)Texts.MonsterBuffTime).GetComponent<TextMeshProUGUI>().text =
            $"{(int)_monsterBuffRemain:D2}s";
        GetText((int)Texts.ButterBuffTime).GetComponent<TextMeshProUGUI>().text =
            $"{(int)_butterBuffRemain:D2}s";

        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().minValue = 0;
        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().maxValue = Managers.Player.GetMaxExp();
        GetSlider((int)Sliders.EXPBar).GetComponent<Slider>().value = Managers.Player.PlayerStat.Exp;

        if (Input.GetKeyDown(KeyCode.Q) && Managers.Player.UseMonsterBuff())
        {
            _monsterBuffRemain = _monsterBuffDuration;
            _monsterBuffPanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E) && Managers.Player.UseButterBuff())
        {
            _butterBuffRemain = _butterBuffDuration;
            _butterBuffPanel.SetActive(true);
        }
        if (_monsterBuffRemain < 0f)
        {
            Managers.Player.TurnOffMonsterBuff();
            _monsterBuffPanel.SetActive(false);
        }
        if (_butterBuffRemain < 0f)
        {
            Managers.Player.TurnOffButterBuff();
            _butterBuffPanel.SetActive(false);
        }

        _monsterBuffRemain -= Time.deltaTime;
        _butterBuffRemain -= Time.deltaTime;
    }
}