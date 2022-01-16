using UnityEngine;

public class CellGrowth : MonoBehaviour
{
    [SerializeField] private GameObject _nucleusWall;
    [SerializeField] private GameObject _miniWall1;
    [SerializeField] private GameObject _miniWall2;
    
    private Vector3 _initialLocalScale;

    private void Awake() => _initialLocalScale = transform.localScale;

    [ContextMenu("Grow")]
    private void TestGrow() => Grow();

    public void Grow(float amount=0.1f) => transform.localScale *= (1f + amount);

    [ContextMenu("Reset")]
    public void ResetSize() => transform.localScale = _initialLocalScale;
    public void NucleusWallRemove() => _nucleusWall.SetActive(false);
    public void NucleusWallReplace() => _nucleusWall.SetActive(true);

    public void MiniWallsSetState(bool state)
    {
        _miniWall1.SetActive(state);
        _miniWall2.SetActive(state);
    }
}
