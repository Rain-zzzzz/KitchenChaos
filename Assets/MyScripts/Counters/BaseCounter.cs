using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjParent
{
    public static event EventHandler OnAnyPlacedSomething;

    public static void ResetStaticData()
    {
        OnAnyPlacedSomething = null;
    }

    [SerializeField] protected Transform counterTopPoint;
    [SerializeField] protected GameObject[] someGameObjects;
    protected KitchenObject kitchenObject;

    //按E键交互
    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter Interact");
    }

    //按K键交互
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter InteractAlternate");
    }

    //接口函数
    public Transform GetKitchenObjFollowTransfrom()
    {
        return counterTopPoint;
    }

    public void SetKitchenObj(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnAnyPlacedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObj()
    {
        return kitchenObject;
    }

    public void ClearKitchenObj()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObj()
    {
        return kitchenObject != null;
    }

    //隐显一些柜台东西
    protected void HideSomeGameObjects()
    {
        foreach (GameObject obj in someGameObjects)
        {
            obj.SetActive(false);
        }
    }

    protected void ShowSomeGameObjects()
    {
        foreach (GameObject obj in someGameObjects)
        {
            obj.SetActive(true);
        }
    }
}