using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the functionalities of the player object. It requires a 
/// Rigidbody2D and a CapsuleCollider2D components, so using this automatically 
/// adds them to the game object using this script
/// </summary>
[RequireComponent(typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public class PlayerBrain : Entity
{
    /// <summary>
    /// This is a reference to the rigidbody so we can access functionalities
    /// in code
    /// </summary>
    private Rigidbody2D _rb;
    
    /// <summary>
    /// The reference to the capsule collider to be use for hit detection
    /// </summary>
    private CapsuleCollider2D _hitCol;

}
