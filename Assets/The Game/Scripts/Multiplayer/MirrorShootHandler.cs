using System;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion.Network
{
    public class MirrorShootHandler : NetworkBehaviour
    {
        private const int ShootRange = 500;

        private Ak47 _ak47;
        private Action<Vector3, Vector3> _cachedOnFire;

        public void Construct(Ak47 ak47)
        {
            _ak47 = ak47;

            _cachedOnFire = (origin, direction) => CmdCastRay(UserData.Instance.Nickname, origin, direction);

            _ak47.OnFired += _cachedOnFire.Invoke;
        }

        private void OnDestroy()
        {
            _ak47.OnFired -= _cachedOnFire.Invoke;
        }

        [Command]
        private void CmdCastRay(string killerNick, Vector3 origin, Vector3 direction)
        {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit, ShootRange))
                return;

            //Debug.Log($"[Server] Hit {hit.collider.name}");

            if (!hit.collider.TryGetComponent(out BoneController boneController)) 
                return;
            
            NetworkIdentity boneNetIdentity = boneController.GetComponentInParent<NetworkIdentity>();

            if (boneNetIdentity != null)
            {
                RpcShowImpact(killerNick, boneNetIdentity.netId, direction, boneController.name);
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