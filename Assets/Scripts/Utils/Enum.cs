using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    Unknown,
    MainStage,
}

public enum UIEvent
{
    Click,
    Drag,
}

public enum HouseColor
{
    Unknown,
    Red,
    Green,
    Yellow,
    Pink,
    Blue,
    Orange,
    Purple,
    Cyan,
    Brown,
}

public static class ColorCatalog
{
    public static readonly Dictionary<HouseColor, Color32> Info = new()
        {
            { HouseColor.Red,    new Color32(220, 60, 60, 255) },
            { HouseColor.Green,  new Color32(70, 180, 90, 255) },
            { HouseColor.Yellow, new Color32(240, 210, 60, 255) },
            { HouseColor.Pink,   new Color32(230, 120, 160, 255) },

            { HouseColor.Blue,   new Color32(70, 120, 210, 255) },
            { HouseColor.Orange, new Color32(235, 140, 55, 255) },
            { HouseColor.Purple, new Color32(150, 90, 190, 255) },
            { HouseColor.Cyan,   new Color32(70, 190, 200, 255) },

            { HouseColor.Brown,  new Color32(140, 95, 65, 255) },
        };
}

public enum Ability
{
    Unknown,
    OrderRush,
    ThrowDelivery,
    RapidThrow,
    Vigilante,
    ButterBlast,
    TripleShot,
    Carjack,
    ThiefMagnet,
}

public static class AbilityCatalog
{
    public static readonly Dictionary<Ability, (string title, string description)> Info = new()
        {
            { Ability.OrderRush, ("주문 폭주", "입소문이 퍼졌습니다. 주문이 더 빨리 들어오고, 더 많은 주문을 받아둘 수 있습니다.") },
            { Ability.ThrowDelivery, ("배달의 달인", "빵을 던져서 맞춰도 배달에 성공합니다. 모든 주문의 수량이 1개로 감소하며, 배달 보수도 그만큼 감소합니다.") },
            { Ability.RapidThrow, ("습박 빵 던져", "기본 시점이 1인칭으로 변경, 마우스 좌클릭 휘두르기가 던지기로 변경됩니다. 빵 던지기의 딜레이가 대폭 감소합니다.") },
            { Ability.Vigilante, ("자경단", "시켜줘, 파리 명예 자경단. 소매치기를 처치할 때마다 보상이 들어옵니다.") },
            { Ability.ButterBlast, ("베스트프렌드", "버터의 효과가 배달 보수 증가 버프에서, 영역 범위 공격으로 변경됩니다.") },
            { Ability.TripleShot, ("바게트의 상처", "빵을 던질 때 세 갈래로 나갑니다. 걱정하지 마세요! 똑같이 1개의 바게트만 소모됩니다.") },
            { Ability.Carjack, ("자동차,\n빌리겠습니다", "차를 훔쳐 탈 수 있습니다. 차를 탄 상태에서는 NPC와 상호작용이 불가능합니다.") },
            { Ability.ThiefMagnet, ("냄새 맡고\n왔습니다", "향긋한 냄새에 소매치기들이 몰려듭니다. 소매치기가 지금보다 2배 더 늘어납니다.") },
        };
}

public enum EnemyHitCause
{
    Unknown,
    Player,
    Car,
}
