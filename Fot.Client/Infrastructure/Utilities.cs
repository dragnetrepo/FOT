using System.Configuration;
using System.Web;

namespace Fot.Client.Infrastructure
{
    public class Utilities
    {
      

        #region Machine id / license scheme (simple sha)
        public static void CheckSerial()
        {
            var machineid = HttpContext.Current.Application["machineid"] as string;
            if (string.IsNullOrEmpty(machineid))
            {
                machineid = GetMachineID();
                HttpContext.Current.Application["machineid"] = machineid;

            }

            string hash = GetMachineHash(machineid);

            var AppCode = ConfigurationManager.AppSettings["AppCode"] as string;

            if (string.IsNullOrEmpty(AppCode))
            {
                //no serial redirect to yeye page
                HttpContext.Current.Server.Transfer("none.aspx");
            }

            if (!hash.Equals(AppCode))
            {
                //bad serial redirect to yeye page
                HttpContext.Current.Server.Transfer("none.aspx");
            }
        }

        public static string GetMachineID()
        {

            var str = "CPU >> " + FingerPrint.cpuId() + "\nBIOS >> " + FingerPrint.biosId() + "\nBASE >> " +
                     FingerPrint.baseId();


            return FingerPrint.GetHash(str);

        }


        public static string GetMachineHash(string machineId)
        {
            string code = machineId;

            string serial = FingerPrint.GetHash(code + "audax*147*temlogic");

            return serial;

        }
        #endregion
    }
}