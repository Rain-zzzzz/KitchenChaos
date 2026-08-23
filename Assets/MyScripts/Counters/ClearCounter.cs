using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (HasKitchenObj() == false)
        {
            if (player.HasKitchenObj())
            {
                player.GetKitchenObj().SetKitchenObjParent(this);
            }
        }
        else
        {
            if (player.HasKitchenObj())
            {
                if (player.GetKitchenObj().TryGetplate(out PlateKitchenObj palteKitchenObj))
                {
                    if (palteKitchenObj.TryAddIngredient(GetKitchenObj().GetKitchenObjectSO()))
                    {
                        Debug.Log("在盘子里添加食材");
                        GetKitchenObj().DestroySelf();
                    }
                }
                else
                {
                    if (GetKitchenObj().TryGetplate(out palteKitchenObj))
                    {
                        if (palteKitchenObj.TryAddIngredient(player.GetKitchenObj().GetKitchenObjectSO()))
                        {
                            Debug.Log("在盘子里添加食材");
                            player.GetKitchenObj().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObj().SetKitchenObjParent(player);
            }
        }
    }
}