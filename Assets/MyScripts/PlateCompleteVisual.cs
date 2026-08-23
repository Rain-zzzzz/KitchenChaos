using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    private struct KitchenObjectSO_GameObj
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObjectPrefab;
    }

    private PlateKitchenObj plateKitchenObj;

    [SerializeField]
    private KitchenObjectSO_GameObj[] kitchenObjectSO_GameObjs;

    private void Awake()
    {
        plateKitchenObj = GetComponent<PlateKitchenObj>();
    }

    private void Start()
    {
        plateKitchenObj.OnTryAddIngredient += PlateKitchenObj_OnTryAddIngredient;
        foreach (KitchenObjectSO_GameObj kitchenObjectSO_GameObj in kitchenObjectSO_GameObjs)
        {
            kitchenObjectSO_GameObj.gameObjectPrefab.SetActive(false);
        }
    }

    private void PlateKitchenObj_OnTryAddIngredient(object sender, PlateKitchenObj.OnTryAddIngredientEventArgs e)
    {
        foreach (KitchenObjectSO_GameObj kitchenObjectSO_GameObj in kitchenObjectSO_GameObjs)
        {
            if (kitchenObjectSO_GameObj.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObj.gameObjectPrefab.SetActive(true);
            }
        }
    }
}