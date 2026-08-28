using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private Transform container;

    private void Awake()
    {
        gameObject.SetActive(true);
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryRecipeManager.Instance.StartSpawnRecipe += DelivetyManager_StartSpawnRecipe;
        DeliveryRecipeManager.Instance.EndSpawnRecipe += DeliveryManager_EndSpawnRecipe;
    }

    private void DelivetyManager_StartSpawnRecipe(object sender, System.EventArgs e)
    {
        UpdetaVisual();
    }

    private void DeliveryManager_EndSpawnRecipe(object sender, System.EventArgs e)
    {
        UpdetaVisual();
    }

    private void UpdetaVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (SinglePecipeSO singlePecipeSO in DeliveryRecipeManager.Instance.GetWaitingRecipeList())
        {
            Transform singleRecipeTransform = Instantiate(recipeTemplate, container);
            singleRecipeTransform.gameObject.SetActive(true);
            singleRecipeTransform.GetComponentInChildren<DeliveryManagerSingleUI>().SetSingleRecipeSOUIName(singlePecipeSO);
        }
    }
}