using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GeaTheRock2
{
    /// <summary>
    /// Class for the main enemy of the game, Asteroids
    /// </summary>
    public class AsteroidEnemy : GameObject
    {
        //fields:
        private int movementSpeed;
        private bool isDead;
        private int timer;
        private int countdown;
        private Rectangle animationRect;

        /// <summary>
        /// event that tracks the player's location
        /// </summary>
        public event PlayerHitbox PlayerDestination;

        /// <summary>
        /// event that tracks the planet's hitbox
        /// </summary>
        public event PlanetHitbox PlanetHitbox;

        /// <summary>
        /// event that is invoked on collision between asteroid and planet
        /// </summary>
        public event Attacked AsteroidAttacked;

        //properties:
        //get property for the enemy's circle position
        public Circle Position { get { return position; } }

        //get property for the enemy object's texture asset
        public Texture2D Asset
        {
            get { return asset; }
        }
        //get property for the isDead bool
        public bool IsDead { get { return isDead; } }

        //get property for the animation countdown
        public int Countdown { get { return countdown; } }

        //Constructors:
        /// <summary>
        /// Parameterized constructor for the AsteroidEnemy class
        /// </summary>
        /// <param name="asset">Texture spriteSheet for the AsteroidEnemy</param>
        /// <param name="position">Circle location/hitbox of enemy</param>
        public AsteroidEnemy(Texture2D asset, Circle position)
            : base(asset, position)
        {
            this.movementSpeed = 2;
            this.isDead = false;
            this.timer = 5;
            this.animationRect = new Rectangle(0, 0, 669, 668);
            this.countdown = 3;
        }

        //Methods:
        /// <summary>
        /// per frame update method for the Enemy object class
        /// </summary>
        public override void Update()
        {
            //for now this will just go straight towards the player
            Circle playerHitbox;
            Circle planetHitbox;
            Vector2 center = new Vector2(800, 480);

            //only run through if the event has subscribers
            if (PlayerDestination != null &&
                PlanetHitbox != null &&
                !isDead)
            {
                playerHitbox = PlayerDestination.Invoke();
                planetHitbox = PlanetHitbox.Invoke();

                //left and right movement tracking
                if (center.X > position.X)
                {
                    position.X += movementSpeed;
                }
                else if(center.X < position.X)
                {
                    position.X -= movementSpeed;
                }

                //up and down movement tracking
                if (center.Y > position.Y)
                {
                    position.Y += movementSpeed;
                }
                else if (center.Y < position.Y)
                {
                    position.Y -= movementSpeed;
                }

                //checking for collision from player and planet
                if (playerHitbox.Intersects(position))
                {
                    isDead = true;
                }
                else if (planetHitbox.Intersects(position)) 
                { 
                    isDead = true;

                    if (AsteroidAttacked != null)
                    {
                        AsteroidAttacked.Invoke();
                    }
                }

            }

        }

        /// <summary>
        /// draws the Enemy object to the game window
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw Enemy object</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //calculating the boundaries/top left coordinates for the rectangle
            int xValue = (int)(this.X - position.Radius);
            int yValue = (int)(this.Y - position.Radius);
            int bounds = position.Radius * 2;

            Rectangle circleRect = new Rectangle(xValue, yValue, bounds, bounds);

            if (!isDead)
            {
                sb.Draw(
                    asset,
                    circleRect,
                    animationRect,
                    Color.White);
            }
            else
            {
                if (timer <= 0 && countdown > 0)
                {
                    animationRect.X += 669;
                    timer = 5;
                    countdown--;
                }

                sb.Draw(
                    asset,
                    circleRect,
                    animationRect,
                    Color.White);

                timer--;
            }

            sb.End();
        }

        /// <summary>
        /// method for Player class event to retrieve the Enemy class position
        /// </summary>
        /// <returns>AsteroidEnemy Location circle</returns>
        public Circle AsteroidLocation()
        {
            return position;
        }

    }
}
