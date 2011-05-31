// Microsoft Reciprocal License (Ms-RL)
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// 
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// 
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// 
// (A) Reciprocal Grants- For any file you distribute that contains code from the software (in source code or binary format), you must provide recipients the source code to that file along with a copy of this license, which license will govern that file. You may license other files that are entirely your own work and do not contain code from the software under any terms you choose.
// (B) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (C) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
// (D) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
// (E) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
// (F) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        private PlayerConfiguration player = new PlayerConfiguration();
        private int test;
        private EnemyConfiguration collector = new EnemyConfiguration();
        private EnemyPredatorConfiguration predator = new EnemyPredatorConfiguration();
        private EnemyConfiguration mine;
        private EnemyDestroyerConfiguration destroyer;


        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }


        public int RaiseHumansPerLevelMin { get; set; }
        public int RaiseHumansPerLevelMax { get; set; }


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

        public PlayerConfiguration Player
        {
            get { return player; }
            set { player = value; }
        }

        public EnemyConfiguration Collector
        {
            get { return collector; }
            set { collector = value; }
        }

        public EnemyPredatorConfiguration Predator
        {
            get { return predator; }
            set { predator = value; }
        }

        public EnemyConfiguration Mine
        {
            get { return mine; }
            set
            {
                mine = value;
                mine.Speed = 80;
            }
        }

        public EnemyDestroyerConfiguration Destroyer
        {
            get { return destroyer; }
            set
            {
                destroyer = value;
                destroyer.Speed = 260f;
            }
        }

        public class PlayerConfiguration
        {
            private int shotCount = 10;
            private float speed = 64f;
            private int lifepoints = 5;
            private Vector2 maxBombSize = new Vector2(500);
            private float timeTillMaxBombSize = 2f;

            public PlayerConfiguration()
            {
            }

            public int ShotCount
            {
                get { return shotCount; }
                set { shotCount = value; }
            }
            public float Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public int Lifepoints
            {
                get { return lifepoints; }
                set { lifepoints = value; }
            }

            public Vector2 MaxBombSize
            {
                get { return maxBombSize; }
                set { maxBombSize = value; }
            }

            public float TimeTillMaxBombSize
            {
                get { return timeTillMaxBombSize; }
                set { timeTillMaxBombSize = value; }
            }

            public float ShootSpeed { get; set; }
        }

        public class EnemyConfiguration
        {
            private float speed = 16f;

            public int NumberFirstLevel { get; set; }
            public int AppearsInLevel { get; set; }
            public int MinAditionalPerLevel { get; set; }
            public int MaxAditionalPerLevel { get; set; }

            public EnemyConfiguration()
            {
            }

            public float Speed
            {
                get { return speed; }
                set { speed = value; }
            }
        }

        public class EnemyPredatorConfiguration : EnemyConfiguration
        {
            private int numberOfShots = 3;

            public EnemyPredatorConfiguration()
            {
            }

            public int NumberOfShots
            {
                get { return numberOfShots; }
                set { numberOfShots = value; }
            }
        }

        public class EnemyDestroyerConfiguration : EnemyConfiguration
        {
            public double MinTimeBetweenAppereance { get; set; }
            public float ProbabilityPerSeccond { get; set; }
            public float RaiseProbabilityPerLevel { get; set; }
            public int DamageCapathyty { get; set; }
        }

    }
}
