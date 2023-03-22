using UnityEngine;

public class CustomBillboard : MonoBehaviour
{
    Transform playerTransform;

    private void Start() => playerTransform = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Transform>();

    private void LateUpdate() => transform.LookAt(playerTransform);
}
