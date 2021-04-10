using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _box;

    public void Activate()
    {
        _box.enabled = true;
    }
    public void Deactivate()
    {
        _box.enabled = false;
    }
}
