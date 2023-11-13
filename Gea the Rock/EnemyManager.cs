using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeaTheRock2
{
    /// <summary>
    /// class that manages all of the AsteroidEnemies for the game
    /// </summary>
    public class EnemyManager
    {

        //fields:
        private List<AsteroidEnemy> enemies;
        private List<AsteroidEnemy> printedEnemies;
        private bool isInstantiated;
        private AsteroidEnemy baseEnemy;
        private int numOfEnemies;

        //class utility fields:
        private Random rng;
        private int asteroidCounter;
        private int timer;

        //Properties:
        //get property for the printed enemies Count
        public int EnemyCount { get { return printedEnemies.Count; } }

        //get property for whether the enemyManager lists are instantiated
        public bool IsInstantiated { get { return isInstantiated; } }

        //Constructors:
        /// <summary>
        /// parameterized constructor for the enemy class
        /// </summary>
        /// <param name="baseEnemy">the base enemy that other will copy</param>
        public EnemyManager(AsteroidEnemy baseEnemy, int numOfEnemies)
        {
            this.baseEnemy = baseEnemy;
            this.numOfEnemies = numOfEnemies;
            this.rng = new Random();
            this.enemies = new List<AsteroidEnemy>();
            this.printedEnemies = new List<AsteroidEnemy>();
            this.isInstantiated = false;
            this.asteroidCounter = 0;
            this.timer = 0;
        }


        //Methods:
        /// <summary>
        /// Instantiates the enemies List with AsteroidEnemy objects
        /// </summary>
        public void Instantiate(Planet planet, Player player)
        {
            if (!isInstantiated)
            {
                for (int i = 0; i < numOfEnemies; i++)
                {

                    //randomizing spawn location
                    int x = rng.Next(0, 1500);
                    int y = RandomHeight();

                    //generating enemy
                    enemies.Add(new AsteroidEnemy(
                        baseEnemy.Asset,
                        new Circle(x, y, 75)));

                    enemies[i].PlayerDestination += player.PlayerLocation;
                    enemies[i].PlanetHitbox += planet.PlanetLocation;
                    enemies[i].AsteroidAttacked += planet.TakeDamage;
                }

                //changing instantiation variable to true
                isInstantiated = true;
            }
        }

        /// <summary>
        /// resets all relevant variables
        /// </summary>
        public void Reset()
        {
            printedEnemies.Clear();
            enemies.Clear();
            isInstantiated = false;
            asteroidCounter = 0;
            timer = 0;
        }

        /// <summary>
        /// Per frame update method for the EnemyManager class
        /// </summary>
        public void Update()
        {
            //every 2 seconds print another enemy
            if (timer >= 60 && asteroidCounter < numOfEnemies)
            {
                //adding an enemy to the print list
                printedEnemies.Add(
                    enemies[asteroidCounter]);

                //upping the asteroid counter
                asteroidCounter++;

                //resetting the timer
                timer = 0;
            }
            timer++;

            //updates all enemies and checks if they're dead
            //and removes them from the list if so
            for (int i = 0; i < printedEnemies.Count; i++)
            {
                //Update all enemies in printing List
                if (printedEnemies.Contains(printedEnemies[i]))
                {
                    printedEnemies[i].Update();
                }

                //if the enemy is dead, remove it from the list
                if (printedEnemies[i].Countdown == 0)
                {
                    printedEnemies.RemoveAt(i);
                }
            }
            
        }

        /// <summary>
        /// draws the enemies specified for printing
        /// </summary>
        /// <param name="sb">SpriteBatch used for drawing</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (AsteroidEnemy enemy in printedEnemies)
            {
                enemy.Draw(sb);
            }
        }

        /// <summary>
        /// helper method to randomize the height at which enemies spawn
        /// </summary>
        /// <returns>either the top or bottom of screen</returns>
        public int RandomHeight()
        {
            int decider = rng.Next(1, 3);

            if (decider == 1)
            {
                return -50;
            }
            else
            {
                return 960;
            }
        }

        /// <summary>
        /// method for the Player class event animations
        /// </summary>
        /// <returns>a list of enemy hitboxes</returns>
        public List<Circle> RetrivingHitboxes()
        {
            List<Circle> hitboxes = new List<Circle>();

            foreach (AsteroidEnemy enemy in enemies)
            {
                hitboxes.Add(enemy.Position);
            }

            return hitboxes;
        }

    }
}
