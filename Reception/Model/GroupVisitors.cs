//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reception.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupVisitors
    {
        public int ID { get; set; }
        public int VisitorID { get; set; }
        public int CheckInID { get; set; }
    
        public virtual CheckIn CheckIn { get; set; }
        public virtual Visitor Visitor { get; set; }
    }
}
