using System;
// ReSharper disable InconsistentNaming

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class EventObject
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string editable { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }

        public override string ToString()
        {
            return title;
        }
    }
}