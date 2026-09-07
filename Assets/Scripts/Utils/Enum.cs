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
    RemoteSupply,
}

public static class AbilityCatalog
{
    public static readonly Dictionary<Ability, (string title, string description)> Info = new()
        {
            { Ability.OrderRush, ("주문 폭주", "입소문이 퍼졌습니다. 주문이 더 빨리 들어오고, 더 많은 주문을 받아둘 수 있습니다.") },
            { Ability.ThrowDelivery, ("배달의 달인", "빵을 던져서 맞춰도 배달에 성공합니다. 모든 주문의 수량이 1개로 감소하며, 배달 보수도 그만큼 감소합니다.") },
            { Ability.RapidThrow, ("습박 빵 던져잇", "기본 시점이 1인칭으로 변경되고 마우스 좌클릭 휘두르기가 던지기로 변경됩니다. 빵 던지기의 딜레이가 대폭 감소합니다.") },
            { Ability.Vigilante, ("자경단", "시켜줘 파리 명예 자경단. 소매치기를 처치할 때마다 보상이 들어옵니다.") },
            { Ability.ButterBlast, ("베스트프렌드", "버터의 효과가 변경됩니다. Shift 키를 눌러 사용할 범위를 결정하고, Shift 키를 떼 바게트 폭격을 날립니다") },
            { Ability.TripleShot, ("강화 투척", "빵을 던질 때 세 갈래로 나갑니다. 걱정하지 마세요! 똑같이 1개의 바게트만 소모됩니다.") },
            { Ability.Carjack, ("차량 강탈", "차를 훔쳐 탈 수 있습니다. 차를 탄 상태에서는 어떤 보상도 얻을 수 없습니다.") },
            { Ability.ThiefMagnet, ("참을 수 없어!", "향긋한 냄새에 소매치기들이 몰려듭니다. 소매치기가 시간이 지나면 점점 늘어납니다.") },
            { Ability.RemoteSupply, ("원격 충전", "베이커리에서 떨어져 있어도 F 키를 눌러 빵을 충전합니다.") },
        };
}

public enum EnemyHitCause
{
    Unknown,
    Player,
    Car,
}

public enum CarTriggerType
{
    Collision,
    Detection
}
