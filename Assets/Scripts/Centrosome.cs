using UnityEngine;
using UnityEngine.Serialization;

public class Centrosome : MonoBehaviour
{
    [SerializeField] private GameObject _chromosomeParent;
    [SerializeField] private Transform _polePoint;
    [SerializeField] private GameObject _sphere;

    private Vector3 _startPoint;

    // Start is called before the first frame update
    void Start()
    {
        _startPoint = _sphere.transform.localPosition;
    }

    [ContextMenu("To End")]
    public void MoveToEndPoint()
    {
        _sphere.transform.localPosition = _polePoint.localPosition;
    }

    [ContextMenu("To Start")]
    public void BackToStart()
    {
        _sphere.transform.localPosition = _startPoint;
    }

    [ContextMenu("Attach Spindles To Centromeres")]
    public void AttachSpindlesToCentromeres()
    {
        // find all chromosomes in this cell by going via the parent
        var chromosomes = _chromosomeParent.GetComponentsInChildren<Chromosome>();

        // for each one, find closest sphere and attach to it
        foreach (var chromosome in chromosomes)
        {
            var target = chromosome.ClosestCentromereToPoint(transform.position);
            AttachSpindleToThat(target);
        }
    }

    
    [ContextMenu("Attach Spindles To Chromosomes (match last letter)")]
    public void AttachSpindlesToChromosomes()
    {
        char myLastLetter = name[name.Length - 1];
        
        // find all chromosomes in this cell by going via the parent
        var chromosomes = _chromosomeParent.GetComponentsInChildren<Chromosome>();

        // for each one, attach to it IF IT MATCHES TAG - don't attach to all
        foreach (var chromosome in chromosomes)
        {
            GameObject that = chromosome.gameObject;
            char lastLetter = that.name[that.name.Length - 1];
            if (lastLetter == myLastLetter) { AttachSpindleToThat(that); }
        }
    }

    private bool AttachSpindleToThat(GameObject target)
    {
        var line = target.GetComponentInParent<LineRenderer>();
        if (line == null) { return false; }
        
        line.enabled = true;
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
            
        var points = new Vector3[2];
        points[0] = _sphere.transform.position;
        points[1] = target.transform.position;
        line.SetPositions(points);
        return true;
    }
}
