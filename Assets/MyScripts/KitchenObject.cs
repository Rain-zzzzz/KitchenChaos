using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjParent kitchenObjParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjParent(IKitchenObjParent kitchenObjParent)
    {
        if (this.kitchenObjParent != null)
        {
            this.kitchenObjParent.ClearKitchenObj();
        }
        this.kitchenObjParent = kitchenObjParent;
        kitchenObjParent.SetKitchenObj(this);

        transform.parent = kitchenObjParent.GetKitchenObjFollowTransfrom();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjParent GetKitchenObjParent()
    {
        return this.kitchenObjParent;
    }

    public void DestroySelf()
    {
        kitchenObjParent.ClearKitchenObj();
        Destroy(gameObject);
    }

    //单例模式-实例化对象
    public static KitchenObject SpawnKitchenObj(KitchenObjectSO kitchenObjectSO, IKitchenObjParent kitchenObjParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjParent(kitchenObjParent);
        return kitchenObject;
    }

    public bool TryGetplate(out PlateKitchenObj plateKitchenObj)
    {
        if (this is PlateKitchenObj)
        {
            plateKitchenObj = this as PlateKitchenObj;
            return true;
        }
        else
        {
            plateKitchenObj = null;
            return false;
        }
    }
}