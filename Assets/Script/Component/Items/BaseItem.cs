using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum ItemType
{
    Exp,
    Money,
    Health
}

public class BaseItem : MonoBehaviour
{
    public ItemType Type;
    public int amount;

    private float _speed = 10f;
    private float _timelife = 60f;
    private float _curTime = 0;
    private Coroutine _coroutine;
    private MeshRenderer _mesh;

    private void Awake()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();
    }

    private void OnDisable()
    {
        _coroutine = null;
        _curTime = 0;
    }

    public virtual void SetAmount(int level)
    {

    }

    public virtual void SetAmount(BaseCharacter baseCharacter)
    {

    }

    private void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime >= _timelife)
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(TimeOut());
            }
        }
    }

    public void MoveTo(BaseCharacter baseCharacter)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _curTime = 0;
        _coroutine = StartCoroutine(MoveToTarget(baseCharacter, _speed));
    }

    private IEnumerator MoveToTarget(BaseCharacter target, float moveSpeed)
    {
        _mesh.enabled = true;
        while (true)
        {
            yield return null;
            Vector3 targetPosition = target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(targetPosition, transform.position) == 0)
            {
                ApplyItem(target);
                ItemsDrop.Instance.ReleaseItem(this);
                yield break;
            }
        }
    }

    private IEnumerator TimeOut()
    {
        yield return null;
        for (int i = 0; i < 5; i++)
        {
            _mesh.enabled = false;
            yield return new WaitForSeconds(0.2f);
            _mesh.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        ItemsDrop.Instance.ReleaseItem(this);
    }

    private void ApplyItem(BaseCharacter target)
    {
        switch (Type)
        {
            case ItemType.Money:
                UserMain.Instance.MoneyAmount += amount;
                break;
            case ItemType.Exp:
                UserMain.Instance.Exp += amount;
                break;
            case ItemType.Health:
                target.Health += amount;
                break;
        }
    }
}
