using System;
using UnityEngine;

/// <summary>
/// Class to deal with level management
/// </summary>
public class LevelManager : MonoBehaviour
{
    //* Singleton Pattern
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    [SerializeField] private Bounds _levelBounds;
    private PlayerBrain _player;
    public Bounds LevelBounds => _levelBounds;
    public PlayerBrain Player { get => _player; set => _player = value; }

    private void Awake()
    {
        //* Make sure there isn't another level manager and have it not destroy on load
        if (_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(_instance);

        OnLevelLoad = SetNewLevel;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBrain>();

    }

    /// <summary>
    /// Method to set-up level bounds and reset player ref
    /// </summary>
    /// <param name="center"></param>
    /// <param name="extents"></param>
    private void SetNewLevel(Vector3 center, Vector3 extents)
    {
        _levelBounds.center = center;
        _levelBounds.extents = extents;

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBrain>();
        }

    }

    public static Action<Vector3, Vector3> OnLevelLoad;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_levelBounds.center, _levelBounds.extents);
    }
}
