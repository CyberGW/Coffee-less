using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>Script to handle the function of the Attack Button in the Battle Scene</summary>
public class BattleButtons : MonoBehaviour {

	private GameObject attacksPanel;
	private bool panelActive;

	/// <summary>At first hide the attack panel and set <see cref="panelActive"/> accordingly</summary>
	void Start () {
		attacksPanel = GameObject.Find("BattleCanvas").transform.Find("AttacksPanel").gameObject;
		attacksPanel.SetActive (false);
		panelActive = false;
	}

	/// <summary>Called when the attack button is pressed, and shows attack panel is previously hidden or vice versa</summary>
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

	/// <summary>
	/// Shows the player menu.
	/// </summary>
	public void showPlayerMenu() {
		SceneManager.LoadSceneAsync ("SwitchPlayer", LoadSceneMode.Additive);
	}

	/// <summary>
	/// Updates the text and buttons in the attack panel, in case a new player has been switched in
	/// </summary>
	public void updateAttacksPanel() { 
		GameObject UI1 = attacksPanel.transform.Find("Margin").Find("MagicSpell1").gameObject;
		GameObject UI2 = attacksPanel.transform.Find("Margin").Find("MagicSpell2").gameObject;
		Player player = GameObject.Find ("BattleCode").GetComponent<MainBattle> ().player;
		updateAttacksPanelHelper (UI1, player.Special1, player.Magic);
		updateAttacksPanelHelper (UI2, player.Special2, player.Magic);
	}

	/// <summary>
	/// A function called by <see cref="updateAttacksPanel"/> that updates the display for a passed in special move
	/// </summary>
	/// <param name="UI">The containing object for the special move UI description</param>
	/// <param name="specialMove">The special move to provide details for</param>
	/// <param name="magic">The amount of magic the player has left, so that the button can be disabled
	/// if there isn't enough magic to use that move</param>
	private void updateAttacksPanelHelper(GameObject UI, SpecialMove specialMove, int magic) {
		UI.transform.Find ("Magic").GetComponent<Text> ().text = "Magic: " + specialMove.Magic.ToString();
		UI.transform.Find ("Desc").GetComponent<Text> ().text = specialMove.Desc;
		if (magic < specialMove.Magic) {
			UI.transform.Find ("Button").GetComponent<Button> ().interactable = false;
		} else {
			UI.transform.Find ("Button").GetComponent<Button> ().interactable = true;
		}
	}
}
