using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListController : MonoBehaviour {
    public Button[] buttons;

	void Awake () {
        buttons = transform.GetComponentsInChildren<Button>();
	}
}
