using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using TiledSharp;


public class CTileMap  
{
	public static CTileMap Instance {get;  private set;}


    // Number of columns.
    private int mMapWidth = 8;
    // Number of rows.
    private int mMapHeight = 8;

    private int mTileWidth = 17;
    private int mTileHeight = 13;
    // Cantidad de columnas.
    public const int MAP_WIDTH = 17;
	// Cantidad de filas.
	public const int MAP_HEIGHT = 13;

    private int mWorldWidth = 64 * 2;
    private int mWorldHeight = 64 * 2;

    public CAndy mAndy;

    // Matrix of the map. Each element is a CTile.
    private List<List<CTile>> mMap;

    // Nomber of different tiles.
    private int mNumTiles = 2;



    // La imagen es de 48x48 pixeles mide cada tile.
    public const int TILE_WIDTH = 64*2;
	public const int TILE_HEIGHT = 64*2;

	// Ancho y alto del nivel en pixeles.
	public const int WORLD_WIDTH = MAP_WIDTH * TILE_WIDTH;
	public const int WORLD_HEIGHT = MAP_HEIGHT * TILE_HEIGHT;

 

	// Cantidad de tiles que hay.
	private const int NUM_TILES = 6;

	// Array con los sprites de los tiles.
	private Sprite[] mTiles;
     
    // La pantalla tiene 17 columnas x 13 filas de tiles.
    // Mapa con el indice de cada tile.
    public static int[] LEVEL_001 = {
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
       1, 1, 1, 1, 1, 1, 1, 1,1,1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
	};

	public static int[] LEVEL_002 = 
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
	};


	private int mCurrentLevel;

	// Tile auxiliar, caminable, que se retorna cuando accedemos afuera del mapa.
	private CTile mEmptyTile;

    private int mGameID;
    public const int GAME_ANDY = 0;
    public const int GAME_RPG = 1;



    public CTileMap(int aGameID)
	{

        if (aGameID == GAME_ANDY)
        {
            setMapWidth(17);
            setMapHeight(13);
            setTileWidth(48 * 2);
            setTileHeight(48 * 2);

            mNumTiles = 6;

            //Los tiles arrancar de 0 a N 
            mTiles = new Sprite [mNumTiles];
		mTiles [(int) CTile.Type.AIR] = Resources.Load<Sprite> ("Sprites/tiles/tile000"); // TILE 000
		mTiles [(int) CTile.Type.STONE] = Resources.Load<Sprite> ("Sprites/tiles/Piedra"); // TILE 001
        mTiles [(int) CTile.Type.WATER] = Resources.Load<Sprite> ("Sprites/tiles/Agua");
		mTiles [(int) CTile.Type.EARTH] = Resources.Load<Sprite> ("Sprites/tiles/Tierra");
		mTiles [(int) CTile.Type.ICE] = Resources.Load<Sprite> ("Sprites/tiles/Hielo");
		mTiles [(int) CTile.Type.SOMETHING] = Resources.Load<Sprite> ("Sprites/tiles/tile005");

		// TODO: CARGAR TODO JUNTO CON LOADALL.

		buildLevel (1);
        }


        else if (aGameID == GAME_RPG)
        {
            mNumTiles = 64;
            mTiles = Resources.LoadAll<Sprite>("Data/maps/tileset_1bit");
            loadLevelTMX("Data/maps/map1");

        }

        mEmptyTile = new CTile (0, 0, 0, mTiles [0],1);
		mEmptyTile.setVisible (false);
		mEmptyTile.setWalkable (true);

		registerSingleton ();
	}


    private void loadLevelTMX(string aFileName)
    {
        // XDocument es la representacion de XML en C#.
        XDocument doc;
        TmxMap tmxMap;

        // Nota: El archivo tiene que ser .XML (no funciona si es .TMX);
        string xml = Resources.Load<TextAsset>(aFileName).text;
        //Debug.Log (xml);

        // Convertir el texto a clase XML.
        doc = XDocument.Parse(xml);

        tmxMap = new TmxMap(doc);

        int mapWidth = tmxMap.Width;
        int mapHeight = tmxMap.Height;
        int tileWidth = tmxMap.TileWidth;
        int tileHeight = tmxMap.TileHeight;

        setMapWidth(tmxMap.Width);
        setMapHeight(tmxMap.Height);
        int scale = 2;
        setTileWidth(tmxMap.TileWidth * scale);
        setTileHeight(tmxMap.TileHeight * scale);

        mMap = new List<List<CTile>>();

        for (int y = 0; y < mapHeight; y++)
        {
            mMap.Add(new List<CTile>());

            for (int x = 0; x < mapWidth; x++)
            {
                CTile tile = new CTile((x * mTileWidth), (y * mTileHeight), 0, mTiles[0], scale);
                mMap[y].Add(tile);
            }
        }

        for (int i = 0; i < tmxMap.Layers[0].Tiles.Count; i++)
        {
            int y = i / mapWidth;
            int x = i % mapHeight;

            // TODO: PONER UNA SOLA LINEA
            int index = tmxMap.Layers[0].Tiles[i].Gid;         // 1 a 64

            getTile(x, y).setTileIndex(index);

            //getTile (x, y).setWalkable (mWalkable [index]); NO APLICA.
            getTile(x, y).setImage(mTiles[index - 1]);           // 0 a 63
        }

        //CCamera.inst().setWorldSize(tileWidth * mapWidth * scale, tileHeight * mapHeight * scale);
        Debug.Log(tileWidth * mapWidth * scale);
        Debug.Log(tileHeight * mapHeight * scale);


        // Info para pathfinding.
        // TODO: Poner costos en cada tile (terreno alto, pasto, pantano, etc).
        //mGrid = new CGrid(mapWidth, mapHeight);
        /*for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; x < mapHeight; y++) 
			{
				mGrid.setWalkable (x, y, getTile (x, y).isWalkable ());
			}
		}*/
    }

    private void registerSingleton()
	{
		if (Instance == null) 
		{
			Instance = this;
		}
		else 
		{
			throw new UnityException( "ERROR: Cannot create another instance of singleton class CEnemyManager.");
		}
	}

	// Construye el mapa. Crear el array y carga el mapa aLevel.
	public void buildLevel(int aLevel)
	{ 
		mCurrentLevel = aLevel;

		int[] m;
		if (aLevel == 1)
			m = LEVEL_001;
		else
			m = LEVEL_002;

		mMap = new List<List<CTile>> ();

		// Para cada fila..
		for (int y = 0; y < mMapHeight; y++) 
		{
			// Crea un array para la fila vacio.
			mMap.Add (new List<CTile> ());			

			// Llenar la fila.
			for (int x = 0; x < mMapWidth; x++) 
			{
				// Obtener que indice de tile es: 0, 1, ....
				int index = m[y * mMapWidth + x]; 
				// Crear el tile.
				CTile tile = new CTile(x * TILE_WIDTH, y * TILE_HEIGHT, index, mTiles[index],2);
				// Agregar el tile a la fila.
				mMap [y].Add (tile);
			}
		}

		loadEnemies (aLevel);
	}

	// Carga el mapa en el array ya creado. No crea el array.
	public void loadLevel(int aLevel)
	{ 
		mCurrentLevel = aLevel;

		int[] m;
		if (aLevel == 1)
			m = LEVEL_001;
		else
			m = LEVEL_002;

		// Para cada fila..
		for (int y = 0; y < mMapHeight; y++) 
		{
			// Llenar la fila.
			for (int x = 0; x < mMapWidth; x++) 
			{
				// Obtener que indice de tile es. 0, 1, ....
				int index = m [y * mMapWidth + x];
				// Cambiar el tile.
				CTile tile = getTile(x, y);
				tile.setTileIndex (index);
				tile.setImage (mTiles[index]);
			}
		}

		loadEnemies (aLevel);
	}

	private void loadEnemies(int aLevel)
	{

        ///Cargo el Niveles 
		if (aLevel == 1) 
		{
            //Pongo a los Enemigos
			CScorpion s = new CScorpion (CScorpion.TYPE_DONT_FALL);
			s.setXY (550, 300);
			CEnemyManager.inst ().add (s);

            CGolemTierra g = new CGolemTierra(CGolemTierra.TYPE_DONT_FALL);
            g.setXY(600, 300);
            CEnemyManager.inst().add(g);

            CSalamandra sal = new CSalamandra(CSalamandra.TYPE_DONT_FALL);
            sal.setXY(500, 300);
            CEnemyManager.inst().add(sal);

            CGolemRoca gr = new CGolemRoca(CGolemTierra.TYPE_DONT_FALL);
            gr.setXY(300, 300);
            CEnemyManager.inst().add(gr);

            CBird cb = new CBird(CBird.TYPE_DONT_FALL);
            cb.setXY(450, 300);
            CEnemyManager.inst().add(cb);

            CElementalAgua el = new CElementalAgua(CElementalAgua.TYPE_DONT_FALL);
            el.setXY(150, 300);
            CEnemyManager.inst().add(el);


            CItem item = new CItem (CItem.TYPE_DONT_FALL);
			item.setXY (700, 300);
			CItemManager.inst ().add (item);

			CCoin coin = new CCoin (CCoin.TYPE_FALL);
			coin.setXY (800, 200);
			CItemManager.inst ().add (coin);
		} 
		else if (aLevel == 2) 
		{
			CScorpion s = new CScorpion (CScorpion.TYPE_FALL);
			s.setXY (700, 300);
			CEnemyManager.inst ().add (s);

			CItem item = new CItem (CItem.TYPE_DONT_FALL);
			item.setXY (400, 300);
			CItemManager.inst ().add (item);

			CCoin coin = new CCoin (CCoin.TYPE_FALL);
			coin.setXY (550, 200);
			CItemManager.inst ().add (coin);
		}
	}


	// aDIr es por donde nos fuimos: CGameConstants.UP,...
	public void changeRoom(int aDir)
	{
		CEnemyManager.inst ().clean ();

		if (mCurrentLevel == 1) 
		{
			if (aDir == CGameConstants.D) 
			{
				loadLevel (2);
			}
		} 
		else if (mCurrentLevel == 2) 
		{
			if (aDir == CGameConstants.A) 
			{
				loadLevel (1);
			}
		}
	}

	public void update()
    {
		for (int y = 0; y < mMapHeight; y++) 
		{
			for (int x = 0; x < mMapHeight; x++) 
			{
				mMap [y] [x].update ();
			}
		}
             
    }

	public void render()
	{
		for (int y = 0; y < mMapHeight; y++) 
		{
			for (int x = 0; x < mMapWidth; x++) 
			{
				mMap [y] [x].render ();
			}
		}
	}

	public void destroy()
	{
		for (int y = mMapHeight - 1; y >= 0; y--) 
		{
			for (int x = mMapWidth - 1; x >= 0; x--) 
			{
				mMap [y] [x].destroy ();
				mMap [y] [x] = null;
			}
			mMap.RemoveAt (y);
		}

		mMap = null;
	}

	// Parametros: aX es la columna. aY es la fila.
	public int getTileIndex(int aX, int aY)
	{
		if (aX < 0 || aX >= mMapWidth || aY < 0 || aY >= mMapWidth) 
		{
			return 0;
		} 
		else 
		{
			return mMap [aY] [aX].getTileIndex ();
		}
	}

	public CTile getTile(int aX, int aY)
	{
		if (aX < 0 || aX >= mMapWidth || aY < 0 || aY >= mMapHeight) 
		{
			// Si accedo fuera del mapa retorna el empty tile que es caminable.
			return mEmptyTile;
		} 
		else 
		{
			return mMap [aY] [aX];
		}
	}

	public void changeTile(CTile tile, CTile.Type type)
	{
		tile.setTileType(type);
		tile.setImage(mTiles[(int)type]);
	}

    public int getMapWidth()
    {
        return mMapWidth;
    }

    // Return the map height in tiles (number of rows of the map).
    public int getMapHeight()
    {
        return mMapHeight;
    }

    public int getTileWidth()
    {
        return mTileWidth;
    }

    public int getTileHeight()
    {
        return mTileHeight;
    }

    public int getWorldWidth()
    {
        return mWorldWidth;
    }

    public int getWorldHeight()
    {
        return mWorldHeight;
    }

    // Calculate the world size. Called each time the size of the map or the tile changes.
    // There is no setters for mWorldWidth and mWorldHeight. There are calculated automatically.
    private void calculateWorldSize()
    {
        mWorldWidth = mMapWidth * mTileWidth;
        mWorldHeight = mMapHeight * mTileHeight;
    }

    // Set the map width in tiles (number of columns of the map).
    public void setMapWidth(int aMapWidth)
    {
        mMapWidth = aMapWidth;
        calculateWorldSize();
    }

    // Set the map height in tiles (number of rows of the map).
    public void setMapHeight(int aMapHeight)
    {
        mMapHeight = aMapHeight;
        calculateWorldSize();
    }

    // Set the tile width in pixels.
    public void setTileWidth(int aTileWidth)
    {
        mTileWidth = aTileWidth;
        calculateWorldSize();
    }

    // Set the tile height in pixels.
    public void setTileHeight(int aTileHeight)
    {
        mTileHeight = aTileHeight;
        calculateWorldSize();
    }


}
