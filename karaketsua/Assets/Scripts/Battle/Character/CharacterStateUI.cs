using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CharacterStateUI : MonoBehaviour {

    public Image faceImage;
    //public Text name;
    public Slider HPbar;
    public Slider skillBar;

    public void Init(Character character)
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("CharacterStateUIParent").transform, false);
        faceImage = Resources.Load<Image>("Icon/"+character.characterParameter.charaName);
        //name.text = character.characterParameter.charaName;
        HPbar.maxValue = character.characterParameter.HP;
        skillBar.maxValue = character.characterParameter.skillPoint;
        UpdateUI(character);

    }
    public void UpdateUI(Character character)
    {
        HPbar.value = character.characterParameter.HP;

        skillBar.value = character.characterParameter.skillPoint;


    }

}
