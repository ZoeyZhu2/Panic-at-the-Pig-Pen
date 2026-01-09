using UnityEngine;
using UnityEngine.InputSystem;

public class NetMoveScript : MonoBehaviour
{
    [SerializeField] Rigidbody2D netRB;
    private GameInputActions inputActions;

    private float moveInput = 0f;
    [SerializeField] private float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputActions = new GameInputActions();
    }
    
    void Start()
    {
    }
    
    void OnEnable()
    {
        inputActions.GamePlay.Enable();
        inputActions.GamePlay.MoveLeft.performed += ctx => moveInput = -1f;
        inputActions.GamePlay.MoveLeft.canceled += ctx => moveInput = 0f;
        inputActions.GamePlay.MoveRight.performed += ctx => moveInput = 1f;
        inputActions.GamePlay.MoveRight.canceled += ctx => moveInput = 0f;
    }

    void OnDisable()
    {
        inputActions.GamePlay.MoveLeft.performed -= ctx => moveInput = -1f;
        inputActions.GamePlay.MoveLeft.canceled -= ctx => moveInput = 0f;
        inputActions.GamePlay.MoveRight.performed -= ctx => moveInput = 1f;
        inputActions.GamePlay.MoveRight.canceled -= ctx => moveInput = 0f;
        inputActions.GamePlay.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current != null)
        {
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                if (touchPosition.x < Screen.width / 2)
                {
                    moveInput = -1f;
                }
                else
                {
                    moveInput = 1f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        netRB.linearVelocity = new Vector2(moveInput * moveSpeed, 0);
    }


}
