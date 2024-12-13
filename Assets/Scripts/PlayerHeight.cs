using HellBeavers.Player;
using UnityEngine;
using Zenject;

public class PlayerHeight : IFixedTickable
{
    private readonly Transform _height;
    private readonly Transform _target;
    private readonly PlayerHeightData _playerHeightData;
    
    public Vector3 Current => _height.position;
    
    [Inject]
    public PlayerHeight(PlayerHeightData playerHeightData, Transform height, Transform target)
    {
        _playerHeightData = playerHeightData;
        _height = height;
        _target = target;
    }
    
    public void FixedTick()
    {
        CalculatingHeight();
    }

    private void CalculatingHeight()
    {
        Physics.Raycast(_height.position, 
            Vector3.down, out RaycastHit hit, _playerHeightData.distance, _playerHeightData.layerMask);
        float newPos = hit.point.y + _playerHeightData.targetHeight;
        _height.position = new Vector3(_target.position.x, newPos, _target.position.z);
    }
}