using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform playerHoldPoint;
    private float playerRadius = .7f;

    private float playerHeight = 2f;
    private float interactDistance = 2f;
    private Vector3 lastInteractDir;
    private bool isWalking = false;
    private BaseCounter selectedCounter = null;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //目的是只在按下E/F产生交互
        gameInput.OnInteractionAction += GameInput_OnInteractionAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        HandleMove();
        //目的是每帧记录lastDir
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void GameInput_OnInteractionAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.Is_GamePlaying()) return;
        RaycastInteract();
        //E被选择的柜台进行交互
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.Is_GamePlaying()) return;
        RaycastInteract();
        //F被选择的柜台进行交互
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleMove()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = (moveDir != Vector3.zero);
        float rotationSpeed = 15f;
        transform.forward = Vector3.Lerp(transform.forward, -moveDir, Time.deltaTime * rotationSpeed);
        //投射检测
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveSpeed * Time.deltaTime);
        //解决对角线问题
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveSpeed * Time.deltaTime);
            if (!canMove)
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveSpeed * Time.deltaTime);
                if (canMove) { moveDir = moveDirZ; }
            }
            else { moveDir = moveDirX; }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero) { lastInteractDir = moveDir; }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    private void RaycastInteract()
    {
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                Debug.Log("按E检测到了柜子");
                SetSelectedCounter(baseCounter);
            }
            else { SetSelectedCounter(null); }
        }
        else { SetSelectedCounter(null); }
    }

    //玩家和柜台的交互是通过接口来实现的，玩家和柜台都实现了IKitchenObjParent接口，这样可以让玩家和柜台之间进行厨房物体的传递和管理。
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