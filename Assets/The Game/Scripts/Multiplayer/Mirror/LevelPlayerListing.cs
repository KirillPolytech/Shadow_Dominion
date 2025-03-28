using System.Collections.Generic;
using Shadow_Dominion;
using UnityEngine;

public class LevelPlayerListing : MonoSingleton<LevelPlayerListing>
{
    private readonly List<LevelPlayerView> _instances = new();
        
    [SerializeField] private Transform content;
    [SerializeField] private LevelPlayerView viewPrefab;

    public void AddView(PlayerViewData[] views)
    {
        RemoveViews();
        
        foreach (var view in views)
        {
            LevelPlayerView levelPlayerView = Instantiate(viewPrefab, content);
            levelPlayerView.Initialize($"<color=white>{view.Nick}</color>", view.Kills.ToString());
            _instances.Add(levelPlayerView);
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
