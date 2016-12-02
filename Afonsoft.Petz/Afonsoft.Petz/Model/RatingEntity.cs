using System;
using System.Collections.Generic;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class RatingEntity : IComparer<RatingEntity>, IComparable<RatingEntity>
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public int InsertByUserId { get; set; }
        public int InsertByClientId { get; set; }
        public int RatingClientId { get; set; }
        public int RatingUserId { get; set; }
        public int RatingPetId { get; set; }
        public int RatingCompanyId { get; set; }
        public int RatingValue { get; set; }

        #region IComparer Members

        public int Compare(RatingEntity x, RatingEntity y)
        {
            int r = x.RatingClientId.CompareTo(y.RatingClientId);
            if (r == 0)
            {
                r = x.RatingUserId.CompareTo(y.RatingUserId);
                if (r == 0)
                {
                    r = x.RatingPetId.CompareTo(y.RatingPetId);
                    if (r == 0)
                        return x.Date.CompareTo(y.Date);
                    else
                        return r;
                }
                else
                    return r;
            }
            else
                return r;
        }

        #endregion

        #region IComparable<RatingEntity> Members

        public int CompareTo(RatingEntity other)
        {
            if (RatingClientId == other.RatingClientId)
                if (RatingUserId == other.RatingUserId)
                    if (RatingPetId == other.RatingPetId)
                        return Date.CompareTo(other.Date);
                    else
                        return other.RatingPetId.CompareTo(RatingPetId);
                else
                    return other.RatingUserId.CompareTo(RatingUserId);
            else
                return other.RatingClientId.CompareTo(RatingClientId);
        }

        #endregion
    }

    [Serializable]
    public class RatingHistoric : IComparer<RatingHistoric>, IComparable<RatingHistoric>
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
        public string InsertByName { get; set; }
        public int RatingValue { get; set; }

        public override string ToString()
        {
            return "<p><b>" + Date.ToString("dd/MM/yyyy") + " </b> - <span class='label label-warning'>" + RatingValue + "<i class='glyphicon glyphicon-star'></i></span> - " + InsertByName + " - <i>" + Comments + "</i></p>";
        }

        #region IComparer Members

        public int Compare(RatingHistoric x, RatingHistoric y)
        {
            int r = x.Date.CompareTo(y.Date);
            if (r == 0)
            {
                r = x.RatingValue.CompareTo(y.RatingValue);
                if (r == 0)
                {
                    r = String.Compare(x.InsertByName, y.InsertByName, StringComparison.Ordinal);
                    if (r == 0)
                        return x.Date.CompareTo(y.Date);
                    else
                        return r;
                }
                else
                    return r;
            }
            else
                return r;
        }

        #endregion

        #region IComparable<RatingHistoric> Members

        public int CompareTo(RatingHistoric other)
        {
            if (this.Date == other.Date)
                if (this.RatingValue == other.RatingValue)
                        return String.Compare(other.InsertByName, this.InsertByName, StringComparison.Ordinal);
                else
                    return other.RatingValue.CompareTo(this.RatingValue);
            else
                return other.Date.CompareTo(this.Date);
        }

        #endregion
    }
}