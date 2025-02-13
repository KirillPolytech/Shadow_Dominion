using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorShootHandler : NetworkBehaviour
    {
        private const int ShootRange = 500;

        [Command]
        public void CmdCastRay(Vector3 origin, Vector3 direction)
        {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit, ShootRange))
                return;

            Debug.Log($"[Server] Hit {hit.collider.name}");

            if (!hit.collider.TryGetComponent(out BoneController boneController)) 
                return;
            
            NetworkIdentity boneNetIdentity = boneController.GetComponentInParent<NetworkIdentity>();

            if (boneNetIdentity != null)
            {
                RpcShowImpact(boneNetIdentity.netId, direction, boneController.name);
            }
        }

        [ClientRpc]
        public void RpcShowImpact(uint netId, Vector3 direction, string boneName)
        {
            if (!NetworkClient.spawned.TryGetValue(netId, out NetworkIdentity identity)) 
                return;
            
            BoneController[] boneController = identity.GetComponentsInChildren<BoneController>();

            foreach (var bone in boneController)
            {
                if (bone.name == boneName)
                {
                    bone.GetComponent<BoneController>().ReceiveDamage(direction);
                }
            }
        }
    }
}