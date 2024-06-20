using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{

    [Header("Floor")]
    [SerializeField] private Terrain _terrain;
    [Header("Container")]
    [SerializeField] private GameObject _objectContainer;

    private MapLevelModel _mapLevel;
    private Vector2 _mapSize;

    private float[,] _noiseMap;
    private int[,] _mapObjectData;


    public void GenerateMap()
    {
        _mapLevel = MapController.Instance.mapLevel;
        _noiseMap = MapController.Instance.noiseMap;
        _mapSize = _mapLevel.size.vector2;

        CreateMapData();
        PaintTerrainMap(_mapLevel.floorType);
        CreateMapObject();
    }

    public void CreateMapObject()
    {
        foreach (var mapObject in _mapLevel.mapObject)
        {
            var mapObjectType = System.Enum.Parse<MapObjectType>(mapObject.mapObjectType, true);
            switch (mapObjectType)
            {
                case MapObjectType.Grass:
                    PaintTerrainGrass(mapObject);
                    break;
                case MapObjectType.Tree:
                    PaintTerrainTree(mapObject);
                    break;
                case MapObjectType.Car:
                    CreateCar(mapObject);
                    break;
                case MapObjectType.Pillar:
                    CreatePillar(mapObject);
                    break;
                case MapObjectType.Town:
                    CreateTown(mapObject);
                    break;
            }
        }
    }

    private void PaintTerrainMap(int floorType)
    {
        _terrain.terrainData.size = new Vector3(_mapSize.x, 1, _mapSize.y);

        float[,,] floor = new float[_terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight, 5];

        for (int y = 0; y < floor.GetLength(1); y++)
        {
            for (int x = 0; x < floor.GetLength(0); x++)
            {
                floor[x, y, floorType] = 1;
            }
        }
        _terrain.terrainData.SetAlphamaps(0, 0, floor);
    }

    private void PaintTerrainGrass(MapObjectModel mapObject)
    {
        int[,] grass = _terrain.terrainData.GetDetailLayer(0, 0, _terrain.terrainData.detailWidth, _terrain.terrainData.detailHeight, 0);

        for (int y = 0; y < _terrain.terrainData.detailHeight; y++)
        {
            for (int x = 0; x < _terrain.terrainData.detailWidth; x++)
            {
                var indexX = (int)(x * _noiseMap.GetLength(1)) / _terrain.terrainData.detailHeight;
                var indexY = (int)(y * _noiseMap.GetLength(0)) / _terrain.terrainData.detailWidth;

                if (_noiseMap[indexX, indexY] > mapObject.area.x && _noiseMap[indexX, indexY] <= mapObject.area.y && Random.Range(0, mapObject.density) == 0)
                {
                    grass[x, y] = 1;
                }
                else
                {
                    grass[x, y] = 0;
                }
            }
        }

        _terrain.terrainData.SetDetailLayer(0, 0, 0, grass);
    }

    private void PaintTerrainTree(MapObjectModel mapObject)
    {
        removeTrees();
        if (mapObject.density == 0) return;

        for (float y = 0; y < _terrain.terrainData.heightmapResolution; y++)
        {
            for (float x = 0; x < _terrain.terrainData.heightmapResolution; x++)
            {

                var indexX = (int)(x * _noiseMap.GetLength(0)) / _terrain.terrainData.heightmapResolution;
                var indexY = (int)(y * _noiseMap.GetLength(1)) / _terrain.terrainData.heightmapResolution;

                if (_noiseMap[indexX, indexY] > mapObject.area.x && _noiseMap[indexX, indexY] <= mapObject.area.y && Random.Range(0, mapObject.density) == 0)
                {
                    Terrain terrain = GetComponent<Terrain>();
                    TreeInstance treeTemp = new TreeInstance();
                    treeTemp.position = new Vector3(y / _terrain.terrainData.heightmapResolution, 0, x / _terrain.terrainData.heightmapResolution);
                    treeTemp.prototypeIndex = Random.Range(0, 3);
                    treeTemp.widthScale = 1f;
                    treeTemp.heightScale = 1f;
                    treeTemp.color = Color.white;
                    treeTemp.lightmapColor = Color.white;
                    _terrain.AddTreeInstance(treeTemp);
                }
            }
        }
        _terrain.Flush();
    }

    private void removeTrees()
    {
        List<TreeInstance> instancesTmp = new List<TreeInstance>();
        _terrain.terrainData.treeInstances = instancesTmp.ToArray();
        float[,] heights = _terrain.terrainData.GetHeights(0, 0, 0, 0);
        _terrain.terrainData.SetHeights(0, 0, heights);
    }

    private void CreateTown(MapObjectModel mapObject)
    {
        // removeObject(mapObject.mapObjectType);
        if (mapObject.density == 0) { return; };
        for (int type = 0; type < mapObject.modelType.Count; type++)
        {
            var townData = MapController.Instance.townObject[mapObject.modelType[type]];
            createObject(townData, (int)mapObject.density, mapObject.area.vector2);
        }
    }

    private void CreateCar(MapObjectModel mapObject)
    {
        // removeObject(mapObject.mapObjectType);
        if (mapObject.density == 0) { return; };
        for (int type = 0; type < 2; type++)
        {
            var carData = MapController.Instance.carObject[type];
            createObject(carData, (int)mapObject.density, mapObject.area.vector2);
        }
    }

    private void CreatePillar(MapObjectModel mapObject)
    {
        // removeObject(mapObject.mapObjectType);
        if (mapObject.density == 0) { return; };
        for (int type = 0; type < mapObject.modelType.Count; type++)
        {
            var objectData = MapController.Instance.pillarObject[type];
            createObject(objectData, (int)mapObject.density, mapObject.area.vector2);
        }
    }

    private void createObject(MapObjectData buildingData, int density, Vector2 areaSize)
    {
        bool[,] arr = new bool[(int)_mapSize.x, (int)_mapSize.y];

        for (int y = 0; y < (int)_mapSize.y; y++)
        {
            for (int x = 0; x < (int)_mapSize.x; x++)
            {
                var indexX = (int)((x * _noiseMap.GetLength(1)) / _mapSize.x);
                var indexY = (int)((y * _noiseMap.GetLength(0)) / _mapSize.y);

                if (_noiseMap[indexY, indexX] > areaSize.x && _noiseMap[indexY, indexX] < areaSize.y)
                {
                    arr[x, y] = true;
                }
                else
                {
                    arr[x, y] = false;
                }
            }
        }

        for (int a = 0; a < arr.GetLength(0); a++)
        {
            for (int b = 0; b < arr.GetLength(1); b++)
            {
                var objectSizeX = buildingData.objectSize.x;
                var objectSizeY = buildingData.objectSize.y;
                var rotate = 0;

                switch (buildingData.objectTag)
                {
                    case MapObjectType.Town:
                        rotate = Random.Range(0, 2) == 0 ? 0 : 90;
                        objectSizeX = rotate == 0 ? buildingData.objectSize.x : buildingData.objectSize.y;
                        objectSizeY = rotate == 0 ? buildingData.objectSize.y : buildingData.objectSize.x;
                        break;
                    case MapObjectType.Car:
                        rotate = Random.Range(0, 360);
                        break;
                }

                if (a + objectSizeX < arr.GetLength(0) && a - objectSizeX > 0 && b + objectSizeY < arr.GetLength(1) && b - objectSizeY > 0)
                {
                    if (arr[a, b] == true && _mapObjectData[a, b] == (int)ObjectType.empty)
                    {
                        for (int i = -(int)objectSizeX; i < (int)objectSizeX; i++)
                        {
                            for (int j = -(int)objectSizeY; j < (int)objectSizeY; j++)
                            {
                                arr[a + i, b + j] = false;
                            }
                        }

                        if (Random.Range(0, density) == 0)
                        {
                            for (int i = -(int)objectSizeX; i < (int)objectSizeX; i++)
                            {
                                for (int j = -(int)objectSizeY; j < (int)objectSizeY; j++)
                                {
                                    _mapObjectData[a + i, b + j] = (int)ObjectType.noempty;
                                }
                            }

                            GameObject mapObject = Instantiate(buildingData.model, _objectContainer.transform);
                            mapObject.transform.localPosition = new Vector3(a, 0, b);
                            mapObject.transform.tag = buildingData.objectTag.ToString().ToLower();
                            mapObject.transform.Rotate(0, rotate, 0, Space.Self);
                        }
                    }

                }
            }
        }
    }

    private void removeObject(string objectTag)
    {
        var objects = GameObject.FindGameObjectsWithTag(objectTag);
        foreach (GameObject _object in objects)
        {
            GameObject.Destroy(_object);
        }
    }


    private void CreateMapData()
    {
        _mapObjectData = new int[(int)_mapSize.x, (int)_mapSize.y];

        for (int y = 0; y < (int)_mapSize.y; y++)
        {
            for (int x = 0; x < (int)_mapSize.x; x++)
            {
                _mapObjectData[x, y] = (int)ObjectType.empty;
            }
        }
    }

}

enum ObjectType
{
    empty,
    noempty,
}

public enum MapObjectType
{
    Grass,
    Tree,
    Car,
    Pillar,
    Town,
}

