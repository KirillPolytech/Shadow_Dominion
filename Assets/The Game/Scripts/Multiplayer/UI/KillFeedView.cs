using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killer;
    [SerializeField] private TextMeshProUGUI victim;
    [SerializeField] private RawImage weaponImage;

    public void SetKiller(string text) => killer.text = text;
    public void SetVictim(string text) => victim.text = text;
    public void SetWeaponImageState(bool state) => weaponImage.gameObject.SetActive(state);
}
