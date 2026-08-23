using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryRecipeManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryRecipeManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyPlacedSomething += BaseCounter_OnAnyPlacedSomething;
        TrashCounter.OnAnyTrashedSomething += TrashCounter_OnAnyPlacedSomething;
        PlateKitchenObj.OnAddIngredient += PlateKitchenObj_OnAddIngredient;
    }

    private void PlateKitchenObj_OnAddIngredient(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop, Camera.main.transform.position, 0.3f);
    }

    private void TrashCounter_OnAnyPlacedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.trash, Camera.main.transform.position, 0.3f);
    }

    private void BaseCounter_OnAnyPlacedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop, Camera.main.transform.position, 0.3f);
    }

    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Camera.main.transform.position, 0.3f);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.chop, Camera.main.transform.position, 0.3f);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, Camera.main.transform.position, 0.3f);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position, 0.3f);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}