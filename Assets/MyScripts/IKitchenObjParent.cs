using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjParent
{
    public Transform GetKitchenObjFollowTransfrom();

    public void SetKitchenObj(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObj();

    public void ClearKitchenObj();

    public bool HasKitchenObj();
}