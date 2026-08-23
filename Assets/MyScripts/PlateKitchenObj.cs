using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObj : KitchenObject
{
    public event EventHandler<OnTryAddIngredientEventArgs> OnTryAddIngredient;

    public static event EventHandler OnAddIngredient;

    public class OnTryAddIngredientEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenobjectSOList;
    private List<KitchenObjectSO> nowKitchenObjectSOList;

    private void Awake()
    {
        nowKitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenobjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (nowKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            nowKitchenObjectSOList.Add(kitchenObjectSO);

            OnTryAddIngredient?.Invoke(this, new OnTryAddIngredientEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            OnAddIngredient?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }

    public List<KitchenObjectSO> GetNowKitchenobjectSOList()
    {
        return nowKitchenObjectSOList;
    }
}