using UnityEngine;

public class LineFollower : MonoBehaviour
{
    [SerializeField] [Range(0.1f,1f)] private float _lineFractionToTravel = 0.75f;
    
    private LineRenderer _line;
    private bool _followingLine;
    private Vector3 _initialPosition;
    private Vector3 _finalPosition;
    private Vector3 _initialPositionWorldSpace;
    
    private float _flightTime;
    private float _flightTimeSoFar;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_followingLine)
        {
            _flightTimeSoFar += Time.deltaTime;
            float fraction = _flightTimeSoFar / _flightTime;
            var pos = Vector3.Lerp(_initialPositionWorldSpace, _finalPosition, fraction);
            transform.position = pos;
            _line.SetPosition(1, pos);
            if (fraction > _lineFractionToTravel) { _followingLine = false; }
        }
    }

    [ContextMenu("Reset Position")]
    public void ResetCompletely()
    {
        transform.localPosition = _initialPosition;
        _line.enabled = false;
    }

    [ContextMenu("Follow Line")]
    private void FollowLineDebug()
    {
        FollowLine();
    }
    
    public void FollowLine(float flightTime = 3f)
    {
        if (_line == null) { return; }
        
        _initialPositionWorldSpace = transform.position;
        _finalPosition = _line.GetPosition(0);
        _flightTime = flightTime;
        _followingLine = true;
        _flightTimeSoFar = 0;
    }
}
