using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeEventController : MonoBehaviour
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    [SerializeField]
    private SwipeInputActionReference _swipeInputActionReference;

    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (_swipeInputActionReference.PrimaryContact.action != null)
        {
            _swipeInputActionReference.PrimaryContact.action.started += ctx => StartTouchPrimary(ctx);
            _swipeInputActionReference.PrimaryContact.action.canceled += ctx => EndTouchPrimary(ctx);
        }

        _mainCamera = Camera.main;
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            if (context.control.device is not Pointer pointer)
            {
                Debug.LogError("Input actions are incorrectly configured. Expected a Pointer binding ScreenTapped.", this);
                return;
            }

            var tapPosition = pointer.position.ReadValue();
            //Debug.Log($"{nameof(StartTouchPrimary)} : {tapPosition}");

            OnStartTouch((tapPosition), (float)context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        Debug.Log(nameof(EndTouchPrimary));
        if (OnEndTouch != null)
        {
            if (context.control.device is not Pointer pointer)
            {
                Debug.LogError("Input actions are incorrectly configured. Expected a Pointer binding ScreenTapped.", this);
                return;
            }

            var tapPosition = pointer.position.ReadValue();
            //Debug.Log($"{nameof(EndTouchPrimary)} : {tapPosition}");

            OnEndTouch((tapPosition), (float)context.time);
        }
    }
}
