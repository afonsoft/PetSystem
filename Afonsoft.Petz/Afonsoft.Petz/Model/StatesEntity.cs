using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class StatesEntity : IComparer<StatesEntity>, IComparable<StatesEntity>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public CountriesEntity Country { get; set; }

        public override string ToString()
        {
            return Abbreviation + " - " + Name;
        }

        #region IComparer Members

        public int Compare(StatesEntity x, StatesEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<StatesEntity> Members

        public int CompareTo(StatesEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}