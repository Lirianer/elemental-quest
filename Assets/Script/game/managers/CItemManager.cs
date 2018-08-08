using UnityEngine;
using System.Collections;

public class CItemManager : CManager
{
	private static CItemManager mInst = null;

	public CItemManager()
	{
		registerSingleton ();
	}

	public static CItemManager inst()
	{
		return mInst;
	}

	private void registerSingleton()
	{
		if (mInst == null) 
		{
			mInst = this;
		}
		else 
		{
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CItemManager.");
		}
	}

	override public void update()
	{
		base.update ();
	}

	override public void render()
	{
		base.render ();
	}

	override public void destroy()
	{
		base.destroy ();
		mInst = null;
	}


}