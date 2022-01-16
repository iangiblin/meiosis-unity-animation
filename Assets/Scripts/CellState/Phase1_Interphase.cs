internal class Phase1_Interphase : IState
{
    // Cell grows
    // DNA copies itself
    
    private Cell_StateMachine _cellStateMachine;
    private string _help = "G1: Cells and synthesize mRNA and proteins.\n" +
                           "S: Chromosomes are copied\n" +
                           "G2: More proteins, more growth";
    public Phase1_Interphase(Cell_StateMachine cellStateMachine)
    {
        _cellStateMachine = cellStateMachine;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cellStateMachine.HoldNextPhase();
        _cellStateMachine.SetHelpText(_help);
        
        _cellStateMachine.CellReset();
        _cellStateMachine.CopyBigDNA(1f);
        _cellStateMachine.CellGrow(2f);
        _cellStateMachine.CopyBigDNA(3f);
        _cellStateMachine.CopyBigDNA(4f);
        _cellStateMachine.CopyBigDNA(5f);
        _cellStateMachine.CellGrow(6f);

        _cellStateMachine.ClearForNextPhase(7f);
    }

    public void OnExit()
    {
        _cellStateMachine.SetHelpText("");
        _cellStateMachine.CancelInvoke();
    }
}
