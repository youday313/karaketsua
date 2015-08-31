using UnityEngine;
using System.Collections;

public class ChargeModeSwitcher : MonoBehaviour {

	
	/*
	all the _ChargeMode available in TapDetector
		Once, 
		Clamp, 
		Loop, 
		PingPong,
	*/

	// Use this for initialization
	void Start () {
		_ChargeMode chargeMode=TapDetector.GetChargeMode();
		if(chargeMode==_ChargeMode.Once) type=0;
		else if(chargeMode==_ChargeMode.Clamp) type=1;
		else if(chargeMode==_ChargeMode.Loop) type=2;
		else if(chargeMode==_ChargeMode.PingPong) type=3;
	}
	
	
	private int type=0;
	void OnGUI(){
		
		//string displayText="";
		//if(type==0) displayText=
		
		if(GUI.Button(new Rect(Screen.width-150, 40, 130, 40), TapDetector.GetChargeMode().ToString())){
			type+=1;
			if(type>3)type=0;
			
			if(type==0) TapDetector.SetChargeMode(_ChargeMode.Once);
			else if(type==1) TapDetector.SetChargeMode(_ChargeMode.Clamp);
			else if(type==2) TapDetector.SetChargeMode(_ChargeMode.Loop);
			else if(type==3) TapDetector.SetChargeMode(_ChargeMode.PingPong);
		}
	}
	
}
