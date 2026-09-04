using UnityEngine;
using UnityEngine.UI;

public class UI_Deliveries : UI_Base
{
    enum Images
    {
        Background,
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }
}
