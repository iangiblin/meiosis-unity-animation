
internal class Phase2_Prophase : IState
{
    private Cell_StateMachine _cellStateMachine;
    private string _help = "Nuclear envelope breaks down\n" +
                           "DNA forms into chromosomes.\n" +
                           "Centrosomes move to the poles of the cell\n" +
                           "Chromosomes pair up with specified homolog\n" +
                           "Cross-over event begins; homologous chromosomes trade pieces.\n" 
                           ;

    public Phase2_Prophase(Cell_StateMachine cellStateMachine)
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

        _cellStateMachine.CellGrow(1f);
        _cellStateMachine.NucleusWallRemove(2f);

        if (_cellStateMachine.Cycle == 2)
        {
            // for the animation we just delete one of each big & small
            // chromosomes because they don't form in cycle II
            
            _cellStateMachine.DeleteOneChromosomeOfEachSize();
            _cellStateMachine.ChromosomesActivate(3f);
        }
        else
        {
            _cellStateMachine.ChromosomesActivate(3f);
            _cellStateMachine.ChromosomesPairUp(4f);
        }

        _cellStateMachine.ClearForNextPhase(5f);
    }

    public void OnExit()
    {
        _cellStateMachine.CancelInvoke();
    }
}
