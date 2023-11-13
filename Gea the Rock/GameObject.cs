using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeaTheRock2
{
    public abstract class GameObject
    {

        //fields:
        protected Texture2D asset;
        protected Circle position;

        //Properties:
        //get/set property for position X coordinate
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        //get/set property for position Y coordinate
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        //Constructors:
        /// <summary>
        /// parameterized constructor for the abstract GameObject class
        /// </summary>
        /// <param name="asset">Texture2D asset for the GameObject</param>
        /// <param name="position">Circular position for the GameObject</param>
        public GameObject(Texture2D asset, Circle position)
        {
            this.asset = asset;
            this.position = position;
        }

        //Methods:
        /// <summary>
        /// per frame Update method
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// draws the GameObject to the game window
        /// </summary>
        /// <param name="sb"></param>
        public abstract void Draw(SpriteBatch sb);

    }
}
