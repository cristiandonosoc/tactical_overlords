using UnityEngine;
using MonsterLove.StateMachine;
using GameLogic;
using System.Collections.Generic;
using Assets.Scripts.Utils;

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
    public void Idle_EntityClick(EntityScript clickedEntityScript)
    {
        SelectedEntityScript = clickedEntityScript;
        _stateMachine.ChangeState(States.Selected);
    }

    #endregion IDLE STATE

    #region SELECTED STATE

    #endregion SELECTED STATE

    internal EntityScript SelectedEntityScript { get; private set; }
    List<Hexagon> _area;
    HashSet<uint> _areaSet;
    void Selected_Enter()
    {
        Debug.Log("ENTERING SELECTED STATE");
        _area = GameLogic.Grid_Math.Area.EntityMovementRange(Map, SelectedEntityScript.Entity);
        _areaSet = new HashSet<uint>();
        foreach (Hexagon hexagon in _area)
        {
            _areaSet.Add(hexagon.Key);
        }
    }

    void Selected_Update()
    {
        _gridManager.ClearGrid();
        // We highlight the area
        _gridManager.PaintList(_area, Color.green);
        // We highlight the path

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 roundedCoords = HexCoordsUtils.GetHexRoundedWorldPosition(HexWorld, mousePos);

        // We get the path
        Hexagon entityHexagon = SelectedEntityScript.Entity.Hexagon;
        List<Hexagon> path = new List<Hexagon>();
        GameLogic.Grid_Math.Path.GetPath(Map, path, _areaSet,
                                         entityHexagon.X, entityHexagon.Y,
                                         (int)roundedCoords.x, (int)roundedCoords.y);

        _gridManager.PaintList(path, Color.red);
    }

    public void Selected_EntityClick(EntityScript clickedEntityScript)
    {
        SelectedEntityScript = clickedEntityScript;
        _stateMachine.ChangeState(States.Selected, StateTransition.ForceSafe);
    }

    void Selected_Exit()
    {
        Debug.Log("EXITING SELECTED STATE");
    }

    #endregion STATE MACHINE



}
