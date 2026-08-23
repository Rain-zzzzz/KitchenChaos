using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    public static event EventHandler OnAnyCut;

    [SerializeField] private CutingRecipeSO[] cutingRecipeSOArray;
    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (HasKitchenObj() == false)
        {
            if (player.HasKitchenObj())
            {
                if (HasRecipeWithInput(player.GetKitchenObj().GetKitchenObjectSO()))
                {
                    CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSO(player.GetKitchenObj().GetKitchenObjectSO());
                    cuttingProgress = 0;
                    player.GetKitchenObj().SetKitchenObjParent(this);
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
                        UIColour = 0
                    });
                }
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
            }
            if (!player.HasKitchenObj())
            {
                GetKitchenObj().SetKitchenObjParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObj() && HasRecipeWithInput(GetKitchenObj().GetKitchenObjectSO()))
        {
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            KitchenObjectSO inputKitchenObjectSO = GetKitchenObj().GetKitchenObjectSO();
            KitchenObjectSO outputKitchenObjectSO = GetOutputfromInput(inputKitchenObjectSO);
            CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSO(inputKitchenObjectSO);
            cuttingProgress++;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
                UIColour = 0
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObj().DestroySelf();
                KitchenObject.SpawnKitchenObj(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSO(inputKitchenObjectSO);
        if (cuttingRecipeSO) return true;
        else return false;
    }

    private KitchenObjectSO GetOutputfromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSO(inputKitchenObjectSO);
        if (cuttingRecipeSO) return cuttingRecipeSO.output;
        else return null;
    }

    private CutingRecipeSO GetCutingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CutingRecipeSO cuttingRecipeSO in cutingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}