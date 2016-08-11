using UnityEngine;
using System.Collections;

public class BlinkingEyes : MonoBehaviour 
{
	public GameObject leftEye;
	public GameObject rightEye;
	public float blinkFrequency;
	public float blinkSpeed;
	private Vector3 targetClosedScale;
	private Vector3 targetOpenScale;
	private float timer = 0.0f;
	private bool blinking = false;
	private bool opening = false;
	
	void Start()
	{
		targetClosedScale = new Vector3(leftEye.transform.localScale.x, 0.0f, leftEye.transform.localScale.z);
		targetOpenScale = leftEye.transform.localScale;
	}

	void Update () 
	{
		timer += Time.deltaTime;
		//leftEye.transform.scale = new Vector3(1,0,1);
		if(blinking)
		{
			if(leftEye.transform.localScale.y <= 0.2f)
			{
				//stop blinking & start opening
				opening = true;
				blinking = false;
			}
			else
			{
				leftEye.transform.localScale = targetClosedScale;
				rightEye.transform.localScale = targetClosedScale;
			}
		}
		else if (opening && (timer+UnityEngine.Random.Range(-.25f, .25f)>blinkSpeed))
		{
			if(leftEye.transform.localScale.y >= targetOpenScale.y)
			{
				opening = false;
			}
			else
            { 
				leftEye.transform.localScale = targetOpenScale;
				rightEye.transform.localScale = targetOpenScale;
			}
		}
		else if (timer+UnityEngine.Random.Range(-1, 1) >blinkFrequency)
		{
			blinking = true;
			timer = 0.0f;
		}
	}
}
