using Mirror;
using UnityEngine;

public class TestNetwork : NetworkManager
{
    [SerializeField]
    private TestSpawnable testSpawnable;
    
    public override void OnStartHost()
    {
        base.OnStartHost();
        
        GameObject obj = Instantiate(testSpawnable.gameObject);
        NetworkServer.Spawn(obj); // Отправляет объект всем клиентам
    }
}
