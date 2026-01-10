using System;
using System.Diagnostics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DownwardRaycast : MonoBehaviour
{
    private RaycastHit      _hit;
    private EventCollision  _tileRefence;
    private EventCollision.events    _eventType;
    private EventCollision.trackType _trackType;
    private EventCollision.trackDirection _trackDirection;
    private ResourceBars _resources;
    private Boolean _tracked;
    [SerializeField] GameObject _promptButton;
    [SerializeField] Image _UpArrow;
    [SerializeField] Image _LeftArrow;
    [SerializeField] Image _DownArrow;
    [SerializeField] Image _RightArrow;
    [SerializeField] Image _TrackImage;
    [SerializeField] Image _CompassImage;
    [SerializeField] Sprite[] _TrackImagesRef;
    private enum target
    {
        CAMEL,
        FALCON,
        WELL,
        OASIS
    }

    void Start()
    {
        _resources = GetComponent<ResourceBars>();
        _UpArrow.DOFade(0, 0.5f);
        _LeftArrow.DOFade(0, 0.5f);
        _DownArrow.DOFade(0, 0.5f);
        _RightArrow.DOFade(0, 0.5f);
        _TrackImage.DOFade(0, 0.5f);
    }

    void Update()
    {
        if (CheckEventType() && Keyboard.current.fKey.wasPressedThisFrame)
            promptPressed();

    }

    Boolean CheckEventType()
    {
        if (_eventType == EventCollision.events.TrackEnd)
            return true;
        else
            return false;
    }

    public void castARay()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2f))
        {
            _tileRefence = _hit.collider.gameObject.GetComponent<EventCollision>();
            switch(_tileRefence._event)
            {
                case EventCollision.events.Track:
                    _trackType = _tileRefence._track;
                    ShowTrackImage();
                    ShowTrackDirection();
                    _tracked = true;
                    break;
                case EventCollision.events.TrackEnd:
                    _promptButton.SetActive(true);
                    _eventType = _tileRefence._event;
                    _trackType = _tileRefence._track;
                    break;
                default:
                    _promptButton.SetActive(true);
                    _eventType = _tileRefence._event;
                    break;

            }
            if (_tileRefence._event != EventCollision.events.TrackEnd) // MAKE SURE IT AFFECTS OTHER INTERACTABLES TOO
            {
                _promptButton.SetActive(false);
            }
            if (_tracked && _tileRefence._event != EventCollision.events.Track)
            {
                _tracked = false;
                ResetTrackImages();
            }
        }
    }

    private void ShowTrackImage()
    {
        switch(_tileRefence._track)
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
        switch(_tileRefence._trackDirection)
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
            switch(_trackType)
            {
                case EventCollision.trackType.Well:
                    _resources.increaseResource(ResourceBars.stat.Hydration, 20);
                    _tileRefence._event = EventCollision.events.None;
                    _eventType = EventCollision.events.None;
                    _promptButton.SetActive(false);
                    var colliders = Physics.OverlapBox(transform.position, Vector3.one * 1.75f / 2f);
                    foreach (var collider in colliders)
                    {
                        if (collider.CompareTag("Well"))
                        {
                            foreach (Transform child in collider.gameObject.transform)
                            {
                                if (child.name == "Circle")
                                    child.gameObject.SetActive(false);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
