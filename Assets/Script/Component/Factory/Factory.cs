using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] public ArmyFactory armyFactory;
    [SerializeField] public EnemyFactory enemyFactory;
    [SerializeField] public WeaponFactory weaponFactory;
    [SerializeField] public AbilitieFactory abilitieFactory;
    [SerializeField] public VFXFactory vFXFactory;

    private static Factory _instance;
    public static Factory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Factory>();
            }

            return _instance;
        }
    }
}