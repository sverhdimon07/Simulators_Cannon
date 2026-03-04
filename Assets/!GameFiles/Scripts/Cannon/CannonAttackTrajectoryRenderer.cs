using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class CannonAttackTrajectoryRenderer: MonoBehaviour
{
    public static Vector3 LaunchVelocity { get; private set; } //вместо статики - внедрение зависимостей

    public static float Mass; //вместо статики - внедрение зависимостей

    [SerializeField] private Transform _startPoint;

    private LineRenderer _lineRenderer;

    private readonly int _pointCount = 40; //пересмотреть отношеине к readonly, я думаю, что его не нужно пихать прям везде, где это можно; это контракт на изменяемость поля, то есть нужно ясно понять, должно оно изменяться или нет

    private readonly float _massMin = 3f;
    private readonly float _massMax = 8f;
    private readonly float _launchForce = 70f;
    private readonly float _timeStep = 0.1f;
    private readonly float _lineWidth = 0.02f;
    private readonly float _mouseWheelSensitivity = 1f;
    private float _mass; //масса и возможно сила запуска не должны быть детерминированы ни этим классом и ни классом самого проджектайла, они должны быть определены классом более высокого уровня (каким-то гейм-менеджером)
    private float _angle; //как инкапсулировать (в такой однобокой и слабовзаимодействующей между собой системе - рано говорить об инкапсуляции (инкапсуляция - это защита инварианта от использования интерфейса взаимодействия И защита логики твоего скрипта от тебя самого, то есть, предотвращение ошибок внутри твоего кода (2е - ХЗ)))

    private Vector3 StartPointPosition => _startPoint.position; //имеет ли это смысл

    private void Awake()
    {
        CalculateMass();
        InitLineRenderer();
        transform.SetParent(null);
    }

    private void OnEnable()
    {
        CannonShooting.Shot += CalculateMass;
    }

    private void OnDisable()
    {
        CannonShooting.Shot -= CalculateMass;
    }

    private void Update()
    {
        ReadInputDuringFrame();
        RenderDuringFrame();
    }

    private void RenderDuringFrame()
    {
        _lineRenderer.positionCount = _pointCount;

        var startPoint = StartPointPosition;
        LaunchVelocity = CalculateLaunchVelocity();

        for (int i = 0; i < _pointCount; i++)
        {
            var t = i * _timeStep;
            var point = startPoint + LaunchVelocity * t;

            point.y += 0.5f * Physics.gravity.y * t * t; //МГ, как избежать - хз, видимо такие МГ надо оставлять

            _lineRenderer.SetPosition(i, point);
        }
    }

    private Vector3 CalculateLaunchVelocity()
    {
        float effectiveVelocity = _launchForce / _mass;

        var vx = effectiveVelocity * Mathf.Cos(_angle * Mathf.Deg2Rad);
        var vy = effectiveVelocity * Mathf.Sin(_angle * Mathf.Deg2Rad);

        return _startPoint.forward * vx + _startPoint.up * vy;
    }

    private void CalculateMass()
    {
        _mass = Random.Range(_massMin, _massMax);
        Mass = _mass;
    }

    private void InitLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void ReadInputDuringFrame()
    {
        float scrollValue = Mouse.current.scroll.y.ReadValue();

        if (scrollValue == 0)
        {
            return;
        }
        _angle += scrollValue * _mouseWheelSensitivity;
        _angle = Mathf.Clamp(_angle, 0, 80);
    }
}
