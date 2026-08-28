using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyTrashedSomething;

    public new static void ResetStaticData()
    {
        OnAnyTrashedSomething = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObj())
        {
            OnAnyTrashedSomething?.Invoke(this, EventArgs.Empty);
            player.GetKitchenObj().DestroySelf();
        }
    }
}