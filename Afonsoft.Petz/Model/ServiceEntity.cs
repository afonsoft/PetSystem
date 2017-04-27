using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class ServiceEntity : IComparer<ServiceEntity>, IComparable<ServiceEntity>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EstimatedTime { get; set; }

        public override string ToString()
        {
            return Id + " - " + Name;
        }

        #region IComparer Members

        public int Compare(ServiceEntity x, ServiceEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<ServiceEntity> Members

        public int CompareTo(ServiceEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}