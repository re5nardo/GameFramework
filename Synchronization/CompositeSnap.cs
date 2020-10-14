using System.Collections.Generic;

namespace GameFramework
{
    public class CompositeSnap : ISnap
    {
        public int Tick { get; set; }
        public string Id { get; set; }

        public List<ISnap> snaps = new List<ISnap>();

        public ISnap Clone()
        {
            CompositeSnap clone = new CompositeSnap();

            clone.Tick = Tick;
            clone.Id = Id;
            snaps.ForEach(snap => clone.snaps.Add(snap.Clone()));

            return clone;
        }

        public bool EqualsMeaningfully(ISnap snap)
        {
            CompositeSnap other = snap as CompositeSnap;

            if (other == null) return false;

            if (other.Id != Id) return false;
            if (other.snaps.Count != snaps.Count) return false;
            for (int i = 0; i < other.snaps.Count; ++i)
            {
                if (!other.snaps[i].EqualsMeaningfully(snaps[i])) return false;
            }

            return true;
        }
    }
}
