//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UroborosApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reminder
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int material_id { get; set; }
        public System.DateTime reminder_datetime { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual users users { get; set; }
    }
}
