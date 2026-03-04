using UnityEngine;
using UnityEngine.InputSystem;

public class CannonMovement : MonoBehaviour
{
    [SerializeField] private float _locomotionSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 100f;

    private readonly LayerMask _groundLayer = -1;

    private Vector3 _currentNormal = Vector3.up;
    private Vector3 _movementInput;

    private readonly float _surfaceOffset = 0f;
    private readonly float _raycastHeight = 2f;
    private readonly float _raycastDistance = 10f;
    private readonly float _rotationSmoothness = 10f;
    private float _rotationInput;

    private readonly bool _rotateToSurfaceNormal = false;

    private void Update()
    {
        UpdateSurfaceNormalDuringFrame();
        ReadInputDuringFrame();
        LocomoteDuringFrame();
        AdjustToSurfaceDuringFrame();

        if (_rotateToSurfaceNormal == true)
        {
            AlignToSurfaceDuringFrame();
        }
    }

    private void UpdateSurfaceNormalDuringFrame()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * _raycastHeight, -Vector3.up, out hit, _raycastDistance, _groundLayer)) _currentNormal = hit.normal;
        else _currentNormal = Vector3.up;
    }

    private void ReadInputDuringFrame() //вместо этого - отдельный класс считывания И передача данных через Event Bus или что-то подобное
    {
        _movementInput = Vector3.zero;
        _rotationInput = 0f;

        if (Keyboard.current.wKey.isPressed) _movementInput += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) _movementInput += Vector3.back;
        if (Keyboard.current.aKey.isPressed) _movementInput += Vector3.left;
        if (Keyboard.current.dKey.isPressed) _movementInput += Vector3.right;

        if (Keyboard.current.qKey.isPressed) _rotationInput -= 1f;
        if (Keyboard.current.eKey.isPressed) _rotationInput += 1f;
    }

    private void LocomoteDuringFrame()
    {
        // Вращение
        if (_rotationInput != 0f)
        {
            float rotationAmount = _rotationInput * _rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotationAmount, 0f, Space.Self);
        }

        // Движение
        if (_movementInput != Vector3.zero)
        {
            // Получаем векторы направления с учетом нормали поверхности
            Vector3 forward = Vector3.ProjectOnPlane(transform.forward, _currentNormal).normalized;
            Vector3 right = Vector3.ProjectOnPlane(transform.right, _currentNormal).normalized;

            // Вычисляем направление движения
            Vector3 moveDirection = Vector3.zero;

            if (Keyboard.current.wKey.isPressed) moveDirection += forward;
            if (Keyboard.current.sKey.isPressed) moveDirection -= forward;
            if (Keyboard.current.aKey.isPressed) moveDirection -= right;
            if (Keyboard.current.dKey.isPressed) moveDirection += right;

            // Применяем движение
            if (moveDirection != Vector3.zero) transform.position += moveDirection.normalized * _locomotionSpeed * Time.deltaTime;
        }
    }

    private void AdjustToSurfaceDuringFrame()
    {
        // Корректируем позицию относительно поверхности
        RaycastHit hit;

        // Устанавливаем позицию на поверхности с учетом offset
        if (Physics.Raycast(transform.position + Vector3.up * _raycastHeight, -Vector3.up, out hit, _raycastDistance, _groundLayer)) transform.position = hit.point + _currentNormal * _surfaceOffset;
    }

    private void AlignToSurfaceDuringFrame()
    {
        // Плавное выравнивание объекта по нормали поверхности
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, _currentNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSmoothness * Time.deltaTime);
    }
}
