using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class LevelObjects : MonoBehaviour
    {
        private List<Transform> _objects;
        private LevelStateMachine _levelStateMachine;

        private int _ind;

        [Inject]
        public void Construct(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;
        }

        private void Awake()
        {
            _objects = GetComponentsInChildren<Transform>().ToList();
            _objects.Remove(transform);

            foreach (var o in _objects)
            {
                o.gameObject.SetActive(false);
            }

            _levelStateMachine.OnStateChanged += OnStateChanged;
        }

        private void OnDestroy()
        {
            _levelStateMachine.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(IState state)
        {
            if (state.GetType() == typeof(GameplayState))
            {
                foreach (var VARIABLE in _objects)
                {
                    VARIABLE.gameObject.SetActive(true);
                }

                return;
            }

            foreach (var VARIABLE in _objects)
            {
                VARIABLE.gameObject.SetActive(false);
            }
        }
    }
}