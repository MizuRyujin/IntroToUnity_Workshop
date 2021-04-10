using UnityEngine;

/// <summary>
/// Scriptable Object to store base stats values of each entity
/// </summary>
[CreateAssetMenu(menuName = "Entity/New Entity Values")]
public class EntityBaseValues : ScriptableObject
{
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _armorPoints;
    [SerializeField] private float _baseDamage;
    [SerializeField] private float _speed;
    [SerializeField] private float _moveForce;

    public float HitPoints => _hitPoints;
    public float ArmorPoints => _armorPoints;
    public float BaseDamage => _baseDamage;
    public float Speed => _speed;
    public float MoveForce => _moveForce;
}
