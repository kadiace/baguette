using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    Unknown,
    MainStage,
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

public static class HouseColors
{
    public static readonly Dictionary<HouseColor, Color32> Colors = new()
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
    Overbooked,
    RemoteDelivery,
    QuickThrow,
    Vigilante,
    Breadstorm,
    TripleShot,
    Carjack,
    GodamCity,
}