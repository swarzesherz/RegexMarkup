using System.ComponentModel;

namespace RegexMarkup.Structs
{
    class Country
    {
        [DisplayName("slug")]
        public string paisInstitucionSlug { get; set; }
        [DisplayName("País")]
        public string pais { get; set; }
    }
}
