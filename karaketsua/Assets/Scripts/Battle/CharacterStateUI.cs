using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CharacterStateUI : MonoBehaviour {

    public Image faceImage;
    public Text name;
    public Slider HPbar;
    public Slider skillBar;

    public void Init(string charaName,float hp,float skill)
    {
        faceImage = Resources.Load<Image>("Icon/"+charaName);
        name.text = charaName;


    }

}
