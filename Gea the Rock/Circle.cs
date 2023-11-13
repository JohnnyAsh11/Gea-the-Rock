using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeaTheRock2
{
    public class Circle
    {

        //fields:
        private Vector2 origin;
        private int radius;

        //Properties:
        //get property for the radius
        public int Radius { get { return radius; } }

        //get property for the origin of the circle
        public Vector2 Origin { get { return origin; } }

        //get/set property for the X value
        public float X
        {
            get { return origin.X; }
            set { origin.X = value; }
        }

        //get/set property for the Y value
        public float Y
        {
            get { return origin.Y; }
            set { origin.Y = value; }
        }

        //Constructors:
        /// <summary>
        /// parameterized constructor for the Circle class
        /// </summary>
        /// <param name="x">origin x value</param>
        /// <param name="y">origin y value</param>
        /// <param name="radius">the radius of the circle</param>
        public Circle(int x, int y, int radius)
        {
            this.radius = radius;
            this.origin = new Vector2(x, y);
        }

        //Methods:
        /// <summary>
        /// checks if two circles have collided
        /// </summary>
        /// <param name="other">the circle being checked</param>
        /// <returns>whether or not there was a collision</returns>
        public bool Intersects(Circle other)
        {
            //making several doubles so the math is pretty
            double distance;
            double first;
            double second;

            double xValue1 = this.X - radius;
            double yValue1 = this.Y - radius;
            
            double xValue2 = other.X - radius;
            double yValue2 = other.Y - radius;

            first = Math.Pow((xValue2 - xValue1), 2);
            second = Math.Pow((yValue2 - yValue1), 2);

            // (X2 - X1) squared and (Y2 - Y1) squared
            //first = Math.Pow((other.X - X), 2);
            //second = Math.Pow((other.Y - Y), 2);

            // the square root fo the sum of them both
            distance = Math.Sqrt((first + second));

            //checking if there is overlap / collision
            if (distance < (other.Radius + this.radius))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// debug draw hitbox method
        /// </summary>
        public void Draw(SpriteBatch sb, Texture2D debugCircle)
        {
            sb.Begin();

            Rectangle hitbox = new Rectangle((int)origin.X - radius, (int)origin.Y - radius, radius * 2, radius * 2);
            sb.Draw(debugCircle, hitbox, Color.White);

            sb.End();
        }

    }
}
