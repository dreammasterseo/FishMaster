using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    public Transform HookedTransform;

    private Camera _mainCamera;
    private Collider2D _coll;

    private int _leght;
    private int _streenght;
    private int _fishCount;

    private bool _camMove;
    private List<Fish> _hocedFisher;
    private Tween _cameraTween;

    void Awake()
    {
        _mainCamera = Camera.main;
        _coll = GetComponent<Collider2D>();
        _hocedFisher = new List<Fish>();
    }


    void Update()
    {
        if (_camMove && Input.GetMouseButton(0))
        {
            Vector3 vector = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }

    public void StartFishing()
    {
        _leght = IdleManager.instance.Lenght - 20;
        _streenght = IdleManager.instance.Streng;
        _fishCount = 0;
        float time = (-_leght) * 0.1f;
        _cameraTween = _mainCamera.transform.DOMoveY(_leght, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y <= -18)
                transform.SetParent(_mainCamera.transform);
        }).OnComplete(delegate
        {
            _coll.enabled = true;
            _cameraTween = _mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (_mainCamera.transform.position.y >= -25f)
                    StopFishing();
            });
        });
        ScreenManager.instanc.ChangeScreen(Screens.Game);
        _coll.enabled = false;
        _camMove = true;
        _hocedFisher.Clear();
    }

    public void StopFishing()
    {
        _camMove = false;
        _cameraTween.Kill(false);
        _cameraTween = _mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (_mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -9);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 9;
            _coll.enabled = true;
            int num = 0;
            for(int i = 0; i < _hocedFisher.Count; i++)
            {
                _hocedFisher[i].transform.SetParent(null);
                _hocedFisher[i].ResetFish();
                num += _hocedFisher[i].Type.price; 
            }
            IdleManager.instance.TotalGain = num;
            ScreenManager.instanc.ChangeScreen(Screens.End);
        });
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Fish") && _fishCount != _streenght)
        {
            _fishCount++;
            Fish component = target.GetComponent<Fish>();
            component.Hooked();
            _hocedFisher.Add(component);
            target.transform.SetParent(transform);
            target.transform.position = HookedTransform.position;
            target.transform.rotation = HookedTransform.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;

            });
            if (_fishCount == _streenght)
                StopFishing();
        }
    }
}