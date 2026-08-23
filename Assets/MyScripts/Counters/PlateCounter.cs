using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;

    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private float plateSpawnTime = 3f;
    [SerializeField] private int maxPlateCount = 4;
    private float plateSpawnTimer = 0f;
    private int plateCount = 0;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;
        if (plateSpawnTimer >= plateSpawnTime)
        {
            if (plateCount < maxPlateCount)
            {
                plateSpawnTimer = 0f;
                plateCount++;
                KitchenObject.SpawnKitchenObj(kitchenObjectSO, this);
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObj())
        {
            if (plateCount > 0)
            {
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                KitchenObject.SpawnKitchenObj(kitchenObjectSO, player);
                plateCount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}