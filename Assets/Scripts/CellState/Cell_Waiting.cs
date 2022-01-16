internal class Cell_Waiting : IState
{
    private Cell_StateMachine _cellStateMachine;
    private string _help = "Camera controls:\n\n" +
                           "<B>W,A,S,D</B> : Forward, Back, Left, Right\n" +
                           "<B>Q,E</B> : Up, Down\n" +
                           "<B><I>Right</I> mouse button</B> : Look around\n\n" +
                           "Press SPACE to enter the first phase";

    public Cell_Waiting(Cell_StateMachine cellStateMachine)
    {
        _cellStateMachine = cellStateMachine;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cellStateMachine.SetHelpText(_help);
        
        _cellStateMachine.MetaphasePlateDeactivate();
        _cellStateMachine.ContractileLineDeactivate();
        _cellStateMachine.NewCellWallsHide();
        _cellStateMachine.ChromosomesOff();
    }

    public void OnExit()
    {
        _cellStateMachine.SetHelpText("");
        _cellStateMachine.CancelInvoke();
    }
}
