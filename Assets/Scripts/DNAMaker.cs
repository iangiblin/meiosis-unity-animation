using UnityEngine;
using Random = UnityEngine.Random;

public class DNAMaker : MonoBehaviour
{
    [SerializeField] [Range(0.1f,1f)] private float _sphereDiameter = 0.5f;
    [SerializeField] [Range(0.001f,0.1f)] private float _linewidth;
    [SerializeField] private int _numberOfPoints = 50;
    
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    private LineRenderer _line;
    private int _currentPointCount = 0;

    private void AddLineComponent()
    {
        _line = gameObject.AddComponent<LineRenderer>();
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.widthMultiplier = _linewidth;
        
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f),
                new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f),
                new GradientAlphaKey(alpha, 1.0f) }
        );
        _line.colorGradient = gradient;
        _line.enabled = false;
    }

    // --------------------------------------------------------------
    [ContextMenu("MakeDNA")]
    public void MakeDNA()
    {
        if (_line == null) { AddLineComponent(); }
        _line.enabled = true;
        
        // ------------- populate/grow the line with some DNA -------------
        
        var pos = transform.position;
        _currentPointCount += _numberOfPoints;

        // create array of 3D points
        var points = new Vector3[_currentPointCount];
        for (int i = 0; i < _currentPointCount; i++)
        {
            points[i] = New3DPointInSphere(pos, _sphereDiameter);
        }
        _line.positionCount = _currentPointCount;
        _line.SetPositions(points);
    }

    // --------------------------------------------------------------
    private Vector3 New3DPointInSphere(Vector3 center, float diameter)
    {
        float radius = diameter / 2;
        float x = Random.Range(-radius, radius);
        float y = Random.Range(-radius, radius);
        float z = Random.Range(-radius, radius);
        return center + new Vector3(x, y, z);
    }

    public void RemoveDNA()
    {
        if (_line == null) { return; }
        _line.enabled = false;
        _currentPointCount = 0;
    }
}
