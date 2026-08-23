using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SinglePecipeSO : ScriptableObject
{
    //原材料
    [SerializeField] public List<KitchenObjectSO> singlePecipeSO_List;
}