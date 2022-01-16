using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    public static SceneData Instance { get; set; }

    public GameObject CellPrefab => _cellPrefab;
    
    private void Awake()
    {
        // cheap Singleton
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
