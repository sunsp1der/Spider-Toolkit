using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stUndo {

	// objects that can undo need to call RegisterUndo and have a PerformUndo method.
	// RegisterUndo (object callingObject, object undoData, string undoInfo="") 
	// 		undoInfo is just human readable reporting if desired
	//		PerformUndo will be called on object

	class UndoEntry {
		object callingObject;
		object undoData;
		string undoDescription;
	}
	
}
