internal class Phase4_Anaphase : IState
{
    private Cell_StateMachine _cellFSM;
    private string _help = "Homologous chromosomes are torn apart at opposite poles;\n" +
                           "sister chromatids stay united.";

    public Phase4_Anaphase(Cell_StateMachine cellFSM)
    {
        _cellFSM = cellFSM;
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _cellFSM.HoldNextPhase();
        _cellFSM.SetHelpText(_help);
        

        if (_cellFSM.Cycle == 1)
        {
            _cellFSM.ChromosomesUnPair();
            _cellFSM.ChromosomesFlyToPoles(2f);
        }
        else
        {
            _cellFSM.CentromeresFlyToPoles(2f);
        }
        
        _cellFSM.ClearForNextPhase(6f);
    }

    public void OnExit()
    {
        _cellFSM.SetHelpText("");
    }
}