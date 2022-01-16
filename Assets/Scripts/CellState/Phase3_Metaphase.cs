internal class Phase3_Metaphase : IState
{
    private readonly Cell_StateMachine _cellFSM;
    private string _help = "Homologous pairs of chromosomes line up on metaphase plate.\n" +
                           "The pairs then attach to spindle fibers of opposite poles";

    public Phase3_Metaphase(Cell_StateMachine cellFsm)
    {
        _cellFSM = cellFsm;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cellFSM.HoldNextPhase();
        _cellFSM.SetHelpText(_help);
        
        _cellFSM.CentrosomesToPoles(1f);
        _cellFSM.MetaphasePlateActivate(2f);
        _cellFSM.ChromosomesToPlate(3f);

        if (_cellFSM.Cycle == 1) { _cellFSM.CentrosomesAttachToChromosomes(4f); }
        else                     { _cellFSM.CentrosomesAttachToCentromeres(4f); }
        
        _cellFSM.ClearForNextPhase(5f);
    }

    public void OnExit()
    {
        _cellFSM.SetHelpText("");
        _cellFSM.CancelInvoke();
    }
}