using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class ClientEntity : IComparer<ClientEntity>, IComparable<ClientEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string Facebook { get; set; }
        public double Rating { get; set; }
        public EnumSex Sex { get; set; } = EnumSex.Other;
        public DateTime? Birthday { get; set; }
        public PhoneEntity[] Phones { get; set; }
        public AddressEntity[] Address { get; set; }
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NickName))
                return Name + " (" + NickName + ")";
            return Name;
        }

        #region IComparer Members

        public int Compare(ClientEntity x, ClientEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<ClientEntity> Members

        public int CompareTo(ClientEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }

}