using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class EmployeesEntity : IComparer<EmployeesEntity>, IComparable<EmployeesEntity>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public UserEntity User { get; set; }
        public Boolean IsCompanyAdmin { get; set; }

        public override string ToString()
        {
            return Id + " - " + (User != null ? User.Name : "");
        }

        #region IComparer Members

        public int Compare(EmployeesEntity x, EmployeesEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.User.CompareTo(x.User);
            return returnValue;
        }

        #endregion

        #region IComparable<EmployeesEntity> Members

        public int CompareTo(EmployeesEntity other)
        {
            return User.CompareTo(other.User);
        }

        #endregion
    }
}