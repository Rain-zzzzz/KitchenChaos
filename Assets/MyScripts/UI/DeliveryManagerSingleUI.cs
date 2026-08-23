using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeSingleName;
    [SerializeField] private Transform iconTemplate;
    [SerializeField] private Transform iconContainer;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetSingleRecipeSOUIName(SinglePecipeSO singlePecipeSO)
    {
        recipeSingleName.text = singlePecipeSO.name;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in singlePecipeSO.singlePecipeSO_List)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}