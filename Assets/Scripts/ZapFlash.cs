using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ZapFlash : MonoBehaviour
{
    [SerializeField] private float _activationTime;
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(WaitBeforeFlash());
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(new Vector2(Random.Range(0.5f, 1),
                            Random.Range(0.5f, 1)) * 400f, ForceMode2D.Impulse);
                print("Pushed player");

            }
        }
    }

    private IEnumerator WaitBeforeFlash()
    {
        yield return new WaitForSeconds(_activationTime);
        _animator.SetTrigger("Zap");
        StartCoroutine(WaitBeforeFlash());
    }
}
