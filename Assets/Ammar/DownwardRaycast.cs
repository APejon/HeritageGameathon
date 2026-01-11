using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DownwardRaycast : MonoBehaviour
{
    [SerializeField] private CinemachineCamera isometricCamera;
    [SerializeField] private SpriteRenderer _promptButton;
    [SerializeField] private Image _UpArrow;
    [SerializeField] private Image _LeftArrow;
    [SerializeField] private Image _DownArrow;
    [SerializeField] private Image _RightArrow;
    [SerializeField] private Image _TrackImage;
    [SerializeField] private Image _CompassImage;
    [SerializeField] private Sprite[] _TrackImagesRef;
    [SerializeField] private CanvasGroup _Compass;
    [SerializeField] private Animator falconAnimator;
    private EventCollision.events _eventType;
    private RaycastHit _hit;
    private CharacterMovement _movementRef;
    private EventCollision.plantType _plantType;
    private ResourceBars _resources;
    private EventCollision _tileRefrence;
    private EventCollision.trackDirection _trackDirection;
    private bool _tracked;
    private EventCollision.trackType _trackType;
    private float initialOrthographicSize;

    private void Awake()
    {
        initialOrthographicSize = Camera.main.orthographicSize;
    }

    private void Start()
    {
        _resources = GetComponent<ResourceBars>();
        _UpArrow.DOFade(0, 0.5f);
        _LeftArrow.DOFade(0, 0.5f);
        _DownArrow.DOFade(0, 0.5f);
        _RightArrow.DOFade(0, 0.5f);
        _TrackImage.DOFade(0, 0.5f);
        _movementRef = GetComponent<CharacterMovement>();
        _Compass.DOFade(0f, 1);
    }

    private void Update()
    {
        if (CheckEventType() && Keyboard.current.fKey.wasPressedThisFrame)
        {
            promptPressed();
        }
    }

    private bool CheckEventType()
    {
        if (_eventType == EventCollision.events.TrackEnd || _eventType == EventCollision.events.Plant)
        {
            return true;
        }

        return false;
    }

    public void castARay()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2f))
        {
            _tileRefrence = _hit.collider.gameObject.GetComponent<EventCollision>();
            switch (_tileRefrence._event)
            {
                case EventCollision.events.Track:
                    _eventType = _tileRefrence._event;
                    _trackType = _tileRefrence._track;
                    ShowTrackImage();
                    ShowTrackDirection();
                    _tracked = true;
                    _Compass.DOFade(1, 0.5f);
                    AudioManager.instance.playSFX(AudioManager.soundEffect.ZOOM, true);
                    break;
                case EventCollision.events.TrackEnd:
                    _promptButton.DOFade(1, 0.5f);
                    _eventType = _tileRefrence._event;
                    _trackType = _tileRefrence._track;
                    break;
                case EventCollision.events.Plant:
                    _promptButton.DOFade(1, 0.5f);
                    _eventType = _tileRefrence._event;
                    _plantType = _tileRefrence._plant;
                    DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                            value => isometricCamera.Lens.OrthographicSize = value, 1.68f, 0.2f)
                        .OnComplete(() => _movementRef.onMove += zoomOut);
                    AudioManager.instance.playSFX(AudioManager.soundEffect.ZOOM, true);
                    break;
                case EventCollision.events.None:
                    _promptButton.DOFade(0, 0.5f);
                    _eventType = _tileRefrence._event;
                    break;
            }

            if (_tracked && _eventType != EventCollision.events.Track)
            {
                Debug.Log(_eventType);
                _tracked = false;
                _movementRef.onMove += compassDisappear;
                ResetTrackImages();
            }
        }
    }

    public void zoomOut()
    {
        _movementRef.onMove -= zoomOut;
        DOTween.To(() => isometricCamera.Lens.OrthographicSize,
            value => isometricCamera.Lens.OrthographicSize = value, initialOrthographicSize, 0.2f);
    }

    public void compassDisappear()
    {
        _movementRef.onMove -= compassDisappear;
        _Compass.DOFade(0, 0.5f);
    }

    private void ShowTrackImage()
    {
        switch (_tileRefrence._track)
        {
            case EventCollision.trackType.Camel:
                _TrackImage.sprite = _TrackImagesRef[(int)target.CAMEL];
                break;
            case EventCollision.trackType.Falcon:
                _TrackImage.sprite = _TrackImagesRef[(int)target.FALCON];
                break;
            case EventCollision.trackType.Well:
                _TrackImage.sprite = _TrackImagesRef[(int)target.WELL];
                break;
            case EventCollision.trackType.Oasis:
                _TrackImage.sprite = _TrackImagesRef[(int)target.OASIS];
                break;
        }

        _TrackImage.DOFade(1, 0.5f);
    }

    private void ShowTrackDirection()
    {
        switch (_tileRefrence._trackDirection)
        {
            case EventCollision.trackDirection.Up:
                _UpArrow.DOFade(1, 0.5f);
                _CompassImage.rectTransform.DOLocalRotate(new Vector3(0, 0, -185), 1f);
                break;
            case EventCollision.trackDirection.Left:
                _LeftArrow.DOFade(1, 0.5f);
                _CompassImage.rectTransform.DOLocalRotate(new Vector3(0, 0, -88), 1f);
                break;
            case EventCollision.trackDirection.Down:
                _DownArrow.DOFade(1, 0.5f);
                _CompassImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 2), 1f);
                break;
            case EventCollision.trackDirection.Right:
                _RightArrow.DOFade(1, 0.5f);
                _CompassImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 90), 1f);
                break;
        }
    }

    private void ResetTrackImages()
    {
        _UpArrow.DOFade(0, 0.5f);
        _LeftArrow.DOFade(0, 0.5f);
        _DownArrow.DOFade(0, 0.5f);
        _RightArrow.DOFade(0, 0.5f);
        _TrackImage.DOFade(0, 0.5f);
    }

    private void promptPressed()
    {
        if (_eventType == EventCollision.events.TrackEnd)
        {
            switch (_trackType)
            {
                case EventCollision.trackType.Camel:
                    _resources.increaseResource(ResourceBars.stat.Health, 40);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    break;
                case EventCollision.trackType.Falcon:
                    _resources.increaseResource(ResourceBars.stat.Hunger, 40);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    AudioManager.instance.playSFX(AudioManager.soundEffect.FALCON, true);
                    var birds = Physics.OverlapBox(transform.position, Vector3.one * 1.75f / 2f);
                    foreach (var collider in birds)
                    {
                        if (collider.CompareTag("Falcon"))
                        {
                            var pos = collider.transform.position;
                            pos.y = -1.1f;
                            collider.transform.position = pos;
                        }
                    }

                    falconAnimator.gameObject.SetActive(true);
                    falconAnimator.CrossFade("Landing", 0f);
                    DOVirtual.DelayedCall(4, () => falconAnimator.gameObject.SetActive(false));
                    break;
                case EventCollision.trackType.Oasis:
                    _resources.increaseResource(ResourceBars.stat.Hunger, 20);
                    _resources.increaseResource(ResourceBars.stat.Hydration, 20);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    break;
                case EventCollision.trackType.Well:
                    _resources.increaseResource(ResourceBars.stat.Hydration, 40);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    AudioManager.instance.playSFX(AudioManager.soundEffect.DRINK, true);
                    var colliders = Physics.OverlapBox(transform.position, Vector3.one * 1.75f / 2f);
                    foreach (var collider in colliders)
                    {
                        if (collider.CompareTag("Well"))
                        {
                            foreach (Transform child in collider.gameObject.transform)
                            {
                                if (child.name == "Circle")
                                {
                                    child.gameObject.SetActive(false);
                                }
                            }
                        }
                    }

                    break;
            }
        }

        if (_eventType == EventCollision.events.Plant)
        {
            AudioManager.instance.playSFX(AudioManager.soundEffect.EAT, true);
            switch (_plantType)
            {
                case EventCollision.plantType.Medicinal:
                    _resources.increaseResource(ResourceBars.stat.Health, 10);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    break;
                case EventCollision.plantType.Poisonous:
                    _resources.decreaseResource(ResourceBars.stat.Health, 10);
                    _tileRefrence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.DOFade(0, 0.5f);
                    break;
            }

            var colliders = Physics.OverlapBox(transform.position, Vector3.one * 1.75f / 2f);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Flower"))
                {
                    collider.gameObject.SetActive(false);
                }
            }
        }
    }

    private enum target
    {
        CAMEL,
        FALCON,
        WELL,
        OASIS
    }
}
