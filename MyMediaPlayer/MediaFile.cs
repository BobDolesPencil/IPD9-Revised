//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyMediaPlayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class MediaFile
    {
        public int mediaID { get; set; }
        public byte[] sourceMedia { get; set; }
        public string mediaType { get; set; }
        public string userId { get; set; }
        public string title { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
