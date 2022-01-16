internal class Phase5_Telophase : IState
{
    private readonly Cell_StateMachine _cellStateMachine;
    private string _help = "Nuclear membrane re-forms around chromosomes and they start to unravel. \n" +
                           "(Not guaranteed in all cells.)\n" +
                           "Ring of proteins called the contractile ring forms \n" +
                           "The ring shrinks pinching the membrane towards the " +
                           "middle and causing a cleavage furrow. \n" +
                           "Resulting in the creation of two haploid daughter cells.\n" +
                           "Cytokinesis and telophase occur at the same time.";

    public Phase5_Telophase(Cell_StateMachine cellStateMachine)
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

        _cellStateMachine.MetaphasePlateDeactivate();
        _cellStateMachine.ChromosomesOff();
        _cellStateMachine.CopySmallDNA();
        _cellStateMachine.CopySmallDNA(1);
        _cellStateMachine.CopySmallDNA(2);
        _cellStateMachine.CopySmallDNA(3);
        _cellStateMachine.CopySmallDNA(4);
        _cellStateMachine.CopySmallDNA(5);
        
        _cellStateMachine.ContractileLineActivate(5);
        _cellStateMachine.OriginalCellWallConstricts(5);
        _cellStateMachine.ContractileLineDeactivate(7);
        _cellStateMachine.NewCellWallsForm(7);
        _cellStateMachine.OriginalCellWallDisappears(9);
        
        _cellStateMachine.CellDivides(10f);
        
        // we will never reach this line because the cell gets destroyed
        _cellStateMachine.ClearForNextPhase(12f);
    }

    public void OnExit()
    {
        _cellStateMachine.NewCellWallsHide();
        _cellStateMachine.SetHelpText("");
        _cellStateMachine.CancelInvoke();
    }
}
