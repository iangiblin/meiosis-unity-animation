using System;
using TMPro;
using UnityEngine;

public class UITextListener : MonoBehaviour
{
    [SerializeField] private TMP_Text _stateText;
    [SerializeField] private TMP_Text _helpText;
    [SerializeField] private TMP_Text _pressSpace;
    [SerializeField] private TMP_Text _toDoList;

    private Cell_StateMachine _cellStateMachine;
    public static UITextListener Instance { get; set; }

    private void Awake()
    {
        // cheap Singleton
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        _cellStateMachine = null;
        DontDestroyOnLoad(this);
    }

    public void RegisterCellStateMachine(Cell_StateMachine machine)
    {
        Debug.Log($"UI: Registering cell FSM on {machine}");
        // we may have previously registered with a different cell
        if (_cellStateMachine != null) { DeregisterEvents(_cellStateMachine); }
        
        // now register with the new one
        _cellStateMachine = machine;
        RegisterEvents(_cellStateMachine);
    }

    private void RegisterEvents(Cell_StateMachine machine)
    {
        machine.OnCellStateChanged += Handle_StateChanged;
        machine.HelpTextUpdate     += Handle_HelpChanged;
        machine.ReadyForNextPhase  += Handle_ReadyForNextPhase;
        _helpText.enabled = true;
    }

    private void DeregisterEvents(Cell_StateMachine machine)
    {
        machine.OnCellStateChanged -= Handle_StateChanged;
        machine.HelpTextUpdate     -= Handle_HelpChanged;
        machine.ReadyForNextPhase  -= Handle_ReadyForNextPhase;
        _helpText.enabled = false;
    }

    private void Handle_HelpChanged(string help)
    {
        if (_helpText == null) { return; }
        _helpText.SetText(help);
    }

    private void Handle_StateChanged(IState state, Cell_StateMachine machine)
    {
        if (_stateText == null) { return; }

        string _state = machine.CurrentStateType.ToString();
        int cycle = machine.Cycle;
        _stateText.SetText($"{_state} {cycle}");
        _pressSpace.enabled = false;

        _toDoList.enabled = false;
    }

    private void Handle_ReadyForNextPhase()
    {
        _pressSpace.enabled = true;
    }
}
