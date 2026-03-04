using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CannonProjectile : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private readonly Vector3 _launchVelocity = CannonAttackTrajectoryRenderer.LaunchVelocity; //статика

    private readonly float _mass = CannonAttackTrajectoryRenderer.Mass; //статика
    private readonly float _lifeTime = 10f;

    public float LifeTime => _lifeTime;

    private void Awake()
    {
        InitRigidbody();
        ApplyForce();
    }

    private void ApplyForce()
    {
        _rigidbody.mass = _mass;
        transform.localScale = new Vector3(_rigidbody.mass / 2, _rigidbody.mass / 2, _rigidbody.mass / 2); //не оптимизированно

        var force = _launchVelocity * _rigidbody.mass;

        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void InitRigidbody()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.linearDamping = 0f; //МГ
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous; //хз - зачем (было написано - для быстрых объектов)
    }
}
