using System;
using UnityEngine;

public class ArmyFactory : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private ArmyUser _armyUser;
    [SerializeField] private ArmyNPC _armyNPC;
    [SerializeField] private GameObject _armyPrefab;

    private Camera _cameraPlayer;
    private ArmysDataModel _armysData;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var json = Resources.Load<TextAsset>("army.data");
        _armysData = JsonUtility.FromJson<ArmysDataModel>(json.text);
        _cameraPlayer = Game.Instance.MainCamera;
    }

    public BaseCharacter CreateArmy(Vector3 position, ArmyType armyType, bool isPlayer)
    {
        // army data
        var armyData = GetArmyData(armyType);
        var jsonData = JsonUtility.ToJson(armyData);

        BaseCharacter prefab = isPlayer ? _armyUser : _armyNPC;

        // add base prefab
        BaseCharacter character = Instantiate(prefab);
        character.name = character.GetInstanceID().ToString();
        character.transform.parent = _container.transform;

        // add model army
        GameObject model = Instantiate(_armyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        model.transform.parent = character.transform;

        if (isPlayer)
        {
            _cameraPlayer.transform.parent = character.transform;
        }

        character.transform.position = position;
        character.Initialize(Factions.Army, jsonData);
        return character;
    }

    private ArmyData GetArmyData(ArmyType enemyType)
    {
        return _armysData.armys.Find(e => e.type == enemyType);
    }
}