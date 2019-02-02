using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class dragTowersScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private GameObject _itemBeingDragged;
	public towerScript Tower;
	public bool IsDraggable = true;
	public GameObject Frame;
	public Camera Cam;

	public void OnBeginDrag(PointerEventData eventData) {
		if (IsDraggable)
		{
			var oldScale = transform.localScale;
			transform.localScale = new Vector3(2f,1.5f,1f);
			_itemBeingDragged = Instantiate(gameObject, transform);
			transform.localScale = oldScale;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (IsDraggable) {
			_itemBeingDragged.transform.position = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (IsDraggable) {
			if (_itemBeingDragged != null) {
				var hit = Physics2D.Raycast (Cam.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit && hit.collider.transform.CompareTag("empty")) {
					gameManager.gm.playerEnergy -= Tower.energy;
					Instantiate (Tower, hit.collider.gameObject.transform.position, Quaternion.identity);			
				}
				Destroy (_itemBeingDragged);
				_itemBeingDragged = null;
			}
		}
	}

	private void Start()
	{
		var dmg = Frame.transform.Find("VLine/HLine/gatling_damage_text").gameObject.GetComponent<Text>();
		dmg.text = Tower.damage + "";
		var reload = Frame.transform.Find("VLine/HLine/gatling_reload_text").gameObject.GetComponent<Text>();
		reload.text = Tower.fireRate + "";
		var range = Frame.transform.Find("VLine/HLine/gatling_range_text").gameObject.GetComponent<Text>();
		range.text = Tower.range + "";
		var cost = Frame.transform.Find("VLine/HLine/gatling_energy_text").gameObject.GetComponent<Text>();
		cost.text = Tower.energy + "";
		var air = Frame.transform.Find("AntiAir").gameObject;
		air.SetActive(Tower.type == towerScript.Type.gatling || Tower.type == towerScript.Type.rocket);
	}
	

	// Update is called once per frame
	private void Update () {
		if (gameManager.gm.playerEnergy - Tower.energy < 0) {
			IsDraggable = false;
			Frame.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.3f);
		} else {
			IsDraggable = true;
			Frame.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
		}
	}

	
}
