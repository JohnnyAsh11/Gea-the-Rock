using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GeaTheRock2
{

    /// <summary>
    /// enumeration for the basic menu/game inputs
    /// </summary>
    public enum GameState
    {
        Menu,
        Game,
        GameWin,
        GameOver,
        Controls,
        SolarSystem,
        Chapters,
        GameChapters
    }

    /// <summary>
    /// enumeration for each individual level in the game
    /// </summary>
    public enum Levels
    {
        Level1,
        Level2,
        Level3,
        Level4
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region Texture / GameObjects

        //debug sprite info
        private Texture2D debugImage;
        private SpriteFont debugFont;

        //actual Texture assets
        private Texture2D playerAsset;
        private Texture2D playerCollisionAsset;
        private Texture2D healthAsset;
        private Texture2D healthCanister;

        //game Objects
        private Player player;
        private AsteroidEnemy enemy;
        private Planet planet;
        private EnemyManager enemyMgr;
        private HealthBar healthBar;

        //Menu buttons
        private Button menuControls;
        private Button menuSolarSystem;
        private Button menuChapters;
        private Button menuBack;

        //within chapter buttons
        private Button previousStage;
        private Button nextStage;

        //chapters page buttons
        private Button chapter1;
        private Button chapter2;
        private Button chapter3;
        private Button chapter4;

        //general screen textures
        private Texture2D titleLogo;
        private Texture2D background;
        private Texture2D solarSystemPage;
        private Texture2D controlsPage;
        private Texture2D chaptersPage;

        //menu button textures
        private Texture2D chaptersMenuHover;
        private Texture2D chaptersMenu;
        private Texture2D controlsMenu;
        private Texture2D controlsMenuHover;
        private Texture2D solarSystemMenu;
        private Texture2D solarSystemMenuHover;

        //general button textures
        private Texture2D backArrow;
        private Texture2D backArrowHover;
        private Texture2D forwardArrow;
        private Texture2D forwardArrowHover;
        private Texture2D playButton;
        private Texture2D playButtonHover;

        //planet assets
        private Texture2D planet1Asset;
        private Texture2D planet2Asset;
        private Texture2D planet3Asset;
        private Texture2D planet4Asset;

        //asteroid sprite sheet
        private Texture2D asteroidAsset;

        //game win and loss screens
        private Texture2D winBackground;
        private Texture2D loseBackground;

        #endregion

        //FSM state variables
        private GameState gameState;
        private Levels level;

        private int levelCounter;
        private Dictionary<string, Texture2D> chapterStages;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            chapterStages = new Dictionary<string, Texture2D>();

            //changing the window sizing to be a little bigger
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 960;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

            //intializing game states
            gameState = GameState.Menu;
            level = Levels.Level1;
            levelCounter = 1;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region texture load-ins

            //debug assets
            debugImage = Content.Load<Texture2D>("DebugImage");            
            debugFont = Content.Load<SpriteFont>("Arial-40");

            //player assets
            playerAsset = Content.Load<Texture2D>("RockGea");
            playerCollisionAsset = Content.Load<Texture2D>("RockGeaHit");

            //GameState.Menu button assets
            chaptersMenu = Content.Load<Texture2D>("Chapters_Button");
            chaptersMenuHover = Content.Load<Texture2D>("Chapters_Button_Hover");
            controlsMenu = Content.Load<Texture2D>("Controls_Button");
            controlsMenuHover = Content.Load<Texture2D>("Controls_Button_Hover");
            solarSystemMenu = Content.Load<Texture2D>("Solar_System_Button");
            solarSystemMenuHover = Content.Load<Texture2D>("Solar_System_Button_Hover");
            backArrow = Content.Load<Texture2D>("Back_Arrow");
            backArrowHover = Content.Load<Texture2D>("Back_Arrow_Hover");

            //general texture load ins
            titleLogo = Content.Load<Texture2D>("titleCard");
            background = Content.Load<Texture2D>("Spacebackground");
            solarSystemPage = Content.Load<Texture2D>("Solar_System_Page");
            chaptersPage = Content.Load<Texture2D>("Chapters_Page_Final");
            controlsPage = Content.Load<Texture2D>("Controls_Page_Final");
            playButton = Content.Load<Texture2D>("Play_button");
            playButtonHover = Content.Load<Texture2D>("Play_Button_Hover");
            forwardArrow = Content.Load<Texture2D>("Forward_Arrow");
            forwardArrowHover = Content.Load<Texture2D>("Forward_Arrow_Hover");

            //health bar assets
            healthAsset = Content.Load<Texture2D>("EmptyHealthBar");
            healthCanister = Content.Load<Texture2D>("HealthBar");

            //planet animation assets
            planet1Asset = Content.Load<Texture2D>("Terestrial");
            planet2Asset = Content.Load<Texture2D>("SuperEarth");
            planet3Asset = Content.Load<Texture2D>("NeptuneLike");
            planet4Asset = Content.Load<Texture2D>("GasGiant");

            //asteroid spritesheet
            asteroidAsset = Content.Load<Texture2D>("Asteroid_SpriteSheet");

            //game win and lose backgrounds
            winBackground = Content.Load<Texture2D>("GameWinPage");
            loseBackground = Content.Load<Texture2D>("GameOverPage");

            #endregion

            //loading in all of the chapter stage assets
            for (int i = 1; i < 5; i++)
            {
                for (int x = 1; x < 4; x++)
                {
                    Texture2D asset = Content.Load<Texture2D>($"Chapter{i}-{x}");

                    chapterStages.Add(
                        $"chapter{i}-{x}",
                        asset);
                }
            }

            player = new Player(
                playerAsset,
                playerCollisionAsset,
                new Circle(200, 430, 50));

            //enemy that will be used as reference for EnemyManager
            enemy = new AsteroidEnemy(
                asteroidAsset,
                new Circle(1500, 800, 50));

            enemyMgr = new EnemyManager(
                enemy,
                45);

            planet = new Planet(
                planet1Asset,
                new Circle(800, 480, 150));

            healthBar = new HealthBar(
                healthAsset,
                healthCanister,
                new Rectangle(400, 860, 800, 100));

            #region buttons

            //button instantiation  (put on one line because this class does not need to be a million lines long)
            menuControls = new Button(controlsMenu, controlsMenuHover, new Rectangle(470, 370, 600, 118));
            menuChapters = new Button(chaptersMenu, chaptersMenuHover, new Rectangle(470, 510, 600, 118));
            menuSolarSystem = new Button(solarSystemMenu, solarSystemMenuHover, new Rectangle(470, 650, 600, 118));
            menuBack = new Button(backArrow, backArrowHover, new Rectangle(50, 50, 140, 81));

            //chapter select buttons
            chapter1 = new Button(playButton, playButtonHover, new Rectangle(65, 580, 215, 62));
            chapter2 = new Button(playButton, playButtonHover, new Rectangle(365, 600, 215, 62));
            chapter3 = new Button(playButton, playButtonHover, new Rectangle(730, 660, 215, 62));
            chapter4 = new Button(playButton, playButtonHover, new Rectangle(1190, 760, 215, 62));

            //moving through chapters
            nextStage = new Button(forwardArrow, forwardArrowHover, new Rectangle(1150, 710, 150, 150));
            previousStage = new Button(backArrow, backArrowHover, new Rectangle(300, 710, 150, 150));

            #endregion

            //instantiates the enemyList
            enemyMgr.Instantiate(planet, player);

            //subscribing the enemyMgr's method to the Player class
            player.CollisionDetection += enemyMgr.RetrivingHitboxes;

            //for healthbar to track the planet's life
            healthBar.PlanetHealth += planet.PlanetHealth;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kbState = Keyboard.GetState();

            #region Finite State Machine

            //Main FSM
            switch (gameState)
            {
                case GameState.Menu:

                    //updating menu buttons
                    menuControls.Update();
                    menuChapters.Update();
                    menuSolarSystem.Update();

                    //changing state based on button press
                    if (menuChapters.IsPressed())
                    {
                        gameState = GameState.Chapters;
                    }
                    else if (menuControls.IsPressed())
                    {
                        gameState = GameState.Controls;
                    }
                    else if (menuSolarSystem.IsPressed())
                    {
                        gameState = GameState.SolarSystem;
                    }
                    break;
                case GameState.Chapters:

                    //updating the button
                    menuBack.Update();
                    chapter1.Update();
                    chapter2.Update();
                    chapter3.Update();
                    chapter4.Update();

                    //checking if its pressed
                    if (menuBack.IsPressed())
                    {
                        gameState = GameState.Menu;
                    }
                    else if (chapter1.IsPressed())
                    {
                        gameState = GameState.GameChapters;
                        level = Levels.Level1;
                    }
                    else if (chapter2.IsPressed())
                    {
                        gameState = GameState.GameChapters;
                        level = Levels.Level2;
                    }
                    else if (chapter3.IsPressed())
                    {
                        gameState = GameState.GameChapters;
                        level = Levels.Level3;
                    }
                    else if (chapter4.IsPressed())
                    {
                        gameState = GameState.GameChapters;
                        level = Levels.Level4;
                    }
                    break;
                case GameState.GameChapters:

                    //level manager method
                    UpdateLevelLogic();

                    break;
                case GameState.Game:

                    //updating game Objects
                    player.Update();
                    planet.Update();
                    healthBar.Update();
                    enemyMgr.Update();

                    if (planet.Health <= 0)
                    {
                        gameState = GameState.GameOver;
                    }
                    else if (planet.Countdown == 0)
                    {
                        gameState = GameState.GameWin;
                    }

                    break;
                case GameState.GameWin:

                    //reseting gameObjects
                    Reset();

                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Menu;
                    }

                    break;
                case GameState.GameOver:

                    //reseting gameObjects
                    Reset();

                    if (kbState.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Game;
                    }

                    break;
                case GameState.Controls:

                    //updating the button
                    menuBack.Update();

                    //checking if its pressed
                    if (menuBack.IsPressed())
                    {
                        gameState = GameState.Menu;
                    }
                    break;
                case GameState.SolarSystem:

                    //updating the button
                    menuBack.Update();

                    //checking if its pressed
                    if (menuBack.IsPressed())
                    {
                        gameState = GameState.Menu;
                    }
                    break;
            }

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //always draw the background
            _spriteBatch.Draw(
                background,
                new Rectangle(0, 0, 1600, 960),
                Color.White);

            _spriteBatch.End();

            //-----------------------------------------------------------------
            if (gameState == GameState.Menu)
            {
                _spriteBatch.Begin();

                //draw the title logo
                _spriteBatch.Draw(
                    titleLogo,
                    new Rectangle(350, 50, 839, 345),
                    Color.White);
                
                _spriteBatch.End();

                //drawing buttons
                menuControls.Draw(_spriteBatch);
                menuChapters.Draw(_spriteBatch);
                menuSolarSystem.Draw(_spriteBatch);
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.Controls)
            {
                _spriteBatch.Begin();

                //always draw the background
                _spriteBatch.Draw(
                    controlsPage,
                    new Rectangle(0, 0, 1600, 960),
                    Color.White);

                _spriteBatch.End();

                //drawing the button
                menuBack.Draw(_spriteBatch);
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.Chapters)
            {
                _spriteBatch.Begin();

                //always draw the background
                _spriteBatch.Draw(
                    chaptersPage,
                    new Rectangle(0, 0, 1600, 960),
                    Color.White);

                _spriteBatch.End();

                //drawing the button
                menuBack.Draw(_spriteBatch);

                //drawing the chapter buttons
                chapter1.Draw(_spriteBatch);
                chapter2.Draw(_spriteBatch);
                chapter3.Draw(_spriteBatch);
                chapter4.Draw(_spriteBatch);

            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.SolarSystem)
            {
                _spriteBatch.Begin();

                //always draw the background
                _spriteBatch.Draw(
                    solarSystemPage,
                    new Rectangle(0, 0, 1600, 960),
                    Color.White);

                _spriteBatch.End();

                //drawing the button
                menuBack.Draw(_spriteBatch);
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.Game)
            {
                //giving the planet obj the right texture
                if (level == Levels.Level1)
                {
                    planet.Asset = planet1Asset;
                }
                else if (level == Levels.Level2)
                {
                    planet.Asset = planet2Asset;
                }
                else if (level == Levels.Level3)
                {
                    planet.Asset = planet3Asset;
                }
                else if (level == Levels.Level4)
                {
                    planet.Asset = planet4Asset;
                }

                //only drawing gameObjects if GameState.Game
                planet.Draw(_spriteBatch);
                player.Draw(_spriteBatch);                
                enemyMgr.Draw(_spriteBatch);
                healthBar.Draw(_spriteBatch);
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.GameChapters)
            {
                DrawLevelLogic();
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.GameWin)
            {
                DrawBackground(winBackground);

                //writing a message to let the player know what to press
                _spriteBatch.Begin();

                _spriteBatch.DrawString(
                    debugFont,
                    "press [ENTER] to continue",
                    new Vector2(500, 800),
                    Color.White);

                _spriteBatch.End();
            }
            //-----------------------------------------------------------------
            else if (gameState == GameState.GameOver)
            {
                DrawBackground(loseBackground);
            }



            base.Draw(gameTime);
        }

        /// <summary>
        /// the programming for the stages of each chapter/level
        /// </summary>
        public void UpdateLevelLogic()
        {
            //-----------------------------------------------------------------
            if (level == Levels.Level1)
            {
                //if first part of chapter
                if (levelCounter == 1)
                {
                    //update buttons
                    nextStage.Update();

                    //conditionals for button input
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                }
                else if (levelCounter == 2)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
                else if (levelCounter == 3)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        //last stage, go to game
                        gameState = GameState.Game;

                        //reset the levelCounter
                        levelCounter = 1;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level2)
            {
                //if first part of chapter
                if (levelCounter == 1)
                {
                    //update buttons
                    nextStage.Update();

                    //conditionals for button input
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                }
                else if (levelCounter == 2)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
                else if (levelCounter == 3)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        //last stage, go to game
                        gameState = GameState.Game;

                        //reset the levelCounter
                        levelCounter = 1;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level3)
            {
                //if first part of chapter
                if (levelCounter == 1)
                {
                    //update buttons
                    nextStage.Update();

                    //conditionals for button input
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                }
                else if (levelCounter == 2)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
                else if (levelCounter == 3)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        //last stage, go to game
                        gameState = GameState.Game;

                        //reset the levelCounter
                        levelCounter = 1;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level4)
            {
                //if first part of chapter
                if (levelCounter == 1)
                {
                    //update buttons
                    nextStage.Update();

                    //conditionals for button input
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                }
                else if (levelCounter == 2)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        levelCounter++;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
                else if (levelCounter == 3)
                {
                    //updating buttons
                    nextStage.Update();
                    previousStage.Update();

                    //checking if buttons are pressed
                    if (nextStage.IsPressed())
                    {
                        //last stage, go to game
                        gameState = GameState.Game;

                        //reset the levelCounter
                        levelCounter = 1;
                    }
                    else if (previousStage.IsPressed())
                    {
                        levelCounter--;
                    }
                }
            }
        }

        /// <summary>
        /// the game logic for drawing chapters and chapter buttons
        /// </summary>
        public void DrawLevelLogic()
        {
            //-----------------------------------------------------------------
            if (level == Levels.Level1)
            {
                if (levelCounter == 1)
                {
                    DrawBackground(chapterStages["chapter1-1"]);
                }
                else if (levelCounter == 2)
                {
                    DrawBackground(chapterStages["chapter1-2"]);
                }
                else if (levelCounter == 3)
                {
                    DrawBackground(chapterStages["chapter1-3"]);
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level2)
            {
                if (levelCounter == 1)
                {
                    DrawBackground(chapterStages["chapter2-1"]);
                }
                else if (levelCounter == 2)
                {
                    DrawBackground(chapterStages["chapter2-2"]);
                }
                else if (levelCounter == 3)
                {
                    DrawBackground(chapterStages["chapter2-3"]);
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level3)
            {
                if (levelCounter == 1)
                {
                    DrawBackground(chapterStages["chapter3-1"]);
                }
                else if (levelCounter == 2)
                {
                    DrawBackground(chapterStages["chapter3-2"]);
                }
                else if (levelCounter == 3)
                {
                    DrawBackground(chapterStages["chapter3-3"]);
                }
            }
            //-----------------------------------------------------------------
            else if (level == Levels.Level4)
            {
                if (levelCounter == 1)
                {
                    DrawBackground(chapterStages["chapter4-1"]);
                }
                else if (levelCounter == 2)
                {
                    DrawBackground(chapterStages["chapter4-2"]);
                }
                else if (levelCounter == 3)
                {
                    DrawBackground(chapterStages["chapter4-3"]);
                }
            }

            //if the game state is in the game chapters draw the buttons
            if (gameState == GameState.GameChapters && levelCounter > 1)
            {
                nextStage.Draw(_spriteBatch);
                previousStage.Draw(_spriteBatch);
            }
            else if (gameState == GameState.GameChapters)
            {
                nextStage.Draw(_spriteBatch);
            }
        }

        /// <summary>
        /// helper method for drawing backgrounds
        /// </summary>
        /// <param name="asset">background asset being drawn</param>
        public void DrawBackground(Texture2D asset)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(
                asset,
                new Rectangle(0, 0, 1600, 960),
                Color.White);

            _spriteBatch.End();
        }

        /// <summary>
        /// helper method that resets the game variables
        /// </summary>
        public void Reset()
        {
            enemyMgr.Reset();
            planet.Reset();
            player.Reset();

            //reinstantiating the EnemyManager
            enemyMgr.Instantiate(planet, player);
        }

    }
}