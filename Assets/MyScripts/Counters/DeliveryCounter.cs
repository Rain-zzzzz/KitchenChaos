using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObj())
        {
            if (player.GetKitchenObj().TryGetplate(out PlateKitchenObj plateKitchenObj))
            {
                if (DeliveryRecipeManager.Instance.DeliveryRecipe(plateKitchenObj))
                {
                    player.GetKitchenObj().DestroySelf();
                }
            }
        }
    }
}