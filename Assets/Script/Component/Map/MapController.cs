using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private MapParallax _parallax;

    [Header("building")]
    [SerializeField] public MapObjectData[] townObject;
    [Header("car")]
    [SerializeField] public MapObjectData[] carObject;
    [Header("pillar")]
    [SerializeField] public MapObjectData[] pillarObject;


    private MapsDataModel _mapData;
    private Vector2 _mapSize;

    public MapLevelModel mapLevel;
    public float[,] noiseMap;

    private static MapController _instance;
    public static MapController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MapController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        var json = Resources.Load<TextAsset>("map.data");
        _mapData = JsonUtility.FromJson<MapsDataModel>(json.text);
    }

    public void GenerateMap(int level)
    {
        mapLevel = _mapData.mapLevels[level];
        _mapSize = mapLevel.size.vector2;
        InitMapData(mapLevel.noise.vector2);
    }

    private void InitMapData(Vector2 noiceSize)
    {
        noiseMap = generateNoiseMap((int)_mapSize.x, (int)_mapSize.y, noiceSize.x, noiceSize.y);
        _parallax.InitParallax(_mapSize);
    }

    public void InitCharacter(BaseCharacter character)
    {
        _parallax.InitCharacter(character);
    }

    private float[,] generateNoiseMap(int sizeX, int sizeY, float scaleX, float scaleY)
    {
        float[,] noiseMap = new float[sizeX, sizeY];
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                float sampleX = x / scaleX;
                float sampleY = y / scaleY;
                float noise = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = noise;
            }
        }
        return noiseMap;
    }
}