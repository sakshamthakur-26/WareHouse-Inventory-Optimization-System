using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZoneApi.Model
{
    public class Zone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ZoneId { get; set; }

        public string Name { get; set; }
        public int MaxCapacity { get; set; }


        /////////////////////////////////////////

        public int CurrentUsage { get; set; }
    }
}
