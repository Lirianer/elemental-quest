using System;
using UnityEngine;

public class CAudioComponent
{
	private GameObject mGameObject;
	private CGameObject mOwner;

	public GameObject gameObject
	{
		get { return mGameObject; }
		set { mGameObject = value; }
	}

	public CGameObject owner
	{
		get{return mOwner;}
		set{mOwner = value;}
	}

	public CAudioComponent(CGameObject aGameObject)
	{
		owner = aGameObject;
		gameObject = new GameObject();

		// Debug.
        /*
		SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("Sprites/debug/frame");
		renderer.color = Color.red;
        */
	}

	public virtual void update()
	{
		if (owner != null) 
		{
			CVector pos = owner.getPos ();
			//if (pos != null)      // TODO: ESTO LO SAQUE CUANDO SE CAMBIO CVECTOR PARA QUE SEA STRUCT
				gameObject.transform.position = new Vector3 (pos.x, -pos.y, pos.z);
		}
	}

	public virtual void destroy()
	{
		
	}
}

