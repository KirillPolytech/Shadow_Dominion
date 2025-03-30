using System.Collections.Generic;
using System.Linq;
using Shadow_Dominion;
using UnityEngine;

public class LevelPlayerListing : MonoSingleton<LevelPlayerListing>
{
    private readonly List<LevelPlayerView> _instances = new();
        
    [SerializeField] private Transform content;
    [SerializeField] private Transform finishContent;
    [SerializeField] private LevelPlayerView viewPrefab;

    public void AddView(PlayerViewData[] views)
    {
        RemoveViews();
        
        foreach (var view in views)
        {
            LevelPlayerView levelPlayerView = Instantiate(viewPrefab, content);
            LevelPlayerView levelPlayerView2 = Instantiate(viewPrefab, finishContent);
            
            levelPlayerView.Initialize($"<color=white>{view.Nick}</color>", view.Kills.ToString());
            levelPlayerView2.Initialize($"<color=white>{view.Nick}</color>", view.Kills.ToString());
            
            _instances.Add(levelPlayerView);
            _instances.Add(levelPlayerView2);
        }
    }

    private void RemoveViews()
    {
        foreach (var view in _instances.Where(view => view && view.gameObject))
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
