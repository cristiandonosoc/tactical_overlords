using UnityEngine;
using MonsterLove.StateMachine;
using GameLogic;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using System.Collections;

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

    void Idle_Enter()
    {
        Debug.Log("ENTERING IDLE STATE");
    }

    void Idle_Exit()
    {
        Debug.Log("EXITING IDLE STATE");
    }

    // For now, nothing to do
    public void Idle_EntityClick(EntityScript clickedEntityScript)
    {
        SelectedEntityScript = clickedEntityScript;
        _stateMachine.ChangeState(States.Selected);
    }

    public void Idle_HexagonClick(HexagonScript clickedHexagonScript)
    {
        // Do nothing
    }

    #endregion IDLE STATE

    #region SELECTED STATE


    internal EntityScript SelectedEntityScript { get; private set; }
    List<Hexagon> _area;
    HashSet<uint> _areaSet;
    List<Hexagon> _path = new List<Hexagon>();
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
        _path.Clear();
        GameLogic.Grid_Math.Path.GetPath(Map, _path, _areaSet,
                                         entityHexagon.X, entityHexagon.Y,
                                         (int)roundedCoords.x, (int)roundedCoords.y);

        _gridManager.PaintList(_path, Color.red);
    }

    public void Selected_EntityClick(EntityScript clickedEntityScript)
    {
        SelectedEntityScript = clickedEntityScript;
        _stateMachine.ChangeState(States.Selected, StateTransition.ForceSafe);
    }

    public void Selected_HexagonClick(HexagonScript clickedHexagonScript)
    {
        if (_areaSet.Contains(clickedHexagonScript.Hexagon.Key))
        {
            // TODO(Cristian): Trigger move action
            _actionList.Clear();
            _actionIndex = 0;
            Map.Move(SelectedEntityScript.Entity, clickedHexagonScript.Hexagon, _actionList);
            // We move to the executing state
            _stateMachine.ChangeState(States.Executing);
        }
    }

    void Selected_Exit()
    {
        Debug.Log("EXITING SELECTED STATE");
    }

    #endregion SELECTED STATE

    #region EXECUTING STATE

    int _actionIndex = 0;
    List<Action> _actionList = new List<Action>();

    #endregion EXECUTING STATE

    void Executing_Enter()
    {

        Debug.Log("ENTERING EXECUTING STATE");
        _gridManager.ClearGrid();
        _gridManager.PaintList(_path, Color.red);
        StopCoroutine("ExecuteActions");
        StartCoroutine("ExecuteActions");
    }

    void Executing_Exit()
    {
        _gridManager.ClearGrid();
        Debug.Log("EXITING EXECUTING STATE");
    }

    IEnumerator ExecuteActions()
    {
        while (_actionIndex < _actionList.Count)
        {
            // We take the next action
            Action action = _actionList[_actionIndex];
            ++_actionIndex;
            switch (action.Type)
            {
                case Action.ActionType.Move:
                {
                        yield return MovementAction(action);
                } break;
            }
        }

        // When we're done, we change the state back to idle
        _stateMachine.ChangeState(States.Idle);
    }

    IEnumerator MovementAction(Action action)
    {
        Vector3 hexCoords = new Vector3(action.Target.X, action.Target.Y);
        Vector3 targetPos = HexCoordsUtils.HexToWorld(HexWorld, hexCoords);

        Vector3 dir = (targetPos - SelectedEntityScript.transform.position).normalized;

        float distance = Vector3.Distance(SelectedEntityScript.transform.position, targetPos);
        float distDelta = SelectedEntityScript.speed * Time.deltaTime;
        while (distance > distDelta)
        {
            // We move, then yield and recompare next frame
            SelectedEntityScript.transform.position += dir * distDelta;
            yield return null;

            // We update the move
            distance = Vector3.Distance(SelectedEntityScript.transform.position, targetPos);
        	distDelta = SelectedEntityScript.speed * Time.deltaTime;
        }

        SelectedEntityScript.transform.position = targetPos;
    }


    #endregion STATE MACHINE



}
