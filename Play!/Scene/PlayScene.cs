using OpenTK;
using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathFinding;
using TiledPlugin;

namespace Play_
{
    class PlayScene : Scene
    {
        protected GameObject bg;
        protected List<Player> players;
        protected int alivePlayers;

        public List<Player> Players { get { return players; } }

        private PowerUpMgr pwrUpMgr;
        public Enemy Enemy;
        private List<TileObj> tiles;

        public PlayScene()
        {
        }

        public override void Start()
        {
            LoadAssets();

                    //CameraMgr.Init();
                    //CameraMgr.CameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.8f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, 0);

                     //CameraMgr.AddCamera("GUI", new Camera());
                    //CameraMgr.AddCamera("Sky", cameraSpeed: 0.02f);
                    //CameraMgr.AddCamera("Bg_0", cameraSpeed: 0.15f);
                    //CameraMgr.AddCamera("Bg_1", cameraSpeed: 0.2f);
                    //CameraMgr.AddCamera("Bg_2", cameraSpeed: 0.9f);

            //bg = new GameObject("bg", DrawLayer.Background);
            //bg.Position = Game.ScreenCenter;
            //bg.IsActive = true;

            //FontMgr.Init();
            //Font stdFont = FontMgr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            //Font comic = FontMgr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);

            //BulletsMgr.Init();
                    //SpawnMgr.Init();

            //players = new List<Player>();            

            //Player player = new Player(Game.GetController(0), 0);
            //player.Position = new Vector2(4, 4);
            //player.IsActive = true;

            //players.Add(player);

            //Controller controller = Game.GetController(1);

            //if (controller is KeyboardController)
            //{
            //    List<KeyCode> keys = new List<KeyCode>();
            //    keys.Add(KeyCode.Up);
            //    keys.Add(KeyCode.Down);
            //    keys.Add(KeyCode.Right);
            //    keys.Add(KeyCode.Left);
            //    keys.Add(KeyCode.CtrlRight);

            //    KeysList keyList = new KeysList(keys);
            //    controller = new KeyboardController(1, keyList);
            //}

            //player = new Player(controller, 1);
            //player.Position = new Vector2(4, 2);
            //player.IsActive = true;



            

            //players.Add(player);

            //alivePlayers = players.Count;


            //BulletsMgr.Init();

            /*
            Enemy = new Enemy("enemy_0");
            Enemy.Position = new Vector2(10, 4);
            Enemy.IsActive = true;
            */
            //Tile t1 = new Tile();
            //t1.Position = new Vector2(10, 7);

            //Tile t2 = new Tile();
            //t2.Position = t1.Position;
            //t2.X += t1.HalfWidth * 2;

            //PowerUpMgr.Init();

            TmxReader reader = new TmxReader("Assets/Tiled/Play_map.tmx");
            TmxTileset tileset = reader.TileSet;
            TmxGrid tmxGrid = reader.TileLayers[0].Grid;

            /*
            int[,] grid = new int[,]
            {
                { 200, 200, 200, 200, 200, 200, 200, 200, 200, 200 },
                { 200,   1,   1,   1,   1,   1,   1, 200,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1, 200, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200,   1,   1,   1,   1,   1,   1,   1,   1, 200 },
                { 200, 200, 200, 200, 200, 200, 200, 200, 200, 200 },
            };
            */

            int[,] grid = new int[tmxGrid.Rows, tmxGrid.Cols];
            for(int row=0; row < tmxGrid.Rows; row++) {
                for (int col = 0; col < tmxGrid.Cols; col++) {
                    int index = row * tmxGrid.Cols + col;
                    TmxCell cell = tmxGrid.At(index);

                    /*
                    if (cell == null)
                    {
                        grid[row, col] = 1;
                    } else if (cell.Type.Props.Has("cost"))
                    {
                        grid[row, col] = cell.Type.Props.GetInt("cost");
                    }
                    */
                    int cost = 1;
                    if (cell != null && cell.Type.Props.Has("cost")) {
                        cost = cell.Type.Props.GetInt("cost");
                    }
                    grid[row, col] = cost;
                }
            }

            float blockUnitWidth = Game.Window.OrthoWidth / grid.GetLength(1);
            float blockUnitHeight = Game.Window.OrthoHeight / grid.GetLength(0);

            GridPathfinder pathfinder = new GridPathfinder(grid, blockUnitWidth, blockUnitHeight);
                 //PowerUpMgr.Init(pathfinder);
            //pwrUpMgr = new PowerUpMgr(pathfinder, 6);

            //Enemy = new Enemy("enemy_0", pathfinder, pwrUpMgr);
            //int cellRow = 1;
            //int cellCol = 1;
            //Enemy.Position = new Vector2(cellCol * blockUnitWidth + blockUnitWidth*0.5f, cellRow * blockUnitHeight + blockUnitHeight * 0.5f);
            //Enemy.IsActive = true;




            // Map Rendering
            GfxMgr.AddTexture("tileset", "Assets/Tiled/" + tileset.TilesetPath);
            tiles = new List<TileObj>();

            int size = tmxGrid.Rows * tmxGrid.Cols;
            for (int index = 0; index < size; index++)
            {
                //if (grid[row,col] == 200)
                TmxCell cell = tmxGrid.At(index);
                if (cell == null) continue;
                
                float pixelToUnitW = blockUnitWidth / cell.Type.Width;
                float pixelToUnitH = blockUnitHeight / cell.Type.Height;

                float posX = cell.PosX * pixelToUnitW + blockUnitWidth/2;
                float posY = cell.PosY * pixelToUnitH + blockUnitHeight/2;
                TileObj tileObj = new TileObj("tileset",
                    cell.Type.OffX, cell.Type.OffY,
                    cell.Type.Width, cell.Type.Height,
                    posX, posY,
                    blockUnitWidth, blockUnitHeight
                );
                tiles.Add(tileObj);

                if (cell.Type.Props.Has("collidable") && cell.Type.Props.GetBool("collidable"))
                {
                    tileObj.RigidBody = new RigidBody(tileObj);
                    tileObj.RigidBody.Type = RigidBodyType.Tile;
                    tileObj.RigidBody.Collider = ColliderFactory.CreateBoxFor(tileObj);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Player);
                    tileObj.RigidBody.AddCollisionType(RigidBodyType.Enemy);
                }
            }
            base.Start();
        }

        private static void LoadAssets()
        {
            //GfxMgr.AddTexture("bg", "Assets/hex_grid_green.png");

            //GfxMgr.AddTexture("player", "Assets/player_1.png");

            //GfxMgr.AddTexture("enemy_0", "Assets/enemy_0.png");
            //GfxMgr.AddTexture("enemy_1", "Assets/enemy_1.png");

            //GfxMgr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            //GfxMgr.AddTexture("blueBar", "Assets/loadingBar_bar.png");

            //GfxMgr.AddTexture("bullet", "Assets/fireball.png");
            
            //GfxMgr.AddTexture("powerUp", "Assets/heart.png");
        }

        public override void Update()
        {
            PhysicsMgr.Update();
            UpdateMgr.Update();
            //pwrUpMgr.Update();
            

            //check collisions
            PhysicsMgr.CheckCollisions();

            //CameraMgr.Update();
        }

        public virtual void OnPlayerDies()
        {
            alivePlayers--;
            if (alivePlayers <= 0)
            {
                IsPlaying = false;
            }
        }

        public override void Draw()
        {
            DrawMgr.Draw();
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Input();
            }
        }

        public override Scene OnExit()
        {
            players.Clear();
            players = null;

            BulletsMgr.ClearAll();
            //SpawnMgr.ClearAll();
            UpdateMgr.ClearAll();
            DrawMgr.ClearAll();
            PhysicsMgr.ClearAll();
            GfxMgr.ClearAll();
            FontMgr.ClearAll();
            //CameraMgr.ResetCamera();

            return base.OnExit();
        }
    }
}
