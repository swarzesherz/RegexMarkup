using System.ComponentModel;

namespace RegexMarkup.Structs
{
    class Institution
    {
        [DisplayName("Institución")]
        public string e_100u { get; set; }
        [DisplayName("Ciudad")]
        public string e_100w { get; set; }
        [DisplayName("País")]
        public string e_100x { get; set; }
        [DisplayName("Registros")]
        public int registros { get; set; }
    }
}
