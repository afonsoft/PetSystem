using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class UserEntity : IComparer<UserEntity>, IComparable<UserEntity>
    {        
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsSystemAdmin { get; set; }
        public string LastIpAddress { get; set; }
        public double Rating { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region IComparer Members

        public int Compare(UserEntity x, UserEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<UserEntity> Members

        public int CompareTo(UserEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}

