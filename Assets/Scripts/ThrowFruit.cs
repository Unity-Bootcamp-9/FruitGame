using UnityEngine;

public class ThrowFruit : MonoBehaviour
{
    [SerializeField]
    private SwipeEventAsset _swipeEventAsset;

    [Range(0f, 1f)]
    public float _minYThreshold;
    [Range(0f, 1f)]
    public float _maxXThreshold;

    public GameManager gameManager;

    private Rigidbody _rigidbody;

    [SerializeField]
    private float launchAngle = 45f;

    // TODO : UI에 Speed 조절용 Slider 필요.
    public float speed = 4f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _swipeEventAsset.eventRaised += Throw;
    }

    private void OnDisable()
    {
        _swipeEventAsset.eventRaised -= Throw;
    }

    private void Throw(object sender, SwipeData args)
    {
        // 스와이프 좌우제한은 Screen 기준으로 제한 //
        gameManager.checkDropText.text = $"y: {args.ScreenDirection.y >= _minYThreshold} x: {Mathf.Abs(args.ScreenDirection.x) <= _maxXThreshold}";
        gameManager.direction.text = $"{args.ScreenDirection} / ABS x: {Mathf.Abs(args.ScreenDirection.x)}";
        if (args.ScreenDirection.y < _minYThreshold || Mathf.Abs(args.ScreenDirection.x) > _maxXThreshold)
        {
            return;
        }

        // 투척거리 계산은 World기준으로 계산 //
        float force = args.Distance / args.Duration;

        // 각도를 라디안으로 변환
        float angleInRadians = launchAngle * Mathf.Deg2Rad;

        // xz 평면에서의 속도
        float horizontalSpeed = Mathf.Cos(angleInRadians);

        // y 방향 속도
        float verticalSpeed = Mathf.Sin(angleInRadians);

        Vector3 launchVelocity = Camera.main.transform.forward + horizontalSpeed * verticalSpeed * Camera.main.transform.up;

        _rigidbody.useGravity = true;
        _rigidbody.AddForce(launchVelocity * force * speed, ForceMode.Impulse);

        gameObject.transform.SetParent(transform.root);

        // 한 번만 Throw 되도록 이벤트 구독 해제
        _swipeEventAsset.eventRaised -= Throw;
    }
}
