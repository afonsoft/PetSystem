using System;
using System.Collections.Generic;
using System.Globalization;


namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class AddressEntity : IComparer<AddressEntity>, IComparable<AddressEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public StatesEntity State { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public override string ToString()
        {
            return Address + ", " + Number + (State != null ? " - " + State.Abbreviation : "") +
                   (!string.IsNullOrEmpty(ZipCode) ? " - CEP: " + ZipCode : "");
        }

        /// <summary>
        /// Metodo para utlizar junto ao Google Maps
        /// </summary>
        public string Json()
        {
            return "{\"Id\": " + Id + ", \"Latitude\": " +
                   Latitude.Value.ToString(CultureInfo.InvariantCulture).Replace(",", ".") + ", \"Longitude\":" +
                   Longitude.Value.ToString(CultureInfo.InvariantCulture).Replace(",", ".") + ", \"Descricao\": \"" +
                   ToString() + "\", \"title\": \"" + (string.IsNullOrEmpty(Name) ? Address : Name) +
                   "\", \"icon\": \"/Markers/marcador.png\" }";
        }

        #region IComparer Members

        public int Compare(AddressEntity x, AddressEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = String.Compare(y.Name, x.Name, StringComparison.Ordinal);
            return returnValue;
        }

        #endregion

        #region IComparable<AddressEntity> Members

        public int CompareTo(AddressEntity other)
        {
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}