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
    public class Button
    {

        //Fields:
        private bool isHovering;
        private Texture2D normalButton;
        private Texture2D hoverButton;
        private Rectangle position;
        private MouseState previousState;

        //Properties:
        //get/set properties for the X and Y values of the button
        public int X { get { return position.X; } }
        public int Y { get { return position.Y; } }

        //Constructors:
        /// <summary>
        /// Parameterized constructor for the button class
        /// </summary>
        /// <param name="normalButton">button when not hovering</param>
        /// <param name="hoverButton">button when hovering</param>
        /// <param name="position">button's position in game window</param>
        public Button(Texture2D normalButton, Texture2D hoverButton, Rectangle position)
        {
            this.normalButton = normalButton;
            this.hoverButton = hoverButton;
            this.position = position;

            isHovering = false;
        }

        //Methods:
        /// <summary>
        /// checks for whether the button is being hovered over
        /// </summary>
        public void Update()
        {
            MouseState mState = Mouse.GetState();

            if (this.X < mState.X && mState.X < (this.X + position.Width) &&
                this.Y < mState.Y && mState.Y < (this.Y + position.Height))
            {
                isHovering = true;
            }
            else
            {
                isHovering = false;
            }
        }

        /// <summary>
        /// checks if the player presses the buttons on screen
        /// </summary>
        /// <returns>returns whether the buttons has been pressed or not</returns>
        public bool IsPressed()
        {
            MouseState mState = Mouse.GetState();

            if (this.X < mState.X && mState.X < (this.X + position.Width) &&
                this.Y < mState.Y && mState.Y < (this.Y + position.Height))
            {
                if (mState.LeftButton == ButtonState.Pressed &&
                    previousState != mState)
                {
                    previousState = mState;
                    return true;
                }
                previousState = mState;
                return false;
            }
            else
            {
                previousState = mState;
                return false;
            }
        }

        /// <summary>
        /// draws based on whether is hovering or not
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw to the game window</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();

            if (isHovering)
            {
                sb.Draw(
                    hoverButton,
                    position,
                    Color.White);
            }
            else if (!isHovering)
            {
                sb.Draw(
                    normalButton,
                    position,
                    Color.White);
            }

            sb.End();
        }


    }
}
