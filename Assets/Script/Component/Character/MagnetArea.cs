using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetArea : MonoBehaviour
{
    public BaseCharacter owner;

    public void Initialize(BaseCharacter _owner)
    {
        owner = _owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("item"))
        {
            var item = other.GetComponent<BaseItem>();
            item.MoveTo(owner);
        }
    }
}
