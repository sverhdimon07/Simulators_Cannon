using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CannonTarget : MonoBehaviour
{
    public static UnityAction TargetHit;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        InitRigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<CannonProjectile>() == false) return;
        TargetHit.Invoke();
    }

    private void InitRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = 1f; //МГ
        _rigidbody.linearDamping = 0f; //МГ
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = true;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous; //хз - зачем (было написано - для быстрых объектов)
    }
}
