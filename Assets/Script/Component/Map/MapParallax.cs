using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapParallax : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private Vector2 terrainSize;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private Transform _character;
    private GameObject[,] _terrains;
    private Vector3 _lastPlayerPosition;

    public void InitParallax(Vector2 size)
    {
        terrainSize = size;
        CreateInitialTerrains();
    }

    public void InitCharacter(BaseCharacter character)
    {
        _character = character.transform;
        _lastPlayerPosition = _character.position;
    }

    void Update()
    {
        if (_character == null) return;

        Vector3 playerOffset = _character.position - _lastPlayerPosition;
        if (Mathf.Abs(playerOffset.x) >= terrainSize.x - (terrainSize.x / 4) ||
            Mathf.Abs(playerOffset.z) >= terrainSize.y - (terrainSize.y / 4))
        {
            UpdateTerrains(playerOffset);
            _lastPlayerPosition = _character.position;
        }
    }

    void CreateInitialTerrains()
    {
        _terrains = new GameObject[3, 3];
        Vector3 startPosition = new Vector3(-terrainSize.x, 0, -terrainSize.y);
        for (int x = 0; x < 3; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                Vector3 position = startPosition + new Vector3(x * terrainSize.x, 0, z * terrainSize.y);
                var map = Instantiate(mapGenerator, position, Quaternion.identity);
                map.GenerateMap();
                _terrains[x, z] = map.gameObject;

                // last terrains
                if (x == 2 && z == 2)
                {
                    _navMeshSurface.BuildNavMesh();
                }
            }
        }
    }

    void UpdateTerrains(Vector3 playerOffset)
    {
        int offsetX = Mathf.RoundToInt(playerOffset.x / terrainSize.x);
        int offsetZ = Mathf.RoundToInt(playerOffset.z / terrainSize.y);

        if (offsetX != 0)
        {
            for (int z = 0; z < 3; z++)
            {
                if (offsetX == 1)
                {
                    GameObject temp = _terrains[0, z];
                    temp.transform.position += new Vector3(3 * terrainSize.x, 0, 0);
                    for (int x = 0; x < 2; x++)
                    {
                        _terrains[x, z] = _terrains[x + 1, z];
                    }
                    _terrains[2, z] = temp;
                }
                else if (offsetX == -1)
                {
                    GameObject temp = _terrains[2, z];
                    temp.transform.position -= new Vector3(3 * terrainSize.x, 0, 0);
                    for (int x = 2; x > 0; x--)
                    {
                        _terrains[x, z] = _terrains[x - 1, z];
                    }
                    _terrains[0, z] = temp;
                }
            }
        }

        if (offsetZ != 0)
        {
            for (int x = 0; x < 3; x++)
            {
                if (offsetZ == 1)
                {
                    GameObject temp = _terrains[x, 0];
                    temp.transform.position += new Vector3(0, 0, 3 * terrainSize.y);
                    for (int z = 0; z < 2; z++)
                    {
                        _terrains[x, z] = _terrains[x, z + 1];
                    }
                    _terrains[x, 2] = temp;
                }
                else if (offsetZ == -1)
                {
                    GameObject temp = _terrains[x, 2];
                    temp.transform.position -= new Vector3(0, 0, 3 * terrainSize.y);
                    for (int z = 2; z > 0; z--)
                    {
                        _terrains[x, z] = _terrains[x, z - 1];
                    }
                    _terrains[x, 0] = temp;
                }
            }
        }

        _navMeshSurface.BuildNavMesh();
    }
}
