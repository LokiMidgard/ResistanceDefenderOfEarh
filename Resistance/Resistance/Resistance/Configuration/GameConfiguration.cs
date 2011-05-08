using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mitgard.Resistance.Configuration
{
    public class GameConfiguration
    {
        private float enemyShotSpeed = 48f;
        private int level;
        private int noHumans;
        private int noPredator;
        private int noCollector;
        private int noMine;
        private bool enemyTargetting;


        public int WorldWidth { get { return 4000; } }
        public int WorldHeight { get { return 800; } }


       

        public float EnemyShotSpeed
        {
            get { return enemyShotSpeed; }
            set { enemyShotSpeed = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int NoHumans
        {
            get { return noHumans; }
            set { noHumans = value; }
        }

        public int NoPredator
        {
            get { return noPredator; }
            set { noPredator = value; }
        }

        public int NoCollector
        {
            get { return noCollector; }
            set { noCollector = value; }
        }

        public int NoMine
        {
            get { return noMine; }
            set { noMine = value; }
        }

        public bool EnemyTargetting
        {
            get { return enemyTargetting; }
            set { enemyTargetting = value; }
        }



    }
}
