using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class PhoneEntity : IComparer<PhoneEntity>, IComparable<PhoneEntity>
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            return Phone;
        }

        #region IComparer Members

        public int Compare(PhoneEntity x, PhoneEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Phone, x.Phone, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<PhoneEntity> Members

        public int CompareTo(PhoneEntity other)
        {
            return String.Compare(Phone, other.Phone, StringComparison.Ordinal);
        }

        #endregion
    }
}