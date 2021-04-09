using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all entities in the game, player, enemies, NPCs etc... 
/// Abstract so it isn't used by mistake and inherits from MonoBehaviour so all 
/// children classes can be used as components in editor
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
