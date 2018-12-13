using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour
{
	static private CGame mInstance;
	private CGameState mState;
	private CCamera mCamera;

	private CAndy mPlayer;
	private CTileMap mMap;

	// Punto de entrada del programa.
	void Awake() 
	{
		if (mInstance != null)
		{
			throw new UnityException ("Error in CGame(). You are not allowed to instantiate it more than once.");
		}

		mInstance = this;

		CMouse.init();
		CKeyboard.init ();
		mCamera = CCamera.inst();

		//setState (new CPlatformGameState ());
		//setState(new CLevelState ());
		setState(new CMainMenuState ());
	}

	static public CGame inst()
	{
		return mInstance;
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	// Update de Unity.
	void Update () 
	{
		//update ();
	}

	void FixedUpdate()
	{
		update ();
	}

	// Se llama despues de Update().
	// https://docs.unity3d.com/Manual/ExecutionOrder.html
	void LateUpdate()
	{
		render ();
	}

	private void update()
	{
		CMouse.update ();
		CKeyboard.update ();
		mState.update ();
		mCamera.update();
	}

	private void render()
	{
		mState.render ();
		mCamera.render();
	}

	public void destroy()
	{
		CMouse.destroy ();
		CKeyboard.destroy ();
		if (mState != null) 
		{
			mState.destroy ();
			mState = null;
		}
		mInstance = null;
	}

	public void setState(CGameState aState)
	{
		if (mState != null) 
		{
			mState.destroy();
			mState = null;
		}

		mState = aState;
		mState.init ();
	}

	public CGameState getState()
	{
		return mState;
	}

	public void setPlayer(CAndy aPlayer)
	{
		mPlayer = aPlayer;
	}

	public CAndy getPlayer()
	{
		return mPlayer;
	}

	public void setMap(CTileMap aMap)
	{
		mMap = aMap;
	}

	public CTileMap getMap()
	{
		return mMap;
	}
}
