using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAni : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private string IS_OPEN = "IsOpen";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerInteractObjAni += ContainerCounter_OnPlayerInteractObj;
    }

    private void ContainerCounter_OnPlayerInteractObj(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_OPEN);
    }
}