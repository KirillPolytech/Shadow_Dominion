using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorRoomPlayer : NetworkRoomPlayer
    {
        #region Client

        [Client]
        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            MirrorPlayersSyncer.Instance.UpdateReadyState(newReadyState, UserData.Instance.Nickname);
            
            Debug.Log($"[Client] Ready state changed: {newReadyState}");
        }

        #endregion
    }
}