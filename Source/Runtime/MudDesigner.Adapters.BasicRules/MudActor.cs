using System;
using System.Collections.Generic;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public abstract class MudActor : GameComponent, IActor
    {
        public IRoom CurrentRoom { get; protected internal set; }

        public IGender Gender { get; private set; }

        public IRace Race { get; private set; }
        
        public List<IGameComponent> Components { get; }
        
        public void MoveToRoom(IRoom room)
        {
            
        }

        public virtual void SetGender(IGender gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException(nameof(gender), "You can not assign a null gender to this actor.");
            }

            this.Gender = gender;
        }

        public virtual void SetRace(IRace race)
        {
            if (race == null)
            {
                throw new ArgumentNullException(nameof(race), "You can not assign a null race to this actor.");
            }

            this.Race = race;
        }
    }
}
