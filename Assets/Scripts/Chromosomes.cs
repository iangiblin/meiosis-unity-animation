using UnityEngine;

public class Chromosomes : MonoBehaviour
{
    [SerializeField] private GameObject _small1;
    [SerializeField] private GameObject _small2;
    [SerializeField] private GameObject _large1;
    [SerializeField] private GameObject _large2;

    [SerializeField] private Vector3 _offset = Vector3.one;
    
    private Transform _small1_initial;
    private Transform _small2_initial;
    private Transform _large1_initial;
    private Transform _large2_initial;

    private void Start()
    {
        // pairing is done by attaching #1 to #2
        _small1_initial = _small1.transform;
        _small2_initial = _small2.transform;
        _large1_initial = _large1.transform;
        _large2_initial = _large2.transform;
    }

    // ===================== ON/OFF ================================
    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        var chromosomes = transform.GetComponentsInChildren<Transform>(true);
        foreach (var tx in chromosomes) { tx.gameObject.SetActive(false); }
    }
    
    [ContextMenu("Activate")]
    public void Activate()
    {
        var chromosomes = transform.GetComponentsInChildren<Transform>(true);
        foreach (var tx in chromosomes) { tx.gameObject.SetActive(true); }
    }

    // ===================== PAIR/UNPAIR ==========================
    
    [ContextMenu("Team Up (homologous pairs)")]
    public void TeamUp()
    {
        // set #1 to be a child of #2 for both large & small
        
        _small1.transform.localPosition = _small2.transform.localPosition + _offset;
        _large1.transform.localPosition = _large2.transform.localPosition + _offset;
        
        _small1.transform.parent = _small2.transform;
        _large1.transform.parent = _large2.transform;
    }

    [ContextMenu("Reset the chromosomes to be siblings, unpaired")]
    public void ResetToSiblings()
    {
        // reset the parents of all of them to be this object

        if (_small1 != null) { _small1.transform.parent = transform; }
        if (_small2 != null) { _small2.transform.parent = transform; }
        if (_large1 != null) { _large1.transform.parent = transform; }
        if (_large2 != null) { _large2.transform.parent = transform; }
    }
    
    
    [ContextMenu("Reset (return to original positions & rotations)")]
    public void ChromoReset()
    {
        // centromeres are used to pull the chromosomes apart
        var centromeres = GetComponentsInChildren<Centromere>();
        foreach (var centromere in centromeres)
        {
            var lineFollower = centromere.GetComponent<LineFollower>();
            lineFollower.ResetCompletely();
        }
        
        ResetToSiblings();
        
        _small1.transform.localPosition = _small1_initial.localPosition;
        _small2.transform.localPosition = _small2_initial.localPosition;
        _large1.transform.localPosition = _large1_initial.localPosition;
        _large2.transform.localPosition = _large2_initial.localPosition;
        
        _small1.transform.localRotation = _small1_initial.localRotation;
        _small2.transform.localRotation = _small2_initial.localRotation;
        _large1.transform.localRotation = _large1_initial.localRotation;
        _large2.transform.localRotation = _large2_initial.localRotation;

    }

    // ========================= TO/FROM PLATE ======================

    [ContextMenu("Move to Metaphase Plate")]
    public void MoveToPlate()
    {
        if (_small2 != null)
        {
            // we have made #1 a child of #2 so we can move #2
            var pos = _small2.transform.localPosition;
            _small2.transform.localPosition = new Vector3(pos.x, 0, pos.z);
            _small2.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
        else
        {
            var pos = _small1.transform.localPosition;
            _small1.transform.localPosition = new Vector3(pos.x, 0, pos.z);
            _small1.transform.localEulerAngles = new Vector3(90, 0, 0);
        }

        if (_large2 != null)
        {
            // same for large
            var pos = _large2.transform.localPosition;
            _large2.transform.localPosition = new Vector3(pos.x, 0, pos.z);
            _large2.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
        else
        {
            var pos = _large1.transform.localPosition;
            _large1.transform.localPosition = new Vector3(pos.x, 0, pos.z);
            _large1.transform.localEulerAngles = new Vector3(90, 0, 0);
        }
    }

    public void DeleteOneOfEachSize()
    {
        var cell = transform.parent.GetComponent<Cell_StateMachine>();
        switch (cell.IndexNumber)
        {
            case 1:
                Destroy(_small2);
                Destroy(_large2);
                break;
            case 2:
                Destroy(_small1);
                Destroy(_large1);
                break;
            default:
                Debug.LogError("Bad call to DeleteOneOfEachSize!");
                break;
        }
    }
}