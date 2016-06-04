using UnityEngine;
using MonsterLove.StateMachine;
using GameLogic;
using System.Collections.Generic;

public partial class SceneManagerScript : MonoBehaviour
{
    public enum States
    {
        Idle,
        Selected,
        Executing
    }

    private StateMachine<States> _stateMachine;

    // This is to be called by the Start method in the main SceneManagerScript definition
    void SetupStateMachine()
    {
        _stateMachine = StateMachine<States>.Initialize(this);
        _stateMachine.ChangeState(States.Idle);
    }

    #region STATE MACHINE

    #region IDLE STATE

    // For now, nothing to do

    #endregion IDLE STATE

    #region SELECTED STATE

    #endregion SELECTED STATE

    List<Hexagon> _area;
    void Selected_Enter()
    {
        Debug.Log("ENTERING SELECTED STATE");
        _area = GameLogic.Grid_Math.Area.EntityMovementRange(Map, SelectedEntity);
        foreach(Hexagon hexagon in area)
        {
            _gridManager.PaintHexagon(hexagon.X, hexagon.Y, Color.green);
        }
    }

    void Selected_Update()
    {
        _gridManager.ClearGrid();
        // We highlight the area
        _gridManager.PaintList(_area, Color.green);
        // We highlight the path

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

    }

    void Selected_Exit()
    {
        Debug.Log("EXITING SELECTED STATE");
        _gridManager.ClearGrid();
    }

    #endregion STATE MACHINE



}
