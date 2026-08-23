using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    [SerializeField] protected KitchenObjectSO kitchenObjectSO;//利用可脚本化对象

    public event EventHandler OnPlayerInteractObjAni;

    private bool isStartCoroution = false;

    public override void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            if (!isStartCoroution && !player.HasKitchenObj()) StartCoroutine(WaitForSeconds());
        }
        else
        {
            kitchenObject.SetKitchenObjParent(player);
            ShowSomeGameObjects();
        }
    }

    private IEnumerator WaitForSeconds()
    {
        isStartCoroution = true;
        OnPlayerInteractObjAni?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(0.75f);

        KitchenObject.SpawnKitchenObj(kitchenObjectSO, this);
        HideSomeGameObjects();
        isStartCoroution = false;
    }
}