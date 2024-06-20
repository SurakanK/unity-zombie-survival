using UnityEngine;

public class ArmyBase : BaseCharacter
{
    public Rigidbody Rigidbody { get; set; }
    public Animator Animator { get; set; }
    public BodyParts BodyParts { get; set; }
    public ArmyData ArmyData;

    public override void Initialize(Factions factions, string jsonData)
    {
        base.Initialize(factions, jsonData);
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        BodyParts = GetComponentInChildren<BodyParts>();
        ArmyData = JsonUtility.FromJson<ArmyData>(jsonData);

        Defense = ArmyData.defense;
        Critical = ArmyData.critical;
        Evasion = ArmyData.evasion;
        Accuracy = ArmyData.accuracy;
    }

    public void ShootAnim(bool isAttack)
    {
        Animator.SetBool("IsAttack", isAttack);
    }

    public void RotateToTarget(BaseCharacter target)
    {
        Vector3 targetPostition = new Vector3(target.transform.position.x, Animator.transform.position.y, target.transform.position.z);
        Animator.transform.LookAt(targetPostition);
    }

    public void Rotation(Vector3 vector)
    {
        if (Weapon != null && Weapon?.Target != null) return;
        Animator.transform.rotation = Quaternion.LookRotation(vector);
    }
}