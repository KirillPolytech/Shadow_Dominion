using System.Linq;
using Mirror;
using NaughtyAttributes;
using Shadow_Dominion;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class MirrorRagdollInitializer : MonoBehaviour
{
    [SerializeField]
    private GameObject ragdollRoot;

    [SerializeField]
    private GameObject animRoot;
    
    [Button]
    public void InitializeRagdoll()
    {
        BoneController[] bones = ragdollRoot.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyUnreliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyUnreliable>();
            if (!networkRigidbodyReliable)
                bone.AddComponent<NetworkRigidbodyUnreliable>();
        }
    }
    
    [Button]
    public void DeleteRagdoll()
    {
        BoneController[] bones = ragdollRoot.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyUnreliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyUnreliable>();
            if (networkRigidbodyReliable)
                DestroyImmediate(bone.GetComponent<NetworkRigidbodyUnreliable>());
        }
    }

    [Button]
    public void Initialize()
    {
        BoneController[] bones = ragdollRoot.GetComponentsInChildren<BoneController>();

        Transform[] childs = animRoot.transform.GetComponentsInChildren<Transform>();
        
        foreach (var bone in bones)
        {
            Transform child = childs.First(x => x.name == bone.name);
            
            if (child == null)
                Debug.LogError($"Cant find: {bone.name}");
            
            NetworkTransformUnreliable ntr = child.GetComponent<NetworkTransformUnreliable>();
            
            if (ntr == null)
                child.AddComponent<NetworkTransformUnreliable>();
        }
    }

    [Button]
    public void Delete()
    {
        NetworkTransformUnreliable[] bones = animRoot.GetComponentsInChildren<NetworkTransformUnreliable>();

        foreach (var bone in bones)
        {
            NetworkTransformUnreliable networkRigidbodyReliable = bone.GetComponent<NetworkTransformUnreliable>();
            if (networkRigidbodyReliable)
                DestroyImmediate(networkRigidbodyReliable);
        }
    }
}