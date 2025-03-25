using Mirror;
using UnityEngine;

namespace Shadow_Dominion.Network
{
    public class MirrorShootHandler : NetworkBehaviour
    {
        private const int ShootRange = 500;

        private Ak47 _ak47;

        public void Construct(Ak47 ak47)
        {
            _ak47 = ak47;

            _ak47.OnFired += CmdCastRay;
        }

        private void OnDestroy()
        {
            _ak47.OnFired -= CmdCastRay;
        }

        [Command]
        private void CmdCastRay(Vector3 origin, Vector3 direction)
        {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit, ShootRange))
                return;

            Debug.Log($"[Server] Hit {hit.collider.name}");

            if (!hit.collider.TryGetComponent(out BoneController boneController)) 
                return;
            
            NetworkIdentity boneNetIdentity = boneController.GetComponentInParent<NetworkIdentity>();

            if (boneNetIdentity != null)
            {
                RpcShowImpact(netIdentity.connectionToClient.address, boneNetIdentity.netId, direction, boneController.name);
            }
        }

        [ClientRpc]
        private void RpcShowImpact(string killerName, uint targetnetID, Vector3 direction, string boneName)
        {
            if (!NetworkClient.spawned.TryGetValue(targetnetID, out NetworkIdentity identity)) 
                return;
            
            BoneController[] boneController = identity.GetComponentsInChildren<BoneController>();

            foreach (var bone in boneController)
            {
                if (bone.name == boneName)
                {
                    bone.GetComponent<BoneController>().ReceiveDamage(direction, killerName);
                }
            }
        }
    }
}