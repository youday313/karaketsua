using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStatusPanel : MonoBehaviour 
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text nextLevelText;
    [SerializeField]
    private Slider hpBar;

    public void Initialize(Image ic, string lv, string nextLv, float hp)
    {
        icon = ic;
        levelText.text = lv;
        nextLevelText.text = nextLv;
        hpBar.value = hp;
    }
}
