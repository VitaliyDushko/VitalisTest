using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BookStoreUtilities
{
    public class GenericResponseFromApiService<T>
    {
        public T Content { get; set; }
        public List<string> Errors { get; set; }
    }
}
    