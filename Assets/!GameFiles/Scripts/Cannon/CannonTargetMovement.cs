using UnityEngine;

public class CannonTargetMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _destinationPoints;

    [Range(0.05f, 0.1f)]
    [SerializeField] private float _locomotionSpeed = 0.05f;

    private Vector3[] _destinationPointPositions; //ссылка на массив не может меняться, сам массив может (про readonly)

    private int _currentDestinationPointPositionIndex;

    private readonly float _minPossibleDistanceBetweenDestinationPointPositions = 0.5f; //хз - норм ли настолько длинные названия (НО они семантически исчерпывающие)

    private void Awake()
    {
        InitAndFullDestinationPointPositions();
    }

    private void Update()
    {
        LocomoteDuringFrame();
    }

    private void LocomoteDuringFrame()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _destinationPointPositions[_currentDestinationPointPositionIndex], Vector3.Distance(transform.position, _destinationPointPositions[_currentDestinationPointPositionIndex]) * 2 * Time.deltaTime); //МГ

        Vector3 targetPosition = _destinationPointPositions[_currentDestinationPointPositionIndex];
        Vector3 direction = targetPosition - transform.position;

        float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z);
        Vector3 normalizedDirection = new Vector3(direction.x / distance, direction.y / distance, direction.z / distance);

        transform.position = new Vector3(transform.position.x + normalizedDirection.x * _locomotionSpeed, transform.position.y + normalizedDirection.y * _locomotionSpeed, transform.position.z + normalizedDirection.z * _locomotionSpeed);

        if (Vector3.Distance(transform.position, _destinationPointPositions[_currentDestinationPointPositionIndex]) <= _minPossibleDistanceBetweenDestinationPointPositions) _currentDestinationPointPositionIndex += 1;
        if (_currentDestinationPointPositionIndex >= _destinationPoints.Length) _currentDestinationPointPositionIndex = 0; //использую больше знаков, чем нужно
    }

    private void InitAndFullDestinationPointPositions()
    {
        _destinationPointPositions = new Vector3[_destinationPoints.Length];

        for (int i = 0; i < _destinationPoints.Length; i++)
        {
            _destinationPointPositions[i] = _destinationPoints[i].position;
        }
    }
}
