using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class BreedEntity : IComparer<BreedEntity>, IComparable<BreedEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlReference { get; set; }
        public SubSpeciesEntity SubSpecies { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(UrlReference))
            {
                if (SubSpecies != null)
                    return "<a data-toggle=\"tooltip\" data-placement=\"top\" title=\"Mais informações desta Raça\" target=\"_blank\" href=\""+ UrlReference + "\" >" + SubSpecies.ToString() + " | " + Name + "<a/>";
                return "<a data-toggle=\"tooltip\" data-placement=\"top\" title=\"Mais informações desta Raça\" target=\"_blank\" href=\"" + UrlReference + "\" >" + Name + "<a/>";
            }else
            {
                if (SubSpecies != null)
                    return SubSpecies.ToString() + " | " + Name;
                return Name;
            }
        }

        #region IComparer Members

        public int Compare(BreedEntity x, BreedEntity y)
        {
            int returnValue = 1;
            if (x != null && y != null)
                returnValue = y.Name.CompareTo(x.Name);
            return returnValue;
        }

        #endregion

        #region IComparable<BreedEntity> Members

        public int CompareTo(BreedEntity other)
        {
            return this.Name.CompareTo(other.Name);
        }

        #endregion
    }
}