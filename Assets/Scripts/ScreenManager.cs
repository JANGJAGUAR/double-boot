using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Dictionary<(Vector3Int pos, Vector3Int dir), Screen> ScreenDict;

    private void Awake()
    {
        ScreenDict = new();
    }
}