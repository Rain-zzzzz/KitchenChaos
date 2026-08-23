using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectedCounter : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject visualGameObject;
    [SerializeField] private float showTime = 1f;
    private bool isShowing = false;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        Hide();
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        Debug.Log("每个柜子自己检查当前射线检测到的是不是自己");
        if (e.selectedCounter == baseCounter)
        {
            if (!isShowing)
            {
                Show();
                isShowing = true;
                StartCoroutine(ShowCoroutine());
            }
        }
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }

    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(showTime);
        Hide();
        isShowing = false;
    }
}