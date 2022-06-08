using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib_Model.Models
{
    public class Publisher
    {
        [Key]
        public int Publicher_Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public List<Book> Books { get; set; } /*1 Publisher se nalazi u vise knjiga*/ /* isto kao 1 na 1 samo ovdje imamo List<Book> za navigaciju , dok je FK potreban na strani Book.cs*/
    }
}
