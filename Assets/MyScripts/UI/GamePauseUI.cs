using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button rebackButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        rebackButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.ToggleGamePause();
        });
        menuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScence);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePause += KitchenManager_OnGamePause;
        KitchenGameManager.Instance.OnGameUnpause += KitchenManager_OnGameUnpause;
        Hide();
    }

    private void KitchenManager_OnGamePause(object sender, System.EventArgs e)
    {
        Show();
    }

    private void KitchenManager_OnGameUnpause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}