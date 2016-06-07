using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{

    public delegate void StateMethod();

    public class StateMachine<T> where T : struct, IConvertible, IComparable
    {
        private List<StateMethod> _enterDelegates;
        private List<StateMethod> _updateDelegates;
        private List<StateMethod> _exitDelegates;

        public T State { get; private set; }

        #region STATIC HELPERS

        private static void AddMethodFromComponent(object component, List<StateMethod> methodList,
                                                   string methodName, BindingFlags bindingFlags)
        {
            MethodInfo enterMethod = component.GetType().GetMethod(methodName, bindingFlags);
            if (enterMethod != null)
            {
                methodList.Add((StateMethod)Delegate.CreateDelegate(typeof(StateMethod),
                                                                    component,
                                                                    enterMethod,
                                                                    false));
            }
            else
            {
                methodList.Add(null);
            }
        }

        #endregion STATIC HELPERS

        public StateMachine(MonoBehaviour component)
        {
            Array values = Enum.GetValues(typeof(T));
            if (values.Length < 1)
            {
                throw new ArgumentException("Enum doesnt have at least one state");
            }

            // We initialize the lists
            _enterDelegates = new List<StateMethod>(values.Length);
            _updateDelegates = new List<StateMethod>(values.Length);
            _exitDelegates = new List<StateMethod>(values.Length);

            Type componentType = component.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance |
                                        BindingFlags.DeclaredOnly |
                                        BindingFlags.Public |
                                        BindingFlags.NonPublic;
            
            // For each state, we look for the correct methods
            foreach (T value in values)
            {
                // We search for the enter method
                string stateName = value.ToString();

                string enterName = string.Format("{0}_Enter", stateName);
                AddMethodFromComponent(component, _enterDelegates, enterName, bindingFlags);

                string updateName = string.Format("{0}_Update", stateName);
                AddMethodFromComponent(component, _updateDelegates, updateName, bindingFlags);

                string exitName = string.Format("{0}_Exit", stateName);
                AddMethodFromComponent(component, _exitDelegates, exitName, bindingFlags);
            }
        }

        public void ChangeState(T newState)
        {
            // We call the exit method (if exists)
            StateMethod exitMethod = _exitDelegates[State.ToInt32(null)];
            if (exitMethod != null)
            {
                exitMethod();
            }

            State = newState;

            // We call the enter method (if exists)
            StateMethod enterMethod = _enterDelegates[State.ToInt32(null)];
            if (enterMethod != null)
            {
                enterMethod();
            }
        }

        public void Update()
        {
            // We call the update Method (if exists)
            StateMethod updateMethod = _updateDelegates[State.ToInt32(null)];
            if (updateMethod != null)
            {
                updateMethod();
            }
        }
    }
}
