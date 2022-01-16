using UnityEngine;

public class Constrict : MonoBehaviour
{
    // I originally learned some of this from here (but the tutorial is overkill)
    // https://www.raywenderlich.com/3169311-runtime-mesh-manipulation-with-unity
    
    [SerializeField] private float _duration = 4;

    Mesh originalMesh;
    MeshFilter meshFilter;

    private Vector3[] _originalVertices;
    private Vector3[] _originalNormals;
    private Vector3[] _finalVertices;
    private bool _animating;
    private float _animationTime;

    void Start()
    {
        meshFilter        = GetComponent<MeshFilter>();
        originalMesh      = meshFilter.mesh;
        _originalNormals  = originalMesh.normals;
        _originalVertices = originalMesh.vertices;

        _animating = false;
    }

    // ------------------------------------------------------------------------

    [ContextMenu("Instantly distort Mesh")]
    private void DistortMeshInstantly()
    {
        originalMesh.vertices = TargetVertices(meshFilter.mesh.vertices);
        originalMesh.RecalculateNormals();
    }
    
    // ------------------------------------------------------------------------

    private void Update()
    {
        if (_animating)
        {
            DoAnimation();
        }
    }

    private void DoAnimation()
    {
        var progress = _animationTime / _duration;

        Vector3[] vA = _originalVertices;
        Vector3[] vB = _finalVertices;
        Vector3[] vV = new Vector3[_originalVertices.Length];

        for (int i = 0; i < _originalVertices.Length; i++)
            vV[i] = Vector3.Lerp(vA[i], vB[i], progress);

        originalMesh.vertices = vV;
        originalMesh.RecalculateNormals();

        _animationTime += Time.deltaTime;
        _animating = (_animationTime < _duration);
    }

    [ContextMenu("Animate Mesh distortion")]
    public void StartAnimation()
    {
        _finalVertices = TargetVertices(meshFilter.mesh.vertices);
        _animationTime = 0;
        _animating = true;
    }

    // ------------------------------------------------------------------------
    private Vector3[] TargetVertices(Vector3[] inputVertices)
    {
        // test function, just constricts the middle of a sphere
        
        var newVertices = new Vector3[inputVertices.Length];
        
        for (int i = 0; i < inputVertices.Length; i++)
        {
            Vector3 pos = inputVertices[i];
            pos.x *= Mathf.Sqrt(Mathf.Abs(pos.y));
            pos.z *= Mathf.Sqrt(Mathf.Abs(pos.y));
            newVertices[i] = pos;
        }

        return newVertices;
    }

    // ------------------------------------------------------------------------
    
    [ContextMenu("Reset Mesh")]
    private void ResetMesh()
    {
        originalMesh          = meshFilter.mesh;
        originalMesh.vertices = _originalVertices;
        originalMesh.normals  = _originalNormals;
    }
    
    // ------------------------------------------------------------------------
}
