#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Grass : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Grass));

		public Grass() : base(2)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override void DoPhysics(Level level)
		{
			if (level.GetSubtractedLight(Coordinates + BlockCoordinates.Up) < 4)
			{
				Block dirt = BlockFactory.GetBlockById(3);
				dirt.Coordinates = Coordinates;
				level.SetBlock(dirt, true, false, false);
			}
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (!isRandom) return;

			var lightLevel = level.GetSubtractedLight(Coordinates + BlockCoordinates.Up);
			if (lightLevel < 4 /* && check opacity */)
			{
				Block dirt = BlockFactory.GetBlockById(3);
				dirt.Coordinates = Coordinates;
				level.SetBlock(dirt, true, false, false);
			}
			else
			{
				if (lightLevel >= 9)
				{
					Random random = new Random();
					for (int i = 0; i < 4; i++)
					{
						var coordinates = Coordinates + new BlockCoordinates(random.Next(3) - 1, random.Next(5) - 3, random.Next(3) - 1);
						Block next = level.GetBlock(coordinates);
						if (next is Dirt && next.Metadata == 0)
						{
							Block nextUp = level.GetBlock(coordinates + BlockCoordinates.Up);
							if (nextUp.IsTransparent && (nextUp.BlockLight >= 4 || nextUp.SkyLight >= 4))
							{
								level.SetBlock(new Grass {Coordinates = coordinates});
							}
						}
					}
				}
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; //Drop dirt block
		}
	}
}