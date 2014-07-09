using UnityEngine;
using System.Collections;

/// <summary>
/// Earthquake effect script
/// </summary>
public class CameraShake : MonoBehaviour
{

    #region Class fields

    GameObject thisCamera;
    private float shakeDuration = 0;
    public float shakeAmount = 1f;
    public float decreaseFactor = 1.0f;
    private Vector3 cameraStartingPosition;

    #endregion

    #region Start and update

    void Start () {
        thisCamera = this.gameObject;
        cameraStartingPosition = thisCamera.transform.position;
	}

	/// <summary>
	/// If shake duration is greater than 0, shake the object
	/// </summary>
	void Update () {

        if (shakeDuration > 0)
        {
            // sorry for the magic number
            thisCamera.transform.localPosition = cameraStartingPosition + (Random.insideUnitSphere/5) * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if(shakeDuration < 0)
        {
            shakeDuration = 0;
            // return the camera to the starting positions
            thisCamera.transform.localPosition = cameraStartingPosition;
        }
	}

    #endregion

    /// <summary>
    /// Shake the object
    /// </summary>
    /// <param name="duration"> Shake duration </param>
    public void Shake(float duration)
    {
        shakeDuration = duration;
    }

}
