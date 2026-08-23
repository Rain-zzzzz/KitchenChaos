using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> StateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private BurningRecipeSO[] fryingRecipeSO_First;
    [SerializeField] private BurningRecipeSO[] fryingRecipeSO_Second;
    private State state;
    private float theTimer;
    private BurningRecipeSO firstRecipeSO;
    private BurningRecipeSO secondRecipeSO;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    };

    private void Start()
    {
        this.state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObj())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Frying:
                    theTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = theTimer / GetFirstRecipeSO(GetKitchenObj().GetKitchenObjectSO()).lastingTime,
                        UIColour = 0
                    });

                    if (theTimer >= GetFirstRecipeSO(GetKitchenObj().GetKitchenObjectSO()).lastingTime)
                    {
                        GetKitchenObj().DestroySelf();
                        KitchenObject.SpawnKitchenObj(GetOutputfromInput_First(firstRecipeSO.input), this);

                        secondRecipeSO = GetSecondRecipeSO(GetKitchenObj().GetKitchenObjectSO());
                        state = State.Fried;
                        StateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        theTimer = 0f;
                    }
                    break;

                case State.Fried:
                    theTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = theTimer / GetSecondRecipeSO(GetKitchenObj().GetKitchenObjectSO()).lastingTime,
                        UIColour = 1
                    });

                    if (theTimer >= GetSecondRecipeSO(GetKitchenObj().GetKitchenObjectSO()).lastingTime)
                    {
                        GetKitchenObj().DestroySelf();

                        KitchenObject.SpawnKitchenObj(GetOutputfromInput_Second(secondRecipeSO.input), this);
                        state = State.Burned;
                        StateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                        theTimer = 0f;
                    }
                    break;

                case State.Burned:
                    // Handle burned logic
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObj() == false)
        {
            if (player.HasKitchenObj())
            {
                if (HasRecipeWithInput(player.GetKitchenObj().GetKitchenObjectSO()))
                {
                    firstRecipeSO = GetFirstRecipeSO(player.GetKitchenObj().GetKitchenObjectSO());
                    player.GetKitchenObj().SetKitchenObjParent(this);
                    state = State.Frying;
                    StateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    theTimer = 0f;
                }
            }
        }
        else
        {
            if (player.HasKitchenObj())
            {
                if (player.GetKitchenObj().TryGetplate(out PlateKitchenObj palteKitchenObj))
                {
                    if (palteKitchenObj.TryAddIngredient(GetKitchenObj().GetKitchenObjectSO()))
                    {
                        Debug.Log("在盘子里添加食材");
                        GetKitchenObj().DestroySelf();
                        state = State.Idle;
                        StateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f,
                            UIColour = 0
                        });
                    }
                }
            }
            if (!player.HasKitchenObj())
            {
                GetKitchenObj().SetKitchenObjParent(player);
                state = State.Idle;
                StateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f,
                    UIColour = 0
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        BurningRecipeSO firstRecipeSO = GetFirstRecipeSO(inputKitchenObjectSO);
        if (firstRecipeSO) return true;
        else return false;
    }

    private KitchenObjectSO GetOutputfromInput_First(KitchenObjectSO inputKitchenObjectSO)
    {
        BurningRecipeSO firstRecipeSO = GetFirstRecipeSO(inputKitchenObjectSO);
        if (firstRecipeSO) return firstRecipeSO.output;
        else return null;
    }

    private KitchenObjectSO GetOutputfromInput_Second(KitchenObjectSO inputKitchenObjectSO)
    {
        BurningRecipeSO SecondRecipeSO = GetSecondRecipeSO(inputKitchenObjectSO);
        if (SecondRecipeSO) return SecondRecipeSO.output;
        else return null;
    }

    private BurningRecipeSO GetFirstRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in fryingRecipeSO_First)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetSecondRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in fryingRecipeSO_Second)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}