
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

    public delegate Circle PlanetHitbox();

    public delegate void Attacked();

    public delegate int Health();

    /// <summary>
    /// class for the Plant Object that the player defends
    /// </summary>
    public class Planet : GameObject
    {

        //fields:
        private int health;
        private bool isDead;
        private int timer;
        private int countdown;
        private Rectangle animationRect;
        private int endCounter;

        //Properties:
        public int Health { get { return health; } }

        //set property for the planet's texture
        public Texture2D Asset
        {
            set { asset = value; }
        }

        //get property for the animation countdown
        public int Countdown { get { return countdown; } }

        //Constructors:
        /// <summary>
        /// parameterized constructor for the Planet Class
        /// </summary>
        /// <param name="asset">Texture spritesheet for the plant</param>
        /// <param name="position">circle position for the planet</param>
        public Planet(Texture2D asset, Circle position)
            : base(asset, position)
        {
            this.health = 100;
            this.isDead = false;
            this.timer = 900;
            this.animationRect = new Rectangle(0, 0, 612, 613);
            this.countdown = 3;
            this.endCounter = 1;
        }

        //Methods:
        /// <summary>
        /// resets all relevant variables for a new game round
        /// </summary>
        public void Reset()
        {
            this.health = 100;
            isDead = false;
            countdown = 3;
            animationRect.X = 0;
            timer = 900;
            endCounter = 1;
        }

        //Update method is empty - could contain something in the future
        /// <summary>
        /// per frame update method for the planet class
        /// </summary>
        public override void Update()
        {

        }

        /// <summary>
        /// draws the planet to the game window
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //calculating the boundaries/top left coordinates for the rectangle
            int xValue = (int)(this.X - position.Radius);
            int yValue = (int)(this.Y - position.Radius);
            int bounds = position.Radius * 2;

            Rectangle circleRect = new Rectangle(xValue, yValue, bounds, bounds);


            //for the finishing win
            if (countdown == 1 && timer == 0)
            {
                timer = 300;

                if (endCounter == 0)
                {
                    countdown--;
                }
                else
                {
                    //move to the last phase
                    animationRect.X += 612;
                    endCounter--;
                }
            }
            //for the general animation
            if (timer == 0)
            {
                timer = 900;

                if (countdown > 1)
                {
                    animationRect.X += 612;

                    countdown--;
                }
            }            
            timer--;

            sb.Draw(
                asset,
                circleRect,
                animationRect,
                Color.White);

            sb.End();
        }

        #region Event methods

        /// <summary>
        /// gives the planet's location for the Asteroid event to track hitbox
        /// </summary>
        /// <returns>the planet's hitbox</returns>
        public Circle PlanetLocation()
        {
            return position;
        }

        /// <summary>
        /// method that runs only when a collision occurs between an
        /// asteroid and planet object
        /// </summary>
        public void TakeDamage()
        {
            health -= 10;

            if (health <= 0)
            {
                isDead = true;
            }
        }

        /// <summary>
        /// gives out the planet's health to any event that needs it
        /// </summary>
        /// <returns>the planet's health</returns>
        public int PlanetHealth()
        {
            return health;
        }

        #endregion

    }
}
