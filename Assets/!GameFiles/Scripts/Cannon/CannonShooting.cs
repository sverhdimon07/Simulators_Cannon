using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CannonShooting : MonoBehaviour //вместо данной реализации - Object Pool
{
    public static UnityAction Shot;

    [SerializeField] private CannonProjectile _projectilePrefab;

    [SerializeField] private Transform _launchPoint;

    private void Update()
    {
        ReadInputDuringFrame(); //вместо этого - отдельный класс считывания И передача данных через Event Bus или что-то подобное
    }

    private void ReadInputDuringFrame()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) Shoot();
    }

    private void Shoot()
    {
        CannonProjectile projectile = InstantiateProjectile();

        StartCoroutine(DestroyProjectile(projectile, projectile.LifeTime)); //хз - должен ли этот класс изменять время жизни проджектайла

        Shot.Invoke();
    }

    private CannonProjectile InstantiateProjectile()
    {
        if (_projectilePrefab == null) throw new NullReferenceException();
        CannonProjectile projectile = Instantiate(_projectilePrefab, _launchPoint.position, Quaternion.identity);

        return projectile;
    }

    private IEnumerator DestroyProjectile(CannonProjectile projectile, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(projectile.gameObject);
    }
}
