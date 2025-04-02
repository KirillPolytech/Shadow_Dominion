using UnityEngine;

public class TiltWindow : MonoBehaviour
{
	private const float Half = 0.5f;
	
	[SerializeField] private Vector2 range = new(5f, 3f);
	[SerializeField] private float force = 5f;

	private Transform _mTrans;
	private Quaternion _mStart;
	private Vector2 _mRot;

	private void Start ()
	{
		_mTrans = transform;
		_mStart = _mTrans.localRotation;
	}

	private void Update ()
	{
		Vector3 pos = Input.mousePosition;

		float halfWidth = Screen.width * Half;
		float halfHeight = Screen.height * Half;
		float x = Mathf.Clamp((pos.x - halfWidth) / halfWidth, -1f, 1f);
		float y = Mathf.Clamp((pos.y - halfHeight) / halfHeight, -1f, 1f);
		_mRot = Vector2.Lerp(_mRot, new Vector2(x, y), Time.deltaTime * force);

		_mTrans.localRotation = _mStart * Quaternion.Euler(-_mRot.y * range.y, _mRot.x * range.x, 0f);
	}
}
