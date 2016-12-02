using System;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public enum StatusEnum
    {
        EventCreateByCompany = 1,
        EventCreateByClient = 2,
        EventChangedByClient = 3,
        EventChangedByCompany = 4,
        EventConfirmedByClient = 5,
        EventConfirmedByCompany = 6,
        EventCanceledByClient = 7,
        EventCanceledByCompany = 8
    }

    [Serializable]
    public class StatusEntity
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public string BackgroundColor { get;  set; }
        public string BorderColor { get;  set; }
        public string TextColor { get;  set; }
    }
}