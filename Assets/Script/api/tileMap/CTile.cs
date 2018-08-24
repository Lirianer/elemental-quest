using UnityEngine;
using System.Collections;

public class CTile : CSprite
{
	public enum Type : int {
		AIR,
		STONE,
		WATER,
		EARTH,
		ICE,
		SOMETHING
	}

	public enum Direction : int {
		TOP,
		LEFT,
		RIGHT,
		BOTTOM
	}

	// Tile index. Starting from 0. 0, 1, 2...
	private int mTileIndex;

	// True = se puede caminar. False = no se puede traspasar.
	private bool mIsWalkable;

	// Parametros: coordenada del tile (x, y) y el indice del tile.
	public CTile(int aX, int aY, int aTileIndex, Sprite aSprite)
	{
		setXY (aX, aY);
		setTileIndex(aTileIndex);

		setImage (aSprite);
		setSortingLayerName ("TileMap");
		setName ("tile");

		setScale (2.0f);

		hideInUnityHierarchy ();
	}

	public void setTileIndex(int aTileIndex)
	{
		mTileIndex = aTileIndex;

		// Set walkable information.
		if (aTileIndex == 1)
			mIsWalkable = false;
        else
            mIsWalkable = true;
        //Tile de tierra 
        if (aTileIndex == 3)
            mIsWalkable = false;
        //Tile de Hielo
        if (aTileIndex == 4)
            mIsWalkable = false;

    }

	public void setTileType(Type type)
	{
		this.setTileIndex((int) type);
	}

	public int getTileIndex()
	{
		return mTileIndex;
	}

	public Type getTileType()
	{
		return (Type)this.getTileIndex();
	}

	override public void render()
	{
		base.render ();
	}

	override public void update()
	{
		base.update ();
	}

	override public void destroy()
	{
		base.destroy ();
	}

	// Invocada desde CTileMap cuando se clickea el mouse en este tile.
	public void clicked()
	{
		//setColor(Color.red);
	}

	public bool isWalkable()
	{
		return mIsWalkable;
	}

	public void setWalkable(bool aIsWalkable)
	{
		mIsWalkable = aIsWalkable;
	}

	public CTile getNeighbourTile(Direction direction)
	{
		int xDirection = 0;
		int yDirection = 0;

		switch (direction)
		{
			case Direction.TOP:
				yDirection = -1;
			break;
			case Direction.RIGHT:
				xDirection = 1;
			break;
			case Direction.BOTTOM:
				yDirection = 1;
			break;
			case Direction.LEFT:
				xDirection = -1;
			break;
		}

		return CTileMap.Instance.getTile((int)(this.getX() + CTileMap.TILE_WIDTH * xDirection) / CTileMap.TILE_WIDTH, (int)(this.getY() + CTileMap.TILE_HEIGHT * yDirection) / CTileMap.TILE_HEIGHT);
	}
}
