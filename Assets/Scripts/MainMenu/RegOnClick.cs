using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RegOnClick : MonoBehaviour {
	public InputField name;
	public InputField password;
	public InputField confPassword;

	public void clear(){
		name.text = "";
		password.text = "";
		confPassword.text = "";
	}

	public void check(){
		
		if (password.text == confPassword.text) {
			print (password.text+" "+confPassword.text);
		}
	}
}

