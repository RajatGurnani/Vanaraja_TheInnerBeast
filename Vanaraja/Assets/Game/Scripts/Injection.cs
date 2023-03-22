using UnityEngine;

public class Injection : MonoBehaviour
{
    public Transform terrain;

    [Tooltip("The radius of check sphere")]
    public float radius;

    [Tooltip("The layers to be checked for collision against")]
    public LayerMask layer;

    [Tooltip("Max tries to search for a location without collision with trees or ground")]
    public int tryCount = 3;

    [Tooltip("The bounds in which the injection can spawn(-bounds, + bounds)")]
    public Vector3 bounds = Vector3.one;

    public GameObject graphic;
    public CapsuleCollider capsuleCollider;

    [Header("Rotation")]
    public bool canRotate = true;
    public float rotationSpeed = 10f;
    public Vector3 rotationDirection = Vector3.zero;

    private void Update()
    {
        if (canRotate)
        {
            transform.Rotate(rotationDirection, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        int count = 0;
        while (count <= tryCount)
        {
            Vector3 pos = new(Random.Range(-bounds.x, bounds.x), Random.Range(1f, bounds.y), Random.Range(-bounds.z, bounds.z));
            if (!Physics.CheckSphere(terrain.position + pos, radius, layer))
            {
                SetInjection();
                transform.localPosition = pos;
                return;
            }
        }
        ResetInjection();

        rotationDirection = Random.insideUnitCircle.normalized;
    }

    public void ResetInjection()
    {
        graphic.SetActive(false);
        capsuleCollider.enabled = false;
    }

    public void SetInjection()
    {
        graphic.SetActive(true);
        capsuleCollider.enabled = true;
    }
}
