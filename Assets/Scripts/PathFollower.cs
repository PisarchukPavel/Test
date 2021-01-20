using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] 
    private PointVariable _lastClickedPoint;

    [SerializeField] 
    private RectTransform _source;

    [SerializeField] 
    private float _speed;
    
    [SerializeField] 
    private float _accelerationSpeed;

    [SerializeField] 
    private float _breakSpeed;
    
    [SerializeField] 
    private float _stoppingDist;

    private int _currentPoint = 0;
    private float _acceleration = 0.0f;
    
    private Vector3 _virtualPosition;
    private Vector3 _target;
    private Vector3 _nextTarget;
    
    private List<Vector3> _path = new List<Vector3>();
    
    private void Awake()
    {
        _lastClickedPoint.OnChanged += OnNewPointAdd;
        _virtualPosition = _source.position;
    }

    private void Update () 
    {
        Movement();
    }
    
    private void Movement() 
    {
        if(_path.Count > 0 && _path.Count != _currentPoint)
        {
            UpdateTargets();
            
            _virtualPosition = Vector3.MoveTowards(_virtualPosition, _target, _speed * Time.deltaTime * _acceleration);
            _source.position = _virtualPosition;
           
            CheckToNextTarget();
            CalculateAcceleration();
        }
    }

    private void UpdateTargets()
    {
        _target = _path[_currentPoint];
        if (_path.Count - 1 > _currentPoint)
        {
            _nextTarget = _path[_currentPoint + 1];
        }
    }
    
    private void CheckToNextTarget()
    {
        float distance = DistanceTo(_virtualPosition, _target);
        if(distance <= 0.0f)
        {
            _currentPoint++;
        }
    }
    
    private void CalculateAcceleration()
    {
        float totalDistanceToFinish = 0.0f;
        Vector3 pos = _virtualPosition;
        for (int i = _currentPoint; i < _path.Count; i++)
        {
            float distance = DistanceTo(pos, _path[i]);
            pos = _path[i];
            totalDistanceToFinish += distance;
        }
        
        if (totalDistanceToFinish < _stoppingDist)
        {
            _acceleration = totalDistanceToFinish / _stoppingDist / _breakSpeed;
        }
        else
        {
            _acceleration += _accelerationSpeed * Time.deltaTime;
        }
        
        _acceleration = Mathf.Clamp01(_acceleration);
    }

    private float DistanceTo(Vector2 position, Vector2 targetPosition)
    {
        Vector2 difference = new Vector2(position.x - targetPosition.x, position.y - targetPosition.y);
        float distance = Mathf.Sqrt(Mathf.Pow(difference.x, 2f) + Mathf.Pow(difference.y, 2f));
        
        return Mathf.Floor(distance);
    }
    
    private void OnNewPointAdd(Vector3 point)
    {
        _path.Add(point);
    }
}