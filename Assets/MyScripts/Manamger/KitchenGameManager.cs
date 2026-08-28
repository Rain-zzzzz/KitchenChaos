using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;

    public event EventHandler OnGameUnpause;

    public event EventHandler OnGamePause;

    public static KitchenGameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    [SerializeField] private float waitingToStart = 1f;
    [SerializeField] private float countdownToStart = 3f;
    [SerializeField] private float gamePlayingTimerMax = 10f;
    private float gamePlayingTimer;
    private bool isGamePause = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += Instance_OnPauseAction;
    }

    private void Instance_OnPauseAction(object sender, EventArgs e)
    {
        ToggleGamePause();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStart -= Time.deltaTime;
                if (waitingToStart < 0f)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:
                countdownToStart -= Time.deltaTime;
                if (countdownToStart < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }

    public bool Is_GamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool Is_Countdown()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStart;
    }

    public bool Is_GameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalize()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void ToggleGamePause()
    {
        if (!isGamePause)
        {
            OnGamePause?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpause?.Invoke(this, EventArgs.Empty);
        }
        isGamePause = !isGamePause;
    }
}