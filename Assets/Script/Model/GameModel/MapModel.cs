using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapObjectData
{
    [SerializeField] int _objectId;
    [SerializeField] MapObjectType _objectTag;
    [SerializeField] Vector2 _objectSize;
    [SerializeField] GameObject _model;

    public int objectId
    {
        get { return _objectId; }
    }
    public MapObjectType objectTag
    {
        get { return _objectTag; }
    }
    public Vector2 objectSize
    {
        get { return _objectSize; }
    }
    public GameObject model
    {
        get { return _model; }
    }
}

[Serializable]
public class MapsDataModel
{
    public List<MapLevelModel> mapLevels;
}

[Serializable]
public class MapLevelModel
{
    public int level;
    public int floorType;
    public SizeModel size;
    public SizeModel noise;
    public List<MapObjectModel> mapObject;
}

[Serializable]
public class MapObjectModel
{
    public string mapObjectType;
    public SizeModel area;
    public int density;
    public List<int> modelType;
}

[Serializable]
public class SizeModel
{
    public float x;
    public float y;

    public Vector2 vector2
    {
        get { return new Vector2(x, y); }
    }
}