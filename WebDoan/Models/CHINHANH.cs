namespace WebDoan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHINHANH")]
    public partial class CHINHANH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHINHANH()
        {
            PHIEUDAT = new HashSet<PHIEUDAT>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaCN { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(10)]
        public string HotLine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUDAT> PHIEUDAT { get; set; }
    }
}
