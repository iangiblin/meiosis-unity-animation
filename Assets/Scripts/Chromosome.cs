using UnityEngine;

public class Chromosome : MonoBehaviour
{
    [SerializeField] private GameObject _centromere1;
    [SerializeField] private GameObject _centromere2;
    
    public GameObject ClosestCentromereToPoint(Vector3 pos)
    {
        var d1 = Vector3.Distance(pos, _centromere1.transform.position);
        var d2 = Vector3.Distance(pos, _centromere2.transform.position);
        return (d1 < d2) ? _centromere1 : _centromere2;
    }
}
