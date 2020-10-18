using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class CompositeSnap : ISnap
    {
        public int Tick { get; set; }
        public string Id { get; set; }

        public List<ISnap> snaps = new List<ISnap>();

        public bool EqualsCore(ISnap snap)
        {
            CompositeSnap other = snap as CompositeSnap;

            if (other == null) return false;
            if (other.Id != Id) return false;
            if (other.snaps.Count != snaps.Count) return false;
            for (int i = 0; i < other.snaps.Count; ++i)
            {
                if (!other.snaps[i].EqualsCore(snaps[i])) return false;
            }

            return true;
        }

        public bool EqualsValue(ISnap snap)
        {
            CompositeSnap other = snap as CompositeSnap;

            if (other == null) return false;
            if (other.Id != Id) return false;
            if (other.snaps.Count != snaps.Count) return false;
            for (int i = 0; i < other.snaps.Count; ++i)
            {
                if (!other.snaps[i].EqualsValue(snaps[i])) return false;
            }

            return true;
        }

        public ISnap Set(ISynchronizable synchronizable)
        {
            throw new NotImplementedException();
        }

        public ISnap Clone()
        {
            CompositeSnap clone = new CompositeSnap();

            clone.Tick = Tick;
            clone.Id = Id;
            snaps.ForEach(snap => clone.snaps.Add(snap.Clone()));

            return clone;
        }

        public override string ToString()
        {
            return $"[Tick {Tick}][CompositeSnap] Id : {Id}";
        }
    }
}
