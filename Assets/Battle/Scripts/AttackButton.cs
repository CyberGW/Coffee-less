using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour {

	private GameObject attacksPanel;
	private bool panelActive;

	// Use this for initialization
	void Start () {
		attacksPanel = GameObject.Find("BattleCanvas").transform.Find("AttacksPanel").gameObject;
		attacksPanel.SetActive (false);
		panelActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setPanelActive() {
		if (panelActive) {
			attacksPanel.SetActive (false);
			panelActive = false;
		} else {
			updateAttacksPanel ();
			attacksPanel.SetActive (true);
			panelActive = true;
		}
	}


	public void updateAttacksPanel() { 
		GameObject UI1 = attacksPanel.transform.Find("Margin").Find("MagicSpell1").gameObject;
		GameObject UI2 = attacksPanel.transform.Find("Margin").Find("MagicSpell2").gameObject;
		Player player = GameObject.Find ("BattleCode").GetComponent<MainBattle> ().player;
		updateAttacksPanelHelper (UI1, player.Special1, player.Magic);
		updateAttacksPanelHelper (UI2, player.Special2, player.Magic);
	}

	private void updateAttacksPanelHelper(GameObject UI, SpecialMove specialMove, int magic) {
		UI.transform.Find ("Magic").GetComponent<Text> ().text = "Magic: " + specialMove.Magic.ToString();
		UI.transform.Find ("Desc").GetComponent<Text> ().text = specialMove.Desc;
		if (magic < specialMove.Magic) {
			UI.transform.Find ("Button").GetComponent<Button> ().interactable = false;
		}
	}
}
