using System;
using System.Collections.Generic;


namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class SubSpeciesEntity : IComparer<SubSpeciesEntity>, IComparable<SubSpeciesEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SpeciesEntity Species { get; set; }
        public override string ToString()
        {
            if (Species != null)
                return Species + " | " + Name;
            return Name;
        }

        #region IComparer Members

        public int Compare(SubSpeciesEntity x, SubSpeciesEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<SubSpeciesEntity> Members

        public int CompareTo(SubSpeciesEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}