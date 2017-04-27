using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class VaccinationEntity : IComparer<VaccinationEntity>, IComparable<VaccinationEntity>
    {
        public int Id { get; set; }

        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Date.ToString("dd/MM/yyyy") + " - " + Comments;
        }


        #region IComparer Members

        public int Compare(VaccinationEntity x, VaccinationEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.Date.CompareTo(x.Date);
            return returnValue;
        }

        #endregion

        #region IComparable<VaccinationEntity> Members

        public int CompareTo(VaccinationEntity other)
        {
            return Date.CompareTo(other.Date);
        }

        #endregion
    }
}