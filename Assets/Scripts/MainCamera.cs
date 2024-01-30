using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    enum Hands
    {
        Empty,
        Screen,
        Cube
    }
    public bool _isAttached;

    [SerializeField] private Player3D _player;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private GameObject _parentScreen;

    private Transform _holdingScreen;
    
    private RaycastHit hit;
    private Hands _hands;

    private void Start()
    {
        _hands = Hands.Empty;
    }

    void Update()
    {
        transform.position = _player.transform.position + Vector3.up * 0.5f;

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask2D = LayerMask.GetMask("2D");
        LayerMask mask3D = LayerMask.GetMask("3D");
        LayerMask maskCube = LayerMask.GetMask("CUBE");


        if (_hands == Hands.Empty)
        {
            // Empty
            if (Physics.Raycast(ray, out hit, 100.0f, mask2D))
            {
                //// 2Dscreen.light = On
            
                // detach
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 3.0f);
                    DetachScreen();
                }
            }

            else if (Physics.Raycast(ray, out hit, 100.0f, maskCube))
            {
                //// 3Dcuve.light = On
                
                // detach
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.green, 3.0f);
                    DetachCube();
                }
                
            }
        }
        
        // Screen
        else if (_hands == Hands.Screen)
        {   
            if (45f < transform.rotation.eulerAngles.x && transform.rotation.eulerAngles.x < 90f)
            {
                _holdingScreen.transform.localRotation = Quaternion.Euler(new Vector3(45f, 0f, 0f));
            }
            else
            {
                _holdingScreen.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            
            if (Physics.Raycast(ray, out hit, 100.0f, mask3D) && (hit.normal.y == 0) && hit.collider.tag !="WALL")
            {
                //// 투명스크린 = On
                
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.blue, 3.0f);
                    AttachScreen();
                }
            }
        }
        
        // Cube
        else if (_hands == Hands.Cube)
        {
            if (Physics.Raycast(ray, out hit, 100.0f, mask3D)) //// && 2D check 있는 곳엔 못 놓음
            {
                //// 투명큐브 = On
                
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.yellow, 3.0f);
                    AttachCube();
                }
            }
        }

    }
    
    private void DetachScreen()
    {
        var screen = hit.collider.GetComponent<Screen>();
        if (screen)
        {
            hit.collider.gameObject.transform.SetParent(_playerObj.transform);
            _holdingScreen = hit.collider.gameObject.transform;
            _holdingScreen.transform.localPosition = new Vector3(0f, -0.7f, 1.0f);
            screen.RemoveFromDict();
            screen.IsDetatched = true;
            _hands = Hands.Screen;
        }
    }
    private void AttachScreen()
    {
        var screen = _holdingScreen.GetComponent<Screen>();
        var originPos = _holdingScreen.position;
        var originRot = _holdingScreen.rotation;
        _holdingScreen.position = hit.collider.gameObject.transform.position;
        _holdingScreen.rotation = Quaternion.LookRotation(hit.normal * -1);
        if (screen.TryAddToDict())
        {
            _holdingScreen.SetParent(_parentScreen.transform);
            screen.IsDetatched = false;
            _hands = Hands.Empty;
        }
        else
        {
            _holdingScreen.position = originPos;
            _holdingScreen.rotation = originRot;
        }
    }
    private void DetachCube()
    {
        ////
        _hands = Hands.Cube;
    }
    private void AttachCube()
    {
        ////
        _hands = Hands.Empty;
    }
}
