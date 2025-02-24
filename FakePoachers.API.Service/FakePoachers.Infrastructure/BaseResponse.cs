using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoachers.Infrastructure
{
    public class BaseResponse
    {
        public int Result { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public string AdditionalInformation { get; set; } = string.Empty;
    }
}
