using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DeliveryRecipeManager : MonoBehaviour
{
    //在一定时间内生成一定数量的菜单
    //菜单list数据-通过可脚本化对象实现
    public static DeliveryRecipeManager Instance { get; private set; }

    public event EventHandler StartSpawnRecipe;

    public event EventHandler EndSpawnRecipe;

    public event EventHandler OnRecipeSuccess;

    public event EventHandler OnRecipeFailed;

    [SerializeField] private AllRecipeSO allRecipeSO_List;
    private List<SinglePecipeSO> waitingRecipeList;
    private float timer = 0f;
    private float spawnRecipeTimeMax = 5f;
    private int waitingRecipesNumMax = 5;

    private void Awake()
    {
        Instance = this;
        waitingRecipeList = new List<SinglePecipeSO>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnRecipeTimeMax;
            if (waitingRecipeList.Count < waitingRecipesNumMax)
            {
                SinglePecipeSO singlePecipeSO = allRecipeSO_List.allRecipeSO_List[UnityEngine.Random.Range(0, allRecipeSO_List.allRecipeSO_List.Count)];
                waitingRecipeList.Add(singlePecipeSO);

                StartSpawnRecipe?.Invoke(this, EventArgs.Empty);
                Debug.Log(singlePecipeSO.name);
            }
        }
    }

    //检查等待check的菜单和我盘子中的菜单内容是否一致
    public bool DeliveryRecipe(PlateKitchenObj plateKitchenObj)
    {
        if (waitingRecipeList.Count > 0)
        {
            SinglePecipeSO singlePecipeSO = waitingRecipeList[0];
            if (singlePecipeSO.singlePecipeSO_List.Count == plateKitchenObj.GetNowKitchenobjectSOList().Count)
            {
                Debug.Log("原料数相同");
                bool singlePecipeSOMatchPlateKitchenObjSO = true;
                foreach (KitchenObjectSO kitchenObjectSO_i in plateKitchenObj.GetNowKitchenobjectSOList())
                {
                    bool hasIngredient = false;
                    foreach (KitchenObjectSO kitchenObjectSO_j in singlePecipeSO.singlePecipeSO_List)
                    {
                        if (kitchenObjectSO_i == kitchenObjectSO_j)
                        {
                            hasIngredient = true;
                        }
                    }
                    if (!hasIngredient)
                    {
                        singlePecipeSOMatchPlateKitchenObjSO = false;
                        break;
                    }
                }

                Debug.Log("核对菜单结束");
                if (!singlePecipeSOMatchPlateKitchenObjSO)
                {
                    OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                    Debug.Log("需求菜单和盘子中的菜品不相符合");
                    return false;
                }
                else
                {
                    waitingRecipeList.RemoveAt(0);
                    EndSpawnRecipe?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    Debug.Log("需求菜单和盘子中的菜品相符合");
                    return true;
                }
            }
            else
            {
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log("需求菜单和盘子中的菜品数量不等");
                return false;
            }
        }
        else
        {
            Debug.Log("现在没有客户需求菜单");
            return false;
        }
    }

    public List<SinglePecipeSO> GetWaitingRecipeList()
    {
        return waitingRecipeList;
    }
}