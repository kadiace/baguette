using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbilityCard : UI_Base
{
    enum Texts
    {
        Title,
        Description,
    }

    enum Buttons
    {
        Reroll,
    }

    private int _index;

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject rerollButton = GetButton((int)Buttons.Reroll).gameObject;
        rerollButton.SetActive(false);
    }

    public void SetCard(int index)
    {
        _index = index;
        Ability? ability = Managers.Player.AbilityCandidates[_index];
        if (ability is not Ability value)
        {
            GetText((int)Texts.Title).GetComponent<TextMeshProUGUI>().text = "선택 불가";
            GetText((int)Texts.Description).GetComponent<TextMeshProUGUI>().text = "선택할 수 없습니다.";
            return;
        }
        (string, string) abilityTooltip = AbilityCatalog.Info[value];
        GetText((int)Texts.Title).GetComponent<TextMeshProUGUI>().text = abilityTooltip.Item1;
        GetText((int)Texts.Description).GetComponent<TextMeshProUGUI>().text = abilityTooltip.Item2;
        gameObject.BindEvent(eventData => OnCardClicked(eventData, value));

        if (_index <= 2 && Managers.Player.AbilityCandidates[_index + 3] != null)
        {
            GameObject rerollButton = GetButton((int)Buttons.Reroll).gameObject;
            rerollButton.SetActive(true);
            rerollButton.BindEvent(OnRerollButtonClicked);
        }
    }

    private void OnCardClicked(PointerEventData eventData, Ability ability)
    {
        Managers.Player.PlayerStat.Abilities.Add(ability);
        Managers.Player.DisableAbilities();
    }

    private void OnRerollButtonClicked(PointerEventData eventData)
    {
        SetCard(_index + 3);
        GetButton((int)Buttons.Reroll).gameObject.SetActive(false);
    }
}
