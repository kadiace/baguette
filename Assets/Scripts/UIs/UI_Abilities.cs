using UnityEngine;
using UnityEngine.UI;

public class UI_Abilities : UI_Base
{
    enum Images
    {
        Abilities,
    }

    UI_AbilityCard[] _abilityCards = new UI_AbilityCard[3];

    public UI_AbilityCard[] UI_AbilityCards { get { return _abilityCards; } set { _abilityCards = value; } }

    public override void Init()
    {
        Bind<Image>(typeof(Images));

        Image abilities = GetImage((int)Images.Abilities);

        for (int i = 0; i < 3; i++)
        {
            UI_AbilityCard abilityCard = Managers.UI.CreateUI<UI_AbilityCard>(abilities.transform, "Components");
            RectTransform rectTransform = abilityCard.GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(i / 3f, rectTransform.anchorMin.y);
            rectTransform.anchorMax = new Vector2((i + 1) / 3f, rectTransform.anchorMax.y);

            rectTransform.offsetMin = new Vector2(20f, 60f);
            rectTransform.offsetMax = new Vector2(-20f, -20f);
            _abilityCards[i] = abilityCard;
        }
    }
}
