using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeaTheRock2
{
    public class HealthBar
    {

        //fields:
        private Texture2D health;
        private Texture2D healthCanister;
        private Rectangle position;
        private Rectangle changePosition;

        /// <summary>
        /// event that tracks the planet's current health
        /// </summary>
        public event Health PlanetHealth;

        //Properties: - NONE - 

        //Constructors:
        /// <summary>
        /// parameterized constructor for the HealthBar class
        /// </summary>
        /// <param name="health">the base image for the healthbar</param>
        /// <param name="healthCanister">the outer pretty image for the healthbar</param>
        /// <param name="position">Rectangle position for the healthbar</param>
        public HealthBar(
            Texture2D health,
            Texture2D healthCanister,
            Rectangle position)
        {
            this.position = position;
            this.health = health;
            this.healthCanister = healthCanister;
            this.changePosition = new Rectangle(position.X,
                                                position.Y,
                                                position.Width,
                                                position.Height);
        }

        //Methods:
        /// <summary>
        /// per frame update method of the HealthBar class
        /// </summary>
        public void Update()
        {
            int planetHealth;

            if (PlanetHealth != null)
            {
                planetHealth = 8 * PlanetHealth.Invoke();

                changePosition.Width = planetHealth;
            }
        }

        /// <summary>
        /// draws the HealthBar to the game window
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //empty health first
            sb.Draw(
                health,
                position,
                Color.White);

            //then the red health
            sb.Draw(
                health,
                changePosition,
                Color.Red);

            //finally the canister
            sb.Draw(
                healthCanister,
                position,
                Color.White);

            sb.End();
        }

    }
}
