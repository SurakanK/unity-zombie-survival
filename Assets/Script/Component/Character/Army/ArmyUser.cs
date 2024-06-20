using UnityEngine;

public class ArmyUser : ArmyBase
{
    private FloatingJoystick _joyStick;
    private MagnetArea _magnetArea;

    private bool _isJoyStickPointerDown;

    public override void Initialize(Factions factions, string jsonData)
    {
        base.Initialize(factions, jsonData);

        _joyStick = FindObjectOfType<FloatingJoystick>();
        _joyStick.OnPointerDownEvent += OnPointerDown;
        _joyStick.OnPointerUpEvent += OnPointerUp;

        _magnetArea = GetComponentInChildren<MagnetArea>();
        _magnetArea.Initialize(this);

        gameObject.transform.tag = "user";

        MapController.Instance.InitCharacter(this);
        UserMain.Instance.Initialize(this);

        MoveSpeed = ArmyData.speed;
        MaxHealth = ArmyData.hp;
        Health = ArmyData.hp;
    }

    private void OnDisable()
    {
        // _joyStick.OnPointerDownEvent -= OnPointerDown;
        // _joyStick.OnPointerUpEvent -= OnPointerUp;
    }

    private void OnPointerDown()
    {
        _isJoyStickPointerDown = true;
    }

    private void OnPointerUp()
    {
        _isJoyStickPointerDown = false;
        Move(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (!_isJoyStickPointerDown || isCantMove) return;

        var joystick = new Vector2(_joyStick.Horizontal, _joyStick.Vertical);
        var direction = joystick.normalized;
        var velocity = new Vector3(direction.x * MoveSpeed, Rigidbody.velocity.y, direction.y * MoveSpeed);
        Move(velocity);
    }

    public override void Move(Vector3 velocity)
    {
        if (velocity.x != 0 || velocity.z != 0)
        {
            Rotation(velocity);
            Animator.SetBool("IsRun", true);
            Animator.speed = MoveSpeed / 5;
        }
        else
        {
            Animator.SetBool("IsRun", false);
            Animator.speed = 1;
        }

        Rigidbody.velocity = velocity;
    }

    public override void HealthUpdate(float health)
    {
        base.HealthUpdate(health);
        var value = health / MaxHealth;
        UserMain.Instance.UpdateHealth(value, Health, MaxHealth);
    }
}