namespace WebDoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOAIDICHVU")]
    public partial class LOAIDICHVU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAIDICHVU()
        {
            DICHVU = new HashSet<DICHVU>();
        }

        [Key]
        [StringLength(50)]
        public string MaLoaiDV { get; set; }

        [StringLength(50)]
        public string TenLoaiDV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DICHVU> DICHVU { get; set; }
    }
}
