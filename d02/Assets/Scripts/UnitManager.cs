using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	private List<Unit> _units = new List<Unit>();

	public Camera ViewCamera;
	// Update is called once per frame
	private void Update ()
	{
		if (Input.GetMouseButtonDown(1))
		{
			var mousePosition = ViewCamera.ScreenToWorldPoint(Input.mousePosition);
			foreach (var unit in _units)
			{
				if(!unit)
					continue;
				if (!unit.IsSelected())
					continue;
				unit.ChangeEndpoint(new Vector3(mousePosition.x, mousePosition.y, 0));
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			var areSelected = FindSelected();
			var wasSelected = false;
			foreach (var unit in _units)
			{
				if(!unit)
					continue;
				if (!unit.CanSelect())
					continue;
				if ((!areSelected || Input.GetKey(KeyCode.LeftControl)) && !unit.IsSelected())
					unit.SetSelection(true);
				else if (areSelected && unit.IsSelected() && Input.GetKey(KeyCode.LeftControl))
					unit.SetSelection(false);
				else if (areSelected)
				{
					ClearAllSelection();
					unit.SetSelection(true);
				}
				wasSelected = true;
			}
			if(!wasSelected)
				ClearAllSelection();	
		}
	}

	private bool FindSelected()
	{
		foreach (var unit in _units)
		{
			if(!unit)
				continue;
			if (unit.IsSelected())
				return true;
		}
		return false;
	}

	public void ClearAllSelection()
	{
		foreach (var unit in _units)
		{
			if(!unit)
				continue;
			unit.SetSelection(false);
		}
	}

	public void AddUnit(Unit newUnit)
	{
		_units.Add(newUnit);
	}
}
