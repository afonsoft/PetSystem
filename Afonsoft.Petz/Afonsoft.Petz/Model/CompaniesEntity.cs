using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class CompaniesEntity : IComparer<CompaniesEntity>, IComparable<CompaniesEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }
        public AddressEntity[] Address { get; set; }
        public ServiceEntity[] Service { get; set; }
        public EmployeesEntity[] Employees { get; set; }
        public PhoneEntity[] Phones { get; set; }
        public WebCamEntity[] WebCam { get; set; }
        public WorkEntity[] WorkDay { get; set; }

        public override string ToString()
        {
            if(!string.IsNullOrEmpty(NickName))
                return Name + " (" + NickName + ")";
            return Name;
        }

        #region IComparer Members

        public int Compare(CompaniesEntity x, CompaniesEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<CompaniesEntity> Members

        public int CompareTo(CompaniesEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion

    }

    [Serializable]
    public class WebCamEntity : IComparer<WebCamEntity>, IComparable<WebCamEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }

        #region IComparer Members

        public int Compare(WebCamEntity x, WebCamEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<WebCamEntity> Members

        public int CompareTo(WebCamEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}