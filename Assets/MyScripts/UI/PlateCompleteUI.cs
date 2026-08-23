using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;

    [SerializeField] private PlateKitchenObj plateKitchenObj;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObj.OnTryAddIngredient += PlateKitchenObj_OnTryAddIngredient;
    }

    private void PlateKitchenObj_OnTryAddIngredient(object sender, PlateKitchenObj.OnTryAddIngredientEventArgs e)
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            //Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO validKitchenObj in plateKitchenObj.GetNowKitchenobjectSOList())
        {
            if (e.kitchenObjectSO == validKitchenObj)
            {
                Transform plateTemplateTransform = Instantiate(iconTemplate, transform);
                plateTemplateTransform.gameObject.SetActive(true);
                plateTemplateTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(e.kitchenObjectSO);
                Debug.Log(e.kitchenObjectSO.objectName);
            }
        }
    }
}