using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeaTheRock2
{

    public delegate Circle PlayerHitbox();

    public delegate List<Circle> EnemyHitbox();

    /// <summary>
    /// Player class that controls "Gea", the games main character
    /// </summary>
    public class Player : GameObject
    {

        //fields:
        private int movementSpeed;
        private int health;
        private KeyboardState previousKBState;
        private Texture2D playerCollisionAsset;
        private bool isColliding;
        public event EnemyHitbox CollisionDetection;

        private Vector2 velocity;
        private Vector2 acceleration;
        private int timer;

        //Properties:
        //get/set for the player health
        public int Health
        {
            get { return health; }
            private set { health = value; }
        }

        //Constructors:
        /// <summary>
        /// parameterized constructor for the player class
        /// </summary>
        /// <param name="asset">Texture asset for the Player</param>
        /// <param name="position">Circle hitbox position for the Player</param>
        public Player(Texture2D asset, Texture2D playerCollisionAsset, Circle position)
            : base(asset, position)
        {
            this.playerCollisionAsset = playerCollisionAsset;
            this.movementSpeed = 6;
            this.health = 100;
            this.isColliding = false;

            this.velocity = new Vector2(0, 0);
            this.acceleration = new Vector2(0, 0);
            this.timer = 0;
        }

        //Methods:
        /// <summary>
        /// resets the player position
        /// </summary>
        public void Reset()
        {
            position.X = 200;
            position.Y = 430;
        }

        /// <summary>
        /// per frame update method for the Player class
        /// </summary>
        public override void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            GameTime gameTime = new GameTime();
            const float frictionalConstant = .2f;
            
            //up and down movement
            if (kbState.IsKeyDown(Keys.Up))
            {
                //position.Y -= movementSpeed;
                acceleration.Y = -1f;
                timer++;
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                //position.Y += movementSpeed;
                acceleration.Y = 1f;
                timer++;
            }
            else
            {
                acceleration.Y = 0;
            }

            //left and right movement
            if (kbState.IsKeyDown(Keys.Left))
            {
                //position.X -= movementSpeed;
                acceleration.X = -1f;
                timer++;
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                //position.X += movementSpeed;
                acceleration.X = 1f;
                timer++;
            }
            else
            {
                acceleration.X = 0f;
            }

            velocity.X += acceleration.X * timer;
            velocity.Y += acceleration.Y * timer;

            //limiting the minimum annd maximum velocities
            if (velocity.X > 10f)
            {
                velocity.X = 10f;                
            }
            else if (velocity.X < -10f)
            {
                velocity.X = -10f;
            }

            if (velocity.Y > 10f)
            {
                velocity.Y = 10f;
            }
            else if (velocity.Y < -10f)
            {
                velocity.Y = -10f;
            }

            //applying friction
            //left/right
            if (velocity.X < 0)
            {
                velocity.X += frictionalConstant;
            }
            else if (velocity.X > 0)
            {
                velocity.X -= frictionalConstant;
            }
            
            //up/down
            if (velocity.Y < 0)
            {
                velocity.Y += frictionalConstant;
            }
            else if (velocity.Y > 0)
            {
                velocity.Y -= frictionalConstant;
            }

            position.X += velocity.X;
            position.Y += velocity.Y;

            //left and right boundaries
            if (position.X <= 50)
            {
                position.X = 50;
            }
            else if (position.X >= 1550)
            {
                position.X = 1550;
            }

            //up and down boundaries
            if (position.Y <= 50)
            {
                position.Y = 50;
            }
            else if (position.Y >= 910)
            {
                position.Y = 910;
            }

            //check if Gea is colliding with any asteroids, if so colliding = true
            if (CollisionDetection != null)
            {
                List<Circle> hitboxes = new List<Circle>();
                hitboxes = CollisionDetection.Invoke();

                foreach (Circle hitbox in hitboxes)
                {
                    if (position.Intersects(hitbox))
                    {
                        //colliding is set to true and we break out of the loop
                        isColliding = true;
                        break;
                    }
                    else
                    {
                        isColliding = false;
                    }
                }
            }

            //reset the timer when the keys are released
            if (previousKBState != kbState)
            {
                timer = 0;
            }

            //setting previous state for potential use later
            previousKBState = kbState;
        }

        /// <summary>
        /// draws the Player object to the game window
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw Player</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //calculating the boundaries/top left coordinates for the rectangle
            int xValue = (int)(this.X - position.Radius);
            int yValue = (int)(this.Y - position.Radius);
            int bounds = position.Radius * 2;

            Rectangle circleRect = new Rectangle(xValue, yValue, bounds, bounds);

            if (!isColliding)
            {
                sb.Draw(
                    asset,
                    circleRect,
                    Color.White);
            }
            else if (isColliding)
            {
                sb.Draw(
                    playerCollisionAsset,
                    circleRect,
                    Color.White);
            }

            sb.End();
        }

        #region Event methods

        /// <summary>
        /// returns the Vector2 position for the asteroids to follow
        /// </summary>
        /// <returns>the vector location</returns>
        public Circle PlayerLocation()
        {
            return position;
        }

        #endregion
    }
}
