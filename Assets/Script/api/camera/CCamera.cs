using UnityEngine;
using UnityEngine.UI;

// The camera is positioned in the center. The (x,y) position of the game object is the center.
public class CCamera : CGameObject
{
	// Camera's world size. The camera will be constrained to this area.
	private float mWorldWidth = 32 * 8;
	private float mWorldHeight = 32 * 8;

	private static CCamera mInstance = new CCamera();

	// Caching of Camera.main.transform.
	private Transform mTransform;

	// This value represents zoom level. It corresponds to the "Size" property of the MainCamera (the camera in the scene).
	// In pixel perfect mode, it is the half of the height of the real height of the screen.
	private float mSize = 540;

	// Size of the window in pixels. The real resolution of the screen (real number of pixels).
	private float mScreenWidth;
	private float mScreenHeight;

	// Aspect ratio of the window.
	private float mAspectRatio;

	// Minimum and maximum values of the size of the camera.
	// By default min size = 2 tiles and max size = 20 tiles (32x32 tiles).
	private float mMinSize = 32 * 2.0f; // Normalized zoom = 1.0. Closer.
	private float mMaxSize = 32 * 20.0f; // Normalized zoom = 0.0. Further. 

	private CCamera()
	{
		mTransform = Camera.main.transform;

		initCamera();
	}

	public static CCamera inst()
	{
		return mInstance;
	}

	private void initCamera()
	{
		// Save the width and height of the screen. Set the real number of pixels of the screen.
		mScreenWidth = Screen.width;
		mScreenHeight = Screen.height;

		mAspectRatio = (float)Screen.width / (float)Screen.height;

		setSize(mSize);
	}

	public void setSize(float aSize)
	{
		mSize = CMath.clamp(mSize, mMinSize, mMaxSize);
		Camera.main.orthographicSize = mSize;
	}

	public float getSize()
	{
		return mSize;
	}

	/// ------------------------------------------------------------------------------------------------------------------    
	/// <summary>
	/// Looks at the position given by the vector position.
	/// Set the position vector of the camera: (x,y,z) coordinates. 
	/// </summary>
	/// <param name="aPos">The position vector of the point to look at.</param>
	/// ------------------------------------------------------------------------------------------------------------------
	public void lookAt(CVector aPos)
	{
		setPos(aPos);
	}

	public void lookAt(CGameObject aGameObject, bool useCenter = true)
	{
		if(useCenter)
		{
			lookAt(aGameObject.getCenter());
		}
		else 
		{
			lookAt(aGameObject.getPos());
		}
	}

	public override void update()
	{
	}

	override public void render()
	{
		// Used to keep camera bounded to integers.
		int x = Mathf.RoundToInt(getX());
		int y = Mathf.RoundToInt(getY());

		mTransform.position = new Vector3(x, -y, -10.0f);
		//mTransform.rotation = Quaternion.Euler(0, 0, mRotation);
	}

	public void setMinSize(float aMinSize)
	{
		mMinSize = aMinSize;
		setSize(CMath.clamp(mSize, mMinSize, mMaxSize));
	}

	// Returns the min possible size of the camera.
	public float getMinSize()
	{
		return mMinSize;
	}

	public void setMaxSize(float aMaxSize)
	{
		mMaxSize = aMaxSize;
		setSize(CMath.clamp(mSize, mMinSize, mMaxSize));
	}

	// Returns the max possible size of the camera.
	public float getMaxSize()
	{
		return mMaxSize;
	}
}