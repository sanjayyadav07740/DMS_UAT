using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.BusinessLogic
{
    public class Licencing
    {
        public int LicencingID { get; set; }
        public int MaxCountNamedUser { get; set; }
        public int MaxCountConcurrentUser { get; set; }
        public int LoginCountNamed { get; set; }
        public int LoginCountConcurrent { get; set; }
        public string ServerIPAddress { get; set; }
        public byte[] EncryptedMaxCountNamedUser { get; set; }
        public byte[] EncryptedMaxCountConcurrentUser { get; set; }
        public byte[] EncryptedLoginCountConcurrent { get; set; }
        public byte[] EncryptedServerIPAddress { get; set; }
       
    }
}
