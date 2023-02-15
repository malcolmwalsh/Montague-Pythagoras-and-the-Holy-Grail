﻿#nullable enable
namespace Assets.Game.Objects.Items
{
    internal class WoollyHat : IItem
    {
        // Fields
        private IObstacle? nemesisObstacle;

        // Properties
        public IObstacle? NemisisObstacle => nemesisObstacle;

        public void SetNemesis(IObstacle obstacle)
        {
            this.nemesisObstacle = obstacle;
        }
    }
}
