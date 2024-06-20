using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private int enemyAmount;


    private Camera _camera;
    private FloatingJoystick _joyStick;
    private BaseCharacter _user;
    private List<EnemyNPC> _allEnemy = new List<EnemyNPC>();
    private float _frameDelay = 1f;

    private void Awake()
    {
        Initialize();
        OnEvent();
    }

    private void Initialize()
    {
        _joyStick = FindObjectOfType<FloatingJoystick>();
        _camera = Game.Instance.MainCamera;
    }

    private void OnEvent()
    {
        Factory.Instance.enemyFactory.EventReleaseEnemy += OnEventReleaseEnemy;
    }

    private void OnDisable()
    {
        // Factory.Instance.enemyFactory.EventReleaseEnemy -= OnEventReleaseEnemy;
    }

    private void OnEventReleaseEnemy(EnemyNPC enemy)
    {
        _allEnemy.Remove(enemy);
    }

    public void InitializeStartGame(BaseCharacter user)
    {
        _user = user;
        var posStart = _user.transform.position;

        for (int i = 0; i < (int)(enemyAmount / 10); i++)
        {
            var spawnPos = new Vector3(
                posStart.x + Random.Range(-10, 10),
                posStart.y,
                posStart.z + Random.Range(-20, 20)
            );

            SpawnEnemy(spawnPos);
        }
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return null;

        while (true)
        {
            CheckEnemyOutOfRange();
            AutoSpawnEnemy();
            yield return new WaitForSeconds(_frameDelay);
        }
    }

    private void CheckEnemyOutOfRange()
    {
        for (int i = _allEnemy.Count - 1; i >= 0; i--)
        {
            var enemy = _allEnemy[i];
            var distance = Vector3.Distance(enemy.transform.position, _user.transform.position);
            if (distance > 30)
            {
                Factory.Instance.enemyFactory.ReleaseEnemy(enemy);
            }
        }
    }

    private void AutoSpawnEnemy()
    {
        for (int i = _allEnemy.Count; i < enemyAmount; i++)
        {
            var spawnPos = GetRandomPositionOutsideCamera();
            SpawnEnemy(spawnPos);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        var randomSpawn = GameUtils.RandomWeighted(0, 7, 10);
        var typeSpawn = (EnemyType)randomSpawn;

        var enemy = Factory.Instance.enemyFactory.CreateEnemy(position, typeSpawn);

        if (!enemy.Weapon)
        {
            var enemyWeapon = Factory.Instance.weaponFactory.MeleeWeapons(WeaponType.MeleeWeapons, "hand");
            enemy.EquipWeapon(enemyWeapon);
        }

        _allEnemy.Add(enemy as EnemyNPC);
    }

    private Vector3 GetRandomPositionOutsideCamera()
    {
        Vector3 bottomLeft = new Vector3(0, 0, 11.5f);
        Vector3 topLeft = new Vector3(0, 1, 30);
        Vector3 bottomRight = new Vector3(1, 0, 11.5f);
        Vector3 topRight = new Vector3(1, 1, 30);

        // Convert viewport coordinates to world coordinates
        Vector3 worldBottomLeft = _camera.ViewportToWorldPoint(bottomLeft);
        Vector3 worldTopLeft = _camera.ViewportToWorldPoint(topLeft);
        Vector3 worldBottomRight = _camera.ViewportToWorldPoint(bottomRight);
        Vector3 worldTopRight = _camera.ViewportToWorldPoint(topRight);

        var radians = Mathf.Atan2(_joyStick.Vertical, _joyStick.Horizontal);
        float degrees = radians * Mathf.Rad2Deg;

        int offset = Random.Range(2, 15);
        float posX = 0;
        float posZ = 0;

        if (degrees == 0)
        {
            degrees = Random.Range(-180, 180);
        }

        if (degrees >= -45 && degrees <= 45)
        {
            // Right
            posX = worldTopRight.x + offset;
            posZ = Random.Range(worldBottomRight.z, worldTopRight.z);
        }
        else if (degrees > 45 && degrees <= 135)
        {
            // Up
            posX = Random.Range(worldTopLeft.x, worldTopRight.x);
            posZ = worldTopLeft.z + offset;
        }
        else if (degrees > 135 || degrees <= -135)
        {
            // Left
            posX = worldTopLeft.x - offset;
            posZ = Random.Range(worldBottomLeft.z, worldTopLeft.z);
        }
        else if (degrees > -135 && degrees < -45)
        {
            // Down
            posX = Random.Range(worldBottomLeft.x, worldBottomRight.x);
            posZ = worldBottomLeft.z - offset;
        }

        return new Vector3(posX, 0, posZ); ;
    }
}
