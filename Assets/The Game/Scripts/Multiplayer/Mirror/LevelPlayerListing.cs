using System.Collections.Generic;
using Shadow_Dominion;
using TMPro;
using UnityEngine;

public class LevelPlayerListing : MonoSingleton<LevelPlayerListing>
{
    private readonly List<TextMeshProUGUI> _instances = new();
        
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI viewPrefab;

    public void AddView(PlayerViewData[] views)
    {
        RemoveViews();
        
        foreach (var view in views)
        {
            TextMeshProUGUI text = Instantiate(viewPrefab, content);
            text.text = $"<color=green>{view.Address}</color> {view.Kills}";
            _instances.Add(text);
        }
    }

    private void RemoveViews()
    {
        foreach (var view in _instances)
        {
            Destroy(view.gameObject);
        }
        
        _instances.Clear();
    }

    public void Reset()
    {
        RemoveViews();
    }
}
