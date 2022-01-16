using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Cell_StateMachine : MonoBehaviour
{
    [SerializeField] private float _delayScale = 1f;
    [SerializeField] private GameObject _cellWall;
    
    // this was static, not sure why, switched to non-static
    public event Action<IState, Cell_StateMachine> OnCellStateChanged;
    public event Action<string> HelpTextUpdate;
    public event Action ReadyForNextPhase;
    
    private StateMachine _machine;
    private bool _resetStates;
    private bool _goToNextPhase;
    private CellGrowth _cellGrowth;
    private bool _weAreAwake;
    private bool _readyToPressSpace;

    public Type CurrentStateType => _machine?.CurrentState.GetType();
    public int Cycle { get; set; } = 1;
    public int IndexNumber { get; set; } = 0;

    private void Awake()
    {
        _machine = new StateMachine();
        _cellGrowth = GetComponent<CellGrowth>();
        _weAreAwake = false;
        _readyToPressSpace = true;
        
        // vital to use this test here, video 14d @ 04:30
        _machine.OnStateChanged += state => OnCellStateChanged?.Invoke(state, this);
        _machine.OnStateChanged += Handle_StateChanged;

        // STATES
        IState waking     = new Cell_Waking(this);
        IState waiting    = new Cell_Waiting(this);
        IState interphase = new Phase1_Interphase(this);
        IState prophase   = new Phase2_Prophase(this);
        IState metaphase  = new Phase3_Metaphase(this);
        IState anaphase   = new Phase4_Anaphase(this);
        IState telophase  = new Phase5_Telophase(this);
       
        // TRANSITIONS - anystate-to-state
        _machine.AddAnyTransition(interphase, () => _resetStates == true);

        // TRANSITIONS - state-to-state
        _machine.AddTransition(waking,     waiting,    () => _weAreAwake == true);
        _machine.AddTransition(waiting,    interphase, () => _goToNextPhase == true);
        _machine.AddTransition(interphase, prophase,   () => _goToNextPhase == true);
        _machine.AddTransition(prophase,   metaphase,  () => _goToNextPhase == true);
        _machine.AddTransition(metaphase,  anaphase,   () => _goToNextPhase == true);
        _machine.AddTransition(anaphase,   telophase,  () => _goToNextPhase == true);
        _machine.AddTransition(telophase,  waiting,    () => _goToNextPhase == true);
        
        // INITIAL STATE
        _machine.SetState(waking);
    }
    private IEnumerator Start()
    {
        // force wait one frame for clients to register
        yield return null;

        _weAreAwake = true;
        _machine.Tick();

        UITextListener.Instance.RegisterCellStateMachine(this);
    }

    private void Update()
    {
        _goToNextPhase = _readyToPressSpace && Input.GetKeyDown(KeyCode.Space);
        _machine.Tick();
    }
    
    public void SetHelpText(string help) => HelpTextUpdate?.Invoke(help);

    private void Handle_StateChanged(IState state)
    {
        // was here to test CancelInvoke (not a good idea, cancels everything)
    }
    public void CellGrow(float delay=0) => Invoke(nameof(GrowDelayed), delay * _delayScale);
    private void GrowDelayed() => _cellGrowth.Grow();

    public void CellReset()
    {
        _cellGrowth.ResetSize();
        RemoveAllDNA();
        NucleusWallReplace();
        ChromosomesOff();
    }

    public void HoldNextPhase() => _readyToPressSpace = false;
    public void ClearForNextPhase(float delay)
    {
        Invoke(nameof(ClearForNextPhaseAfterDelay), delay * _delayScale);
    }

    private void ClearForNextPhaseAfterDelay()
    {
        _readyToPressSpace = true;
        ReadyForNextPhase?.Invoke();
    }

    // -------------- NUCLEUS ------------------

    public void NucleusWallRemove(float delay=0) => Invoke(nameof(NucleusRemoveDelayed), delay * _delayScale);
    private void NucleusRemoveDelayed() => _cellGrowth.NucleusWallRemove();
    public void NucleusWallReplace(float delay=0) => _cellGrowth.NucleusWallReplace();

    // ---------- METAPHASE_PLATE ----------------
    
    public void MetaphasePlateActivate(float delay)
    {
        Invoke(nameof(MetaActivateDelayed), delay * _delayScale);
    }

    private void MetaActivateDelayed()
    {
        var plate = GetComponentInChildren<MetaphasePlate>();
        plate.Activate();
    }

    public void MetaphasePlateDeactivate()
    {
        var plate = GetComponentInChildren<MetaphasePlate>();
        plate.Deactivate();
    }
    
    // ------------- NEW CELLS -------------------

    public void NewCellWallsForm(float delay=0) => Invoke(nameof(NewCellWallsFormDelayed), delay * _delayScale);
    public void NewCellWallsFormDelayed() => _cellGrowth.MiniWallsSetState(true);

    public void NewCellWallsHide(float delay=0) => Invoke(nameof(NewCellWallsHideDelayed), delay * _delayScale);
    public void NewCellWallsHideDelayed() => _cellGrowth.MiniWallsSetState(false);

    public void CellDivides(float delay=0) => Invoke(nameof(CellsDivideDelayed), delay * _delayScale);

    public void OriginalCellWallDisappears(float delay)
    {
        Invoke(nameof(OriginalCellWallDisappearsDelayed), delay * _delayScale);
    }

    private void OriginalCellWallDisappearsDelayed()
    {
        _cellWall.SetActive(false);
    }

    public void OriginalCellWallConstricts(float delay)
    {
        Invoke(nameof(OriginalCellWallConstrictsDelayed), delay * _delayScale);
    }

    private void OriginalCellWallConstrictsDelayed()
    {
        var obj = GetComponentInChildren<Constrict>();
        obj.StartAnimation();
    }

    private void CellsDivideDelayed()
    {
        // keeping a reference to the cell prefab in the SceneData singleton
        GameObject _cellPrefab = SceneData.Instance.CellPrefab;
        
        // spawn new cells to the left & right of parent
        var pos = transform.position + transform.right;
        var newCell = Instantiate(_cellPrefab, pos, Quaternion.identity);
        newCell.transform.Rotate(Vector3.forward, 90f, Space.Self);
        newCell.GetComponent<Cell_StateMachine>().Cycle += 1;
        newCell.GetComponent<Cell_StateMachine>().IndexNumber = 1;

        pos = transform.position - transform.right;
        newCell = Instantiate(_cellPrefab, pos, Quaternion.identity);
        newCell.transform.Rotate(Vector3.forward, 90f, Space.Self);
        newCell.GetComponent<Cell_StateMachine>().Cycle += 1;
        newCell.GetComponent<Cell_StateMachine>().IndexNumber = 2;

        // we have replaced this cell with two new ones; we can destroy it.
        ContractileLineDeactivate();
        MetaphasePlateDeactivate();
        ChromosomesOff();
        RemoveAllDNA();
        _cellGrowth.MiniWallsSetState(false);
        
        Destroy(this.gameObject);
    }
    
    // ---------- CONTRACTILE_LINE ----------------
    
    public void ContractileLineActivate(float delay=0)
    {
        Invoke(nameof(ContractileLineActivateDelayed), delay * _delayScale);
    }

    private void ContractileLineActivateDelayed()
    {
        var line = GetComponentInChildren<ContractileLine>(true);
        line.Activate();
    }

    public void ContractileLineDeactivate(float delay=0)
    {
        Invoke(nameof(ContractileLineDeactivateDelayed), delay * _delayScale);
    }
    
    private void ContractileLineDeactivateDelayed()
    {
        var line = GetComponentInChildren<ContractileLine>(true);
        line.Deactivate();
    }
    
    // =========================== DNA ====================================
    
    public void CopyBigDNA(float delay=0) => Invoke(nameof(CopyBigDNADelayed), delay * _delayScale);
    public void CopySmallDNA(float delay=0) => Invoke(nameof(CopySmallDNADelayed), delay * _delayScale);

    private void CopyBigDNADelayed() => CopyDNAInLayer("BigDNA");
    private void CopySmallDNADelayed() => CopyDNAInLayer("SmallDNA");
    private void CopyDNAInLayer(string layerName)
    {
        var dnas = GetComponentsInChildren<DNAMaker>();
        foreach (var dna in dnas)
        {
            if (dna.gameObject.layer == LayerMask.NameToLayer(layerName))
            {
                dna.MakeDNA();
            }
        }
    }

    public void RemoveAllDNA()
    {
        var dnas = GetComponentsInChildren<DNAMaker>();
        foreach (var dna in dnas) { dna.RemoveDNA(); }
    }
    
    // ======================== CHROMOSOMES ===============================
    
    public void ChromosomesActivate(float delay=0) => Invoke(nameof(ChromosomesOnDelayed), delay * _delayScale);

    private void ChromosomesOnDelayed()
    {
        RemoveAllDNA();
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.Activate();
    }

    public void ChromosomesOff()
    {
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.Deactivate();
    }
    
    // --------------- delete for cycle II ---------------------------
    
    public void DeleteOneChromosomeOfEachSize()
    {
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.DeleteOneOfEachSize();
    }

    // --------------- pair / unpair ----------------------------------

    public void ChromosomesPairUp(float delay=0) => Invoke(nameof(ChromosomesPairUpDelayed), delay * _delayScale);

    private void ChromosomesPairUpDelayed()
    {
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.TeamUp();
    }
    
    public void ChromosomesUnPair(float delay=0) => Invoke(nameof(ChromosomesUnPairDelayed), delay * _delayScale);

    public void ChromosomesUnPairDelayed()
    {
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.ResetToSiblings();
    }
    
    // ----------------- fly! -----------------------------------------
    
    public void ChromosomesFlyToPoles(float delay) => Invoke(nameof(ChromosomesFlyToPolesDelayed), delay * _delayScale);

    public void ChromosomesFlyToPolesDelayed()
    {
        var chromosomes = GetComponentsInChildren<Chromosome>(true);
        foreach (var chromosome in chromosomes)
        {
            var lineFollower = chromosome.GetComponent<LineFollower>();
            lineFollower.FollowLine();
        }
    }
    public void ChromosomesToPlate(float delay) => Invoke(nameof(ChromosomesToPlateDelayed), delay * _delayScale);

    private void ChromosomesToPlateDelayed()
    {
        var chromosomes = GetComponentInChildren<Chromosomes>(true);
        chromosomes.MoveToPlate();
    }

    // ======================== CENTROSOMES ============================
    
    public void CentrosomesToPoles(float delay)
    {
        Invoke(nameof(CentrosomesToPolesDelayed), delay * _delayScale);
    }

    private void CentrosomesToPolesDelayed()
    {
        var cents = GetComponentsInChildren<Centrosome>();
        foreach (var cent in cents) { cent.MoveToEndPoint(); }
    }

    public void CentrosomesReset(float delay)
    {
        var cents = GetComponentsInChildren<Centrosome>();
        foreach (var cent in cents) { cent.BackToStart(); }
    }


    public void CentrosomesAttachToCentromeres(float delay)
    {
        Invoke(nameof(CentrosomesAttachToCentromeresDelayed), delay * _delayScale);
    }
    
    public void CentrosomesAttachToCentromeresDelayed()
    {
        var cents = GetComponentsInChildren<Centrosome>();
        foreach (var cent in cents) { cent.AttachSpindlesToCentromeres(); }
    }

    // ------------- attach spindles to chromosomes -----------------------
    public void CentrosomesAttachToChromosomes(float delay)
    {
        Invoke(nameof(CentrosomesAttachToChromosomesDelayed), delay * _delayScale);
    }
    
    public void CentrosomesAttachToChromosomesDelayed()
    {
        var cents = GetComponentsInChildren<Centrosome>();
        foreach (var cent in cents) { cent.AttachSpindlesToChromosomes(); }
    }

    // --------------------- CENTROMERES ----------------------------
    
    public void CentromeresFlyToPoles(float delay)
    {
        Invoke(nameof(CentromeresFlyToPolesDelayed), delay * _delayScale);
    }

    private void CentromeresFlyToPolesDelayed()
    {
        var cents = GetComponentsInChildren<Centromere>();
        foreach (var cent in cents)
        {
            var lineFollower = cent.GetComponent<LineFollower>();
            lineFollower.FollowLine();
        }
    }

    // ========================================================================
   
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        string _stateText = "unknown";
        string _statecolour = "white";
        
        if (CurrentStateType != null)
        {
            // if      (CurrentStateType == typeof(Idle))      { _statecolour = "white"; }
            // else if (CurrentStateType == typeof(Scared))    { _statecolour = "yellow"; }

            _stateText = CurrentStateType.Name.ToLower();
        }
        
        _statecolour = "white"; // prefer all white for now
        
        GUIStyle customStyle = new GUIStyle();
        customStyle.fontSize = 16;   // can also use e.g. <size=30> in Rich Text
        customStyle.richText = true;
        string richText = "<color="+_statecolour+"><B>[" + _stateText + "]</B></color>";
        
        Vector3 textPosition = transform.position + (Vector3.up * 0.3f);
        Handles.Label(textPosition, richText, customStyle);
    }
#endif
}
