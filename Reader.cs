//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsLib
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reader()
        {
            this.Logbooks = new HashSet<Logbook>();
            this.ReaderOrders = new HashSet<ReaderOrder>();
        }
    
        public int Reader_Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string CardNumber { get; set; }
        public string Password { get; set; }
        public System.DateTime DateOfSigningIn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Logbook> Logbooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReaderOrder> ReaderOrders { get; set; }
    }
}