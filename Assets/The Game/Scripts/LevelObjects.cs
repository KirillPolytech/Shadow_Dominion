using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class LevelObjects : MonoBehaviour
    {
        private List<Transform> _objects;
        private LevelSO _levelSo;
        private float _timer, _timeToSpawn;

        private int _ind;
        
        [Inject]
        public void Construct(LevelSO levelSo)
        {
            _levelSo = levelSo;
        }

        private void Awake()
        {
            _objects = GetComponentsInChildren<Transform>().ToList();
            _objects.Remove(transform);

            _timeToSpawn = (float)_levelSo.InitializeWaitTime / _objects.Count;
            
            foreach (var o in _objects)
            {
                o.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (_ind >= _objects.Count - 1)
                return;
            
            _timer += Time.fixedDeltaTime;
            
            if (_timer < _timeToSpawn)
                return;
            
            _objects[_ind++].gameObject.SetActive(true);

            _timer = 0;
        }
    }
}