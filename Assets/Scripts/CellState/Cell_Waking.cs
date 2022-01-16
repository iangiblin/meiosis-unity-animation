internal class Cell_Waking : IState
{
    private Cell_StateMachine _cellStateMachine;
    private string _help = "The cell is waking up; please wait.";

    public Cell_Waking(Cell_StateMachine cellStateMachine)
    {
        _cellStateMachine = cellStateMachine;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cellStateMachine.SetHelpText(_help);
    }

    public void OnExit()
    {
        _cellStateMachine.SetHelpText("");
    }
}