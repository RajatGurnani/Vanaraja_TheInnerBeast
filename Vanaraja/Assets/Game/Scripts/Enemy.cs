using System.Collections;
using UnityEngine;

/// <summary>
/// The class to define enemy behaviour
/// </summary>
public class Enemy : MonoBehaviour
{
    public float detectionRange = 1.0f;
    public float moveSpeed = 10f;
    public float maxTurnRadius = 10f;

    public Animator animator;
    public AudioSource audioSource;
    public CharacterController characterController;
    Coroutine coroutine;

    [Tooltip("The time after which the enemy moves in random direction")]
    [Range(1f, 3f)] public float turnTime;
    public Quaternion moveDirection;
    public Vector3 movePoint = Vector3.zero;
    public float moveDistance;
    public Transform graphic;

    IEnumerator RandomMoveDirection()
    {
        while (true)
        {
            Vector2 temp = moveDistance * Random.insideUnitCircle;
            movePoint = transform.position + new Vector3(Mathf.Abs(temp.x), 0, Mathf.Abs(temp.y));
            moveDirection = Quaternion.LookRotation((movePoint - transform.position).normalized, Vector3.up);
            yield return new WaitForSeconds(Random.Range(0, turnTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            gameObject.CompareTag(Tags.Player);
        }
    }

    private void Update()
    {
        graphic.rotation = Quaternion.RotateTowards(graphic.rotation, moveDirection, maxTurnRadius * Time.deltaTime);
        characterController.Move(transform.forward * Time.deltaTime);
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(nameof(RandomMoveDirection));
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
