using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private EnemySpawnController _enemySpawn;

    private void Awake()
    {
        MapController.Instance.GenerateMap(0);
    }

    void Start()
    {
        var mapSize = MapController.Instance.mapLevel.size;
        var posDrop = new Vector3(mapSize.x / 2, 0, mapSize.y / 2);

        var armyUser = Factory.Instance.armyFactory.CreateArmy(posDrop, ArmyType.Soldier, true);
        var armyUserWeapon = Factory.Instance.weaponFactory.GetGunWeapon(WeaponType.SubmachineGuns, "ump45");
        armyUser.EquipWeapon(armyUserWeapon);

        for (int i = 0; i < 1; i++)
        {
            var posMember = GameUtils.RandomAroundPosition(posDrop, 3);
            var armyNPC = Factory.Instance.armyFactory.CreateArmy(posMember, ArmyType.Soldier, false);
            var armyNPCWeapon = Factory.Instance.weaponFactory.GetGunWeapon(WeaponType.SubmachineGuns, "ump45");
            armyNPC.EquipWeapon(armyNPCWeapon);
            var npc = armyNPC as ArmyNPC;

            npc.SetLeader(armyUser);
        }

        // test drop items
        for (int i = 0; i < 0; i++)
        {
            var posRandom = new Vector3(
                armyUser.transform.position.x + Random.Range(-10, 10),
                0,
                armyUser.transform.position.z + Random.Range(-10, 10)
            );

            ItemsDrop.Instance.DropItem(posRandom);
        }

        _enemySpawn.InitializeStartGame(armyUser);
    }
}
