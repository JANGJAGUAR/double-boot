using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage1Camera : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _moveTime;
    [SerializeField] private GameObject _centerCursor;

    private Transform _originTransform;
    private Transform _destTransform;

    private bool _movingCamera = false;
    private float _time = 0f;

    private void Start()
    {
        _centerCursor.SetActive(false);
    }

    private void Update()
    {
        if (_movingCamera)
        {
            _time += Time.deltaTime;
            transform.SetPositionAndRotation(Vector3.Lerp(_originTransform.position, _destTransform.position, _time / _moveTime), Quaternion.Slerp(_originTransform.rotation, _destTransform.rotation, _time / _moveTime));
            if (_time >= _moveTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnOpenDoor()
    {
        _originTransform = transform;
        _destTransform = _mainCamera.transform;
        _movingCamera = true;
    }

    private void OnDestroy()
    {
        _centerCursor.SetActive(true);
    }
}
