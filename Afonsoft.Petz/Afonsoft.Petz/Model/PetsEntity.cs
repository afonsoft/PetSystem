using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    public enum EnumSex { Other = 0, Female = 1, Male = 2 }

    [Serializable]
    public class PetsEntity : IComparer<PetsEntity>, IComparable<PetsEntity>
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Document { get; set; }
        public string Color { get; set; }
        public string Facebook { get; set; }
        public double Rating { get; set; }
        public EnumSex Sex { get; set; } = EnumSex.Other;
        public Double? Weight { get; set; }
        public DateTime? Birthday { get; set; }
        public SubSpeciesEntity SubSpecies { get; set; }
        public BreedEntity Breed { get; set; }
        public SizeEntity Size { get; set; }

        public override string ToString()
        {
            if(!string.IsNullOrEmpty(NickName))
                return Name + " ( " + NickName + " )";
            return Name;
        }

        #region IComparer Members

        public int Compare(PetsEntity x, PetsEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<PetsEntity> Members

        public int CompareTo(PetsEntity other)
        {
            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }

    [Serializable]
    public class HistoricEntity : IComparer<HistoricEntity>, IComparable<HistoricEntity>
    {
        public EmployeesEntity Employee { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return Date.ToString("dd/MM/yyyy") + " : " + (Employee?.User != null ? Employee.User.UserName : "Employee") + " - " + Comments;
        }

        #region IComparer Members

        public int Compare(HistoricEntity x, HistoricEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.Date.CompareTo(x.Date);
            return returnValue;
        }

        #endregion

        #region IComparable<HistoricEntity> Members

        public int CompareTo(HistoricEntity other)
        {
            return Date.CompareTo(other.Date);
        }

        #endregion

    }
}