using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjParent
{
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Transform playerHoldPoint;

    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private float interactDistance = 2f;

    private Vector3 lastInteractDir;
    private bool isWalking = false;
    private BaseCounter selectedCounter = null;
    private KitchenObject kitchenObject;

    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    private void Awake()
    {
    }

    private void Start()
    {
        //目的是只在按下E/F产生交互
        GameInput.Instance.OnInteractionAction += GameInput_OnInteractionAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        if (!IsOwner) return;
        //HandleMove();
        HandleMovementServerAuth();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void GameInput_OnInteractionAction(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.Is_GamePlaying())
        {
            RaycastInteract();
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.Is_GamePlaying())
        {
            RaycastInteract();
            if (selectedCounter != null)
            {
                selectedCounter.InteractAlternate(this);
            }
        }
    }
    private void HandleMovementServerAuth()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        HandleMovementServerRpc(inputVector);
    }

    [ServerRpc(RequireOwnership =false)]
    private void HandleMovementServerRpc(Vector2 inputVector)
    {
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        isWalking = moveDir != Vector3.zero;
        float rotationSpeed = 15f;
        transform.forward = Vector3.Lerp(transform.forward, -moveDir, Time.deltaTime * rotationSpeed);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveSpeed * Time.deltaTime);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveSpeed * Time.deltaTime);

            if (!canMove)
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveSpeed * Time.deltaTime);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
            else
            {
                moveDir = moveDirX;
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    //private void HandleMove()
    //{
    //    
    //    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    //    isWalking = moveDir != Vector3.zero;
    //    float rotationSpeed = 15f;
    //    transform.forward = Vector3.Lerp(transform.forward, -moveDir, Time.deltaTime * rotationSpeed);

    //    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveSpeed * Time.deltaTime);
    //    if (!canMove)
    //    {
    //        Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
    //        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveSpeed * Time.deltaTime);

    //        if (!canMove)
    //        {
    //            Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
    //            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveSpeed * Time.deltaTime);

    //            if (canMove)
    //            {
    //                moveDir = moveDirZ;
    //            }
    //        }
    //        else
    //        {
    //            moveDir = moveDirX;
    //        }
    //    }

    //    if (canMove)
    //    {
    //        transform.position += moveDir * moveSpeed * Time.deltaTime;
    //    }
    //}

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    private void RaycastInteract()
    {
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                Debug.Log("按E检测到了柜子");
                SetSelectedCounter(baseCounter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    public Transform GetKitchenObjFollowTransfrom()
    {
        return playerHoldPoint;
    }

    public void SetKitchenObj(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObj()
    {
        return kitchenObject;
    }

    public void ClearKitchenObj()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObj()
    {
        return kitchenObject != null;
    }
}
