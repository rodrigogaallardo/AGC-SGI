

namespace SGI.Model
{
    using System;
    
    public partial class aspnet_Membership_GetUserByName_Result
    {
        public string Email { get; set; }
        public string PasswordQuestion { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public Nullable<System.DateTime> Column1 { get; set; }
        public System.DateTime LastPasswordChangedDate { get; set; }
        public System.Guid UserId { get; set; }
        public bool IsLockedOut { get; set; }
        public System.DateTime LastLockoutDate { get; set; }
    }
}
