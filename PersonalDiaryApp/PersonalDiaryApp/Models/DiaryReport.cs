using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace PersonalDiaryApp.Models
{
    public class DiaryReport
    {
        [Key]
        public int ItemId { get; set; }

        public string Title { get; set; }

     
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DiaryDate { get; set; } = DateTime.Now;

        
        public string Description { get; set; }
       
        [DisplayName(" Dailay image")]
        public string DiaryImage { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImgFile { get; set; }
    }
}

