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

    public float HitPoints => _hitPoints;
    public float ArmorPoints => _armorPoints;
    public float BaseDamage => _baseDamage;
}
