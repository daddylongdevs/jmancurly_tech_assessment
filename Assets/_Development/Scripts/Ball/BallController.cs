using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Transform carryPosition;
    [SerializeField] private Transform aimPosition;
    [SerializeField] private float additiveGravity = 2f;

    [SerializeField] float lobBias = 0.2f;

    private Rigidbody rb;
    private Collider col;
    private Transform targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (transform.position.y <= -10f)
        {
            targetPosition = null;
            transform.position = Vector3.up;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            col.enabled = true;
            rb.isKinematic = false;
        }

        if (targetPosition == null)
            return;

        transform.position = targetPosition.position;
    }

    private void FixedUpdate()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * additiveGravity, ForceMode.Acceleration);
        }
    }

    public void ExecuteShot(Vector3 direction, float shotPower, Vector3 spinDirection)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        direction.y += lobBias;
        direction.Normalize();

        rb.isKinematic = false;

        rb.linearVelocity *= 0;
        rb.angularVelocity *= 0;

        rb.AddForce(direction * shotPower, ForceMode.Impulse);

        rb.AddTorque(spinDirection * shotPower * 0.5f, ForceMode.Impulse);
        targetPosition = null;

        StartCoroutine(ToggleColliderWithDelay(0.05f, true));
    }

    public void Aim()
    {
        targetPosition = aimPosition;
    }

    public void CancelAim()
    {
        targetPosition = carryPosition;
    }

    public void Pickup()
    {
        col.enabled = false;

        targetPosition = carryPosition;
        rb.isKinematic = true;
    }

    public void Drop()
    {
        col.enabled = true;
        rb.isKinematic = false;

        targetPosition = null;
    }

    IEnumerator ToggleColliderWithDelay(float delay, bool enabled)
    {
        yield return new WaitForSeconds(delay);

        col.enabled = enabled;
    }
}