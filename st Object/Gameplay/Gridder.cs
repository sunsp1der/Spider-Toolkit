using UnityEngine;
using System.Collections;

[AddComponentMenu("st Object/Gameplay/Gridder")]
[RequireComponent(typeof(stObject))]

/// <summary>
/// On awake, Gridder will make a grid of clones of this object.
/// For use on archetypes only.
/// Object references will be stored in the 'grid' array.
/// All Gridder components will be removed from clones.
/// </summary>
public class Gridder : MonoBehaviour {

	[Tooltip("Location of grid center")]
	public Vector2 center;
	[Tooltip("Distance between each clone")]
	public Vector2 spacing = new Vector2(1,1);
	[Tooltip("Number of columns in grid")]
	public int columns = 2;
	[Tooltip("Number of rows in grid")]
	public int rows = 2;
	[Tooltip("Rename clones to objectname_column#_row#")]
	public bool rename = true;
	[Tooltip("If not blank, place all clones inside parent gameobject with this name")]
	public string parent = "grid";

	[HideInInspector]
	public GameObject[,] grid; // store references to clones here, just in case user needs them later
	[HideInInspector]
	public GameObject parentObject;

	void ArchetypeStart() {
		// offset is bottom-left point of grid... used to make math a bit simpler later
		Vector3 offset = new Vector3( center.x - (columns-1) * spacing.x * 0.5f,
		                              center.y - (rows-1) * spacing.y * 0.5f,
		                              gameObject.transform.position.z);
		// parent
		if (parent != "") {
			parentObject = new GameObject();
			parentObject.name = parent;
			parentObject.transform.position = new Vector3( center.x, center.y, gameObject.transform.position.z);
		}
		grid = new GameObject[columns, rows];
		// deactivate 
		for (int c=0; c<columns; c++) {
			for (int r=0; r<rows; r++) {
				// create grid node
				GameObject ob = stTools.Spawn (gameObject);
				// store in grid
				grid[c,r] = ob;
				// parent
				ob.transform.parent = parentObject.transform;
				// position
				ob.transform.position = new Vector3( offset.x + c * spacing.x, offset.y + r * spacing.y, offset.z);
				// rename
				if (rename) {
					ob.name = gameObject.name + "_" + c.ToString() + "_" + r.ToString();
				}
				// remove Gridder component for clones. 
				Gridder[] gridderComponents = ob.GetComponents<Gridder>();
				foreach (Gridder gridder in gridderComponents) {
					Destroy(gridder);
				}
			}
		}
	}
}
