using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    private ScreenManager _screenManager;
    private Vector3Int _screenPosition;
    private Vector3Int _screenDir;

    public Screen LeftScreen { private set; get; }
    public Screen RightScreen { private set; get; }
    public Screen UpScreen { private set; get; }
    public Screen DownScreen { private set; get; }

    public bool IsDetatched;

    private void Start()
    {
        _screenManager = FindObjectOfType<ScreenManager>();

        AddToDict();
    }

    private void Update()
    {
        if (IsDetatched)
        {
            LeftScreen = null;
            RightScreen = null;
            UpScreen = null;
            DownScreen = null;
        }
        else
        {
            _screenPosition = Vector3Int.RoundToInt(transform.position);
            _screenDir = Vector3Int.RoundToInt(transform.rotation * Vector3.back);
            UpdateConnectedScreens();
        }
    }

    private void UpdateConnectedScreens()
    {
        // 위
        if (_screenManager.ScreenDict.TryGetValue((_screenPosition + Vector3Int.up, _screenDir), out Screen otherScreen))
        {
            UpScreen = otherScreen;
        }
        else
        {
            UpScreen = null;
        }

        // 아래
        if (_screenManager.ScreenDict.TryGetValue((_screenPosition + Vector3Int.down, _screenDir), out otherScreen))
        {
            DownScreen = otherScreen;
        }
        else
        {
            DownScreen = null;
        }

        Vector3Int left = Vector3Int.RoundToInt(Vector3.Cross(Vector3.up, _screenDir));

        // 왼쪽
        if (_screenManager.ScreenDict.TryGetValue((_screenPosition + _screenDir + left, -left), out otherScreen))
        {
            LeftScreen = otherScreen;
        }
        else if (_screenManager.ScreenDict.TryGetValue((_screenPosition + left, _screenDir), out otherScreen))
        {
            LeftScreen = otherScreen;
        }
        else if (_screenManager.ScreenDict.TryGetValue((_screenPosition, left), out otherScreen))
        {
            LeftScreen = otherScreen;
        }
        else
        {
            LeftScreen = null;
        }

        // 오른쪽
        if (_screenManager.ScreenDict.TryGetValue((_screenPosition + _screenDir - left, left), out otherScreen))
        {
            RightScreen = otherScreen;
        }
        else if (_screenManager.ScreenDict.TryGetValue((_screenPosition - left, _screenDir), out otherScreen))
        {
            RightScreen = otherScreen;
        }
        else if (_screenManager.ScreenDict.TryGetValue((_screenPosition, -left), out otherScreen))
        {
            RightScreen = otherScreen;
        }
        else
        {
            RightScreen = null;
        }
    }

    public void RemoveFromDict()
    {
        if (!_screenManager.ScreenDict.Remove((_screenPosition, _screenDir)))
        {

        }
    }

    public void AddToDict()
    {
        _screenPosition = Vector3Int.RoundToInt(transform.position);
        _screenDir = Vector3Int.RoundToInt(transform.rotation * Vector3.back);
        _screenManager.ScreenDict.Add((_screenPosition, _screenDir), this);
    }

    public bool TryAddToDict()
    {
        _screenPosition = Vector3Int.RoundToInt(transform.position);
        _screenDir = Vector3Int.RoundToInt(transform.rotation * Vector3.back);
        if (_screenManager.ScreenDict.TryAdd((_screenPosition, _screenDir), this))
        {
            return true;
        }

        return false;
    }
}
