using Mirror;
using NaughtyAttributes;
using Shadow_Dominion;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class MirrorRagdollInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject root;

    [Button]
    public void Initialize()
    {
        BoneController[] bones = root.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyReliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyReliable>();
            if (!networkRigidbodyReliable)
                bone.AddComponent<NetworkRigidbodyReliable>();
        }
    }
    
    [Button]
    public void Delete()
    {
        BoneController[] bones = root.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyReliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyReliable>();
            if (networkRigidbodyReliable)
                Destroy(networkRigidbodyReliable);
        }
    }
}