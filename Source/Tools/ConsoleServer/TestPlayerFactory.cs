//-----------------------------------------------------------------------
// <copyright file="TestPlayerFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Networking
{
    using System;
    using MudDesigner.MudEngine.Actors;
    using Commanding;

    public class TestPlayerFactory : IPlayerFactory
    {
        public IPlayer CreatePlayer(IActorCommand initialCommand)
        {
            return new TestPlayer();
        }
    }
}
