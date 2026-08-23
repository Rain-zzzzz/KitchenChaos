using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject SizzlingParticles;
    [SerializeField] private GameObject StoveOnVisual;

    private void Start()
    {
        stoveCounter.StateChanged += StoveCounter_OnStateChanged;
        SizzlingParticles.SetActive(false);
        StoveOnVisual.SetActive(false);
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        switch (e.state)
        {
            case StoveCounter.State.Idle:
                SizzlingParticles.SetActive(false);
                StoveOnVisual.SetActive(false);
                break;

            case StoveCounter.State.Frying:
                SizzlingParticles.SetActive(true);
                StoveOnVisual.SetActive(true);
                break;

            case StoveCounter.State.Fried:
                SizzlingParticles.SetActive(true);
                StoveOnVisual.SetActive(true);
                break;

            case StoveCounter.State.Burned:
                SizzlingParticles.SetActive(false);
                StoveOnVisual.SetActive(false);
                break;
        }
    }
}