using UnityEngine;
using System.Collections;

public class CEnemyManager : CManager
{
	private static CEnemyManager mInst = null;
	
	public CEnemyManager()
	{
		registerSingleton ();
	}
	
	public static CEnemyManager inst()
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
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CEnemyManager.");
		}
	}
	
	override public void update()
	{
		base.update ();

		CSprite enemy = (CSprite) this.collidesRect(CGame.inst().getPlayer());

		if(enemy != null && enemy.isVisible())
		{
			CGame.inst().getPlayer().die();
		}
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

	public void spawnEnemy(float x, float y, int type = 0)
	{
		Debug.Log(x + " - " + y);
		CEnemy enemy = new CEnemy();

		type = type == 0 ? CMath.randomIntBetween(1, 3) : type;

		switch (type) {
			case 1:
				enemy = (CEnemy) new CSalamandra(CSalamandra.TYPE_DONT_FALL);
				break;
			case 2:
				enemy = (CEnemy) new CGolemTierra(CGolemTierra.TYPE_DONT_FALL);
				break;
			case 3:
				enemy = (CEnemy) new CElementalAgua(CElementalAgua.TYPE_DONT_FALL);
				break;
			case 4:
				enemy = (CEnemy) new CBird(CBird.TYPE_DONT_FALL);
				break;
		}
		enemy.setState(CEnemy.STATE_FALLING);
		enemy.setXY(x - enemy.getWidth() / 2, y + enemy.getHeight() / 2);
		this.add(enemy);
	}
}