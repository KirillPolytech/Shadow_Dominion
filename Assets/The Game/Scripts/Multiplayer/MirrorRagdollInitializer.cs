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
    public void Initialize()
    {
        BoneController[] bones = ragdollRoot.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyReliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyReliable>();
            if (!networkRigidbodyReliable)
                bone.AddComponent<NetworkRigidbodyReliable>();
        }

        Transform[] childs = animRoot.transform.GetComponentsInChildren<Transform>();
        
        foreach (var bone in bones)
        {
            Transform child = childs.First(x => x.name == bone.name);
            
            if (child == null)
                Debug.LogError($"Cant find: {bone.name}");
            
            NetworkTransformReliable ntr = child.GetComponent<NetworkTransformReliable>();
            
            if (ntr == null)
                child.AddComponent<NetworkTransformReliable>();
        }
    }

    [Button]
    public void Delete()
    {
        BoneController[] bones = ragdollRoot.GetComponentsInChildren<BoneController>();

        foreach (var bone in bones)
        {
            NetworkRigidbodyReliable networkRigidbodyReliable = bone.GetComponent<NetworkRigidbodyReliable>();
            if (networkRigidbodyReliable)
                DestroyImmediate(networkRigidbodyReliable);
        }
    }
}