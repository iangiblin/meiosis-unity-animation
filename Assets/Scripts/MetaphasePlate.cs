using UnityEngine;

public class MetaphasePlate : MonoBehaviour
{
    [SerializeField] private GameObject _plate;
    
    void Start() => Deactivate();

    [ContextMenu("Activate Plate")]
    public void Activate() => _plate.SetActive(true);

    [ContextMenu("Deactivate Plate")]
    public void Deactivate() => _plate.SetActive(false);
}