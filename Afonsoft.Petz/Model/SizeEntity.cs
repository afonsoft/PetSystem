using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class SizeEntity : IComparer<SizeEntity>, IComparable<SizeEntity>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Name + " (" + Description + ")";
        }

        #region IComparer Members

        public int Compare(SizeEntity x, SizeEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<UserEntity> Members

        public int CompareTo(SizeEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}