using System;
using UnityEngine;
using UnityEngine.UIElements;

public struct SwipeData
{
    // world vector
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float Distance;
    public Vector2 Direction;
    // screen vector
    public Vector3 StartScreenPosition;
    public Vector3 EndScreenPosition;
    public float ScreenDistance;
    public Vector2 ScreenDirection;
    // time
    public float Duration;
}

public class SwipeDetector : MonoBehaviour
{
    [SerializeField]
    private SwipeEventAsset _swipeEventAsset;

    [SerializeField]
    private float minimumDistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;

    private SwipeEventController _swipeEventController;

    // tocuh time
    private float startTime;
    private float endTime;

    // world vector
    private Vector2 startPosition;
    private Vector2 endPosition;

    // screen vector
    public Vector3 StartScreenPosition;
    public Vector3 EndScreenPosition;

    private void Awake()
    {
        _swipeEventController = GetComponent<SwipeEventController>();
    }

    private void OnEnable()
    {
        _swipeEventController.OnStartTouch += SwipeStart;
        _swipeEventController.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        _swipeEventController.OnStartTouch -= SwipeStart;
        _swipeEventController.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        // Start position in world coordinates with a proper z value
        Vector3 startWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        startPosition = new Vector2(startWorldPosition.x, startWorldPosition.y);
        StartScreenPosition = position;
        startTime = time;

        //Debug.Log($"{nameof(SwipeStart)}, Pos : {position}, Time : {time}");
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        // End position in world coordinates with a proper z value
        Vector3 endWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane));
        endPosition = new Vector2(endWorldPosition.x, endWorldPosition.y);
        EndScreenPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        float distance = Vector2.Distance(startPosition, endPosition);
        float screenDistance = Vector2.Distance(StartScreenPosition, EndScreenPosition);
        float duration = endTime - startTime;

        Debug.Log($"Dist: {distance}");
        Debug.Log($"Duration: {duration}");
        if (distance >= minimumDistance && duration <= maximumTime)
        {
            Vector3 direction = (endPosition - startPosition).normalized;
            Vector3 screenDirection = (EndScreenPosition - StartScreenPosition).normalized;

            var data = new SwipeData()
            {
                StartPosition = new Vector3(startPosition.x, startPosition.y, 0),
                EndPosition = new Vector3(endPosition.x, endPosition.y, 0),
                Distance = distance,
                Direction = direction,
                Duration = duration,
                StartScreenPosition = StartScreenPosition,
                EndScreenPosition = EndScreenPosition,
                ScreenDistance = screenDistance,
                ScreenDirection = screenDirection
            };

            _swipeEventAsset.Raise(data);
        }
    }
}
