//-----------------------------------------------------------------------
// <copyright file="TestPlayer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.MudEngine.Networking
{
    public class TestPlayer : GameComponent, IPlayer
    {
        public List<IGameComponent> Components
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRoom CurrentRoom
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IGender Gender
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IMobClass MobClass
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRace Race
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AddAbility(IStat ability)
        {
            throw new NotImplementedException();
        }

        public void AddMountPoint(IMountPoint mountPoint)
        {
            throw new NotImplementedException();
        }

        public IMountPoint FindMountPoint(string pointName)
        {
            throw new NotImplementedException();
        }

        public IStat[] GetAbilities()
        {
            throw new NotImplementedException();
        }

        public IMountPoint[] GetMountPoints()
        {
            throw new NotImplementedException();
        }

        public void MoveToRoom(IRoom room)
        {
            throw new NotImplementedException();
        }

        public void SetGender(IGender gender)
        {
            throw new NotImplementedException();
        }

        public void SetRace(IRace race)
        {
            throw new NotImplementedException();
        }

        protected override Task Load()
        {
            throw new NotImplementedException();
        }

        protected override Task Unload()
        {
            throw new NotImplementedException();
        }
    }
}
