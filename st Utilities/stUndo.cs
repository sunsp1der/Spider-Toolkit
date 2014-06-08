using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class stUndo {

	// objects that can undo need to call RegisterUndo and have method PerformUndo (UndoEntry undoInfo, bool isUndo) isUndo=false means Redo.
	// RegisterUndo (object callingObject, object undoData, object redoData, string undoDesc="") 
	// 		undoDesc is just human readable reporting if desired
	//		PerformUndo will be called on object

	static int undoListSize = 1;
	static int undoIndex = 0; // points to next location to place entry
	static public List<UndoEntry> undoList = new List<UndoEntry>();

	static public bool hasUndo {
		get {
			return (undoIndex > 0);
		}
	}

	static public bool hasRedo {
		get {
			return (undoList.Count > undoIndex);
		}
	}

	public class UndoEntry {
		public object callingObject;
		public object undoData;
		public object redoData;
		public string undoDesc;
		public UndoEntry( object callingObject, object undoData, object redoData = null, string undoDesc="") {
			this.callingObject = callingObject;
			this.undoData = undoData;
			this.redoData = redoData;
			this.undoDesc = undoDesc;
		}
	}

	/// <summary>
	/// Registers the undo.
	/// </summary>
	/// <param name="callingObject">Calling object. This is the object that will receive the PerformUndo callback</param>
	/// <param name="undoData">Undo data. Whatever you need to undo the change</param>
	/// <param name="redoData">Redo data. Whatever you need to redo if undone. Can be set in PerformUndo also.</param>
	/// <param name="undoDesc">Undo desc. Text explaining operation... for use with visual undo lists</param>
	public static void RegisterUndo(object callingObject, object undoData, object redoData = null, string undoDesc="")
	{
		undoList.Insert (undoIndex, new UndoEntry(callingObject, undoData, redoData, undoDesc));
		undoIndex+=1;
		undoList.RemoveRange(undoIndex, undoList.Count-undoIndex);
		if (undoList.Count > undoListSize) {
			undoList.RemoveAt(0);
			undoIndex-=1;
		}
	}

	/// <summary>
	/// Performs the undo.
	/// </summary>
	/// <param name="isUndo">If set to <c>true</c>, undo. Otherwise, redo</param>
	public static void PerformUndo(bool isUndo = true){
		UndoEntry entry;
		if (isUndo) {
			if (!hasUndo) return;
			entry = undoList[undoIndex-1];
			undoIndex -= 1;
		}
		else {
			if (!hasRedo) return;
			entry = undoList[undoIndex];
			undoIndex += 1;
		}
		object[] args = {entry, isUndo};
		Introspector.GetMethod(entry.callingObject, "PerformUndo").Invoke(entry.callingObject, args);
	}

	public static void ClearUndoList(){
		undoIndex = 0;
		undoList.Clear ();
	}
	
}
