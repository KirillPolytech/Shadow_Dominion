using Mirror;
using Shadow_Dominion;
using UnityEngine;
using Object = System.Object;

public class MirrorShootHandler : NetworkBehaviour
{
    private const int ShootRange = 500;
    
    [Command]
    public void Raycast(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, ShootRange))
        {
            Debug.Log($"[Server] Hit {hit.collider.name}");

            if (hit.collider.TryGetComponent(out BoneController boneController))
            {
                
            }

            RpcShowImpact(hit.point, hit.normal);
        }

        return;
    }
    
    [ClientRpc]
    public void RpcShowImpact(Vector3 hitPosition, Vector3 hitNormal)
    {
        
    }
}
