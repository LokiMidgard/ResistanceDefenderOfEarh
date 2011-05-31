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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = System.String;
using TOutput = System.String;
using System.Xml.Linq;
using Mitgard.Resistance.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace ResistanceContentPipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Resistance Configuration Processor")]
    public class ContentProcessor1 : ContentProcessor<XDocument, GameConfiguration>
    {
    const String NAMESPACE = "http://resistance.res";
        public override GameConfiguration Process(XDocument input, ContentProcessorContext context)
        {

            //Debugger.Break();

            var root = input.Root;
            //System.Xml.Linq.XName f = new System.Xml.Linq.XName; 
            var world = root.Element(XName.Get("World",NAMESPACE));
            var worldWidth = world.Attribute("width");
            var worldHeight = world.Attribute("height");
            var EnemyShootspeed = world.Element(XName.Get("EnemyShotSpeed",NAMESPACE));

            var player = root.Element(XName.Get("Player",NAMESPACE));
            var maxLifePoints = player.Element(XName.Get("MaxLifePoints",NAMESPACE));
            var PlayerSpeed = player.Element(XName.Get("Speed",NAMESPACE));
            var PlayerShootSpeed = player.Element(XName.Get("ShootSpeed",NAMESPACE));
            var PlayerShootCount = player.Element(XName.Get("ShootCount",NAMESPACE));
            var maxBombSize = player.Element(XName.Get("MaxBombSize",NAMESPACE));
            var timeToMaxBombSize = player.Element(XName.Get("TimeToMaxDetonation",NAMESPACE));

            var humans = root.Element(XName.Get("Human",NAMESPACE));
            var humanNumberFirstLevel = humans.Attribute("numberFirstLevel");
            var humanRaisePerLevelMin = humans.Attribute("raisePerLevelMin");
            var humanRaisePerLevelMax = humans.Attribute("raisePerLevelMax");

            var enemyPredator = root.Element(XName.Get("EnemyPredator",NAMESPACE));
            var enemyPredatorNumberFirstLevel = enemyPredator.Attribute("numberFirstLevel");
            var enemyPredatroAppearsInLevel = enemyPredator.Attribute("appearsInLevel");
            var enemyPredatorMinAditionalPerLevel = enemyPredator.Attribute("minAditionalPerLevel");
            var enemyPredatorMaxAditionalPerLevel = enemyPredator.Attribute("maxAditionalPerLevel");
            var enemyPredatorSpeed = enemyPredator.Element(XName.Get("Speed",NAMESPACE));
            var enemyPredatorTargetting = enemyPredator.Element(XName.Get("Targeting",NAMESPACE));


            var enemyCollector = root.Element(XName.Get("EnemyCollector",NAMESPACE));
            var enemyCollectorNumberFirstLevel = enemyCollector.Attribute("numberFirstLevel");
            var enemyCollectorAppearsInLevel = enemyCollector.Attribute("appearsInLevel");
            var enemyCollectorMinAditionalPerLevel = enemyCollector.Attribute("minAditionalPerLevel");
            var enemyCollectorMaxAditionalPerLevel = enemyCollector.Attribute("maxAditionalPerLevel");
            var enemyCollectorSpeed = enemyCollector.Element(XName.Get("Speed",NAMESPACE));

            var enemyMine = root.Element(XName.Get("EnemyMine",NAMESPACE));
            var enemyMineNumberFirstLevel = enemyMine.Attribute("numberFirstLevel");
            var enemyMineAppearsInLevel = enemyMine.Attribute("appearsInLevel");
            var enemyMineMinAditionalPerLevel = enemyMine.Attribute("minAditionalPerLevel");
            var enemyMineMaxAditionalPerLevel = enemyMine.Attribute("maxAditionalPerLevel");
            var enemyMineSpeed = enemyMine.Element(XName.Get("Speed",NAMESPACE));

            var enemyDestroyer = root.Element(XName.Get("EnemyDestroyer",NAMESPACE));
            var enemyDestroyerAppearsInLevel = enemyDestroyer.Attribute("appearsInLevel");
            var enemyDestroyerMinTimeBetweenAppereance = enemyDestroyer.Attribute("minTimeBetweenAppereance");
            var enemyDestroyerProbabilityPerSeccond = enemyDestroyer.Attribute("probabilityPerSeccond");
            var enemyDestroyerRaiseProbabilityPerLevel = enemyDestroyer.Attribute("raiseProbabilityPerLevel");
            var enemyDestroyerSpeed = enemyDestroyer.Element(XName.Get("Speed",NAMESPACE));
            var enemyDestroyerDamageCapathity = enemyDestroyer.Element(XName.Get("DamageCapathyty",NAMESPACE));




            GameConfiguration g = new GameConfiguration();
            g.WorldHeight = int.Parse(worldHeight.Value, CultureInfo.InvariantCulture.NumberFormat);
            g.WorldWidth = int.Parse(worldWidth.Value, CultureInfo.InvariantCulture.NumberFormat);
            g.RaiseHumansPerLevelMax = int.Parse(humanRaisePerLevelMax.Value, CultureInfo.InvariantCulture.NumberFormat);
            g.RaiseHumansPerLevelMin = int.Parse(humanRaisePerLevelMin.Value, CultureInfo.InvariantCulture.NumberFormat);

            g.EnemyShotSpeed = float.Parse(EnemyShootspeed.Value, CultureInfo.InvariantCulture.NumberFormat);
            g.Level = 1;
            g.NoHumans = int.Parse(humanNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat);
            g.NoPredator = int.Parse(enemyPredatroAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat) == 1 ? int.Parse(enemyPredatorNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) : 0;
            g.NoCollector = int.Parse(enemyCollectorAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat) == 1 ? int.Parse(enemyCollectorNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) : 0;
            g.NoMine = int.Parse(enemyMineAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat) == 1 ? int.Parse(enemyMineNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) : 0;
            g.EnemyTargetting = bool.Parse(enemyPredatorTargetting.Value);
            g.Player = new GameConfiguration.PlayerConfiguration() { Lifepoints = int.Parse(maxLifePoints.Value, CultureInfo.InvariantCulture.NumberFormat), MaxBombSize = new Vector2(float.Parse(maxBombSize.Element(XName.Get("X",NAMESPACE)).Value, CultureInfo.InvariantCulture.NumberFormat), float.Parse(maxBombSize.Element(XName.Get("Y",NAMESPACE)).Value, CultureInfo.InvariantCulture.NumberFormat)), ShotCount = int.Parse(PlayerShootCount.Value, CultureInfo.InvariantCulture.NumberFormat), Speed = float.Parse(PlayerSpeed.Value, CultureInfo.InvariantCulture.NumberFormat), TimeTillMaxBombSize = float.Parse(timeToMaxBombSize.Value, CultureInfo.InvariantCulture.NumberFormat), ShootSpeed = float.Parse(PlayerShootSpeed.Value, CultureInfo.InvariantCulture.NumberFormat) };
            g.Collector = new GameConfiguration.EnemyConfiguration() { Speed = float.Parse(enemyCollectorSpeed.Value, CultureInfo.InvariantCulture.NumberFormat), AppearsInLevel = int.Parse(enemyCollectorAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MaxAditionalPerLevel = int.Parse(enemyCollectorMaxAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MinAditionalPerLevel = int.Parse(enemyCollectorMinAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), NumberFirstLevel = int.Parse(enemyCollectorNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) };
            g.Predator = new GameConfiguration.EnemyPredatorConfiguration() { Speed = float.Parse(enemyPredatorSpeed.Value, CultureInfo.InvariantCulture.NumberFormat), AppearsInLevel = int.Parse(enemyPredatroAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MaxAditionalPerLevel = int.Parse(enemyPredatorMaxAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MinAditionalPerLevel = int.Parse(enemyPredatorMinAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), NumberFirstLevel = int.Parse(enemyCollectorNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) };
            g.Mine = new GameConfiguration.EnemyConfiguration() { Speed = float.Parse(enemyMineSpeed.Value, CultureInfo.InvariantCulture.NumberFormat), AppearsInLevel = int.Parse(enemyMineAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MaxAditionalPerLevel = int.Parse(enemyMineMaxAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MinAditionalPerLevel = int.Parse(enemyMineMinAditionalPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), NumberFirstLevel = int.Parse(enemyMineNumberFirstLevel.Value, CultureInfo.InvariantCulture.NumberFormat) };
            g.Destroyer = new GameConfiguration.EnemyDestroyerConfiguration() { Speed = float.Parse(enemyDestroyerSpeed.Value, CultureInfo.InvariantCulture.NumberFormat), NumberFirstLevel = 1, MinAditionalPerLevel = 0, MaxAditionalPerLevel = 0, AppearsInLevel = int.Parse(enemyDestroyerAppearsInLevel.Value, CultureInfo.InvariantCulture.NumberFormat), MinTimeBetweenAppereance = double.Parse(enemyDestroyerMinTimeBetweenAppereance.Value, CultureInfo.InvariantCulture), ProbabilityPerSeccond = float.Parse(enemyDestroyerProbabilityPerSeccond.Value, CultureInfo.InvariantCulture.NumberFormat), RaiseProbabilityPerLevel = float.Parse(enemyDestroyerRaiseProbabilityPerLevel.Value, CultureInfo.InvariantCulture.NumberFormat), DamageCapathyty = int.Parse(enemyDestroyerDamageCapathity.Value, CultureInfo.InvariantCulture.NumberFormat) };



            return g;

        }


    }
}