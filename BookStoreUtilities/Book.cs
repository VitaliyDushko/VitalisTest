using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreUtilities
{
    public class Book
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class ErrorResponse
    {
        public List<string> Errors { get; set; }
    }

}
