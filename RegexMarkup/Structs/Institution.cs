using System.ComponentModel;

namespace RegexMarkup.Structs
{
    class Institution
    {
        [DisplayName("Institución")]
        public string institucion { get; set; }
        [DisplayName("Ciudad")]
        public string ciudad { get; set; }
        [DisplayName("País")]
        public string pais { get; set; }
        [DisplayName("Registros")]
        public int registros { get; set; }
    }
}
