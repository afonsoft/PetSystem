using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class SchedulingEntity : IComparer<SchedulingEntity>, IComparable<SchedulingEntity>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AddressId { get; set; }
        public string Comments { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public PetsEntity Pet { get; set; }
        public ClientEntity Client { get; set; }
        public EmployeesEntity Employee { get; set; }
        public ServiceEntity Service { get; set; }
        public AddressEntity Address { get; set; }
        public StatusEntity Status { get; set; }
        public CompaniesEntity Company { get; set; }

        public override string ToString()
        {
            return Pet != null ? Pet.Name : Id.ToString();
        }

        #region IComparer Members

        public int Compare(SchedulingEntity x, SchedulingEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.CompanyId.CompareTo(x.CompanyId);
            if (returnValue == 0)
                if (y != null) if (x != null) returnValue = y.DateStart.CompareTo(x.DateStart);
            return returnValue;
        }

        #endregion

        #region IComparable<PetsEntity> Members

        public int CompareTo(SchedulingEntity other)
        {
            if (CompanyId == other.CompanyId)
                    return DateStart.CompareTo(other.DateStart);
                else
                    return CompanyId.CompareTo(other.CompanyId);
        }

        #endregion
    }

    [Serializable]
    public class SchedulingEntityInsertOrUpdate
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AddressId { get; set; }
        public int PetId { get; set; }
        public int ClientId { get; set; }
        public int? UserId { get; set; }
        public int? ServiceId { get; set; }
        public int? EmployeeId { get; set; }
        public string Comments { get; set; }

        private DateTime _dateStart = DateTime.Now;
        public DateTime DateStart
        {
            get
            {
                return _dateStart;
            }
            set
            {
                _dateStart = value;
            }
        }

        private DateTime _dateEnd = DateTime.Now.AddMinutes(30);
        public DateTime DateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                _dateEnd = value;
            }
        }
    }

    [Serializable]
    public class CompanyCalenderEntity : IComparer<CompanyCalenderEntity>, IComparable<CompanyCalenderEntity>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AddressId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        #region IComparer Members

        public int Compare(CompanyCalenderEntity x, CompanyCalenderEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.CompanyId.CompareTo(x.CompanyId);
            if (returnValue == 0)
                if (y != null) if (x != null) returnValue = y.DateStart.CompareTo(x.DateStart);
            return returnValue;
        }

        #endregion

        #region IComparable<CompanySchedulingEntity> Members

        public int CompareTo(CompanyCalenderEntity other)
        {
            if (CompanyId == other.CompanyId)
                return DateStart.CompareTo(other.DateStart);
            else
                return CompanyId.CompareTo(other.CompanyId);
        }

        #endregion
    }
}