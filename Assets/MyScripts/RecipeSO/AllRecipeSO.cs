using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AllRecipeSO : ScriptableObject
{
    [SerializeField] public List<SinglePecipeSO> allRecipeSO_List;
}