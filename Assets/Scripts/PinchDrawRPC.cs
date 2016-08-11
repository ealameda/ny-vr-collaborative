using UnityEngine;
using System.Collections.Generic;
using Leap.Unity.DetectionExamples;

public class PinchDrawRPC : MonoBehaviour 
{
	private List<DrawStateRPC> drawStateRPCs;

	[PunRPC]
	void MakeDrawStateRPCs(int numDrawStates)
	{
		Debug.Log("Inside make drawstateprcs");
		PinchDraw pd = gameObject.GetComponent<PinchDraw>();
		drawStateRPCs = new List<DrawStateRPC>();
		for (int i=0;i<numDrawStates;i++)
		{
			DrawStateRPC drawStateRPC = new DrawStateRPC(pd._material, pd._minSegmentLength, pd._drawResolution, pd.DrawColor, pd.DrawRadius, pd._smoothingDelay);
			drawStateRPCs.Add(drawStateRPC);
			Debug.Log("Draw State RPC added");
		}
		Debug.Log("Num of DSRPC:" + drawStateRPCs.Count);
	}

	[PunRPC]
	void BeginDraw(int index)
	{
		drawStateRPCs[index].BeginNewLine();
	}
	[PunRPC]
	void UpdateLine(Vector3 pos, int index)
	{
		drawStateRPCs[index].UpdateLine(pos);
	}

	[PunRPC]
	void FinishLine(int index)
	{
		drawStateRPCs[index].FinishLine();
	}
}