using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginOnClick : MonoBehaviour {
	
	public InputField name;
	public InputField password;

	public void clear(){
		name.text = "";
		password.text = "";
	}
}
