using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Fot.Admin.Infrastructure
{
    public class Utilities
    {
        public static string FormatHtmlContent(string content)
        {

            string path = HttpContext.Current.Request.Path;


            path = path.Replace(@"/Client", string.Empty);
            path = path.Replace(@"/client", string.Empty);
            


            string path_nofile = Path.GetDirectoryName(path);

            path_nofile = Path.Combine(path_nofile, "img");

            path_nofile = path_nofile.Replace("\\", "/");

            string replace = path_nofile; // "/faceoftesting/img/";

            string real_url = HttpContext.Current.Server.MapPath(replace);

            real_url = real_url.Replace("\\", "/");

            string newContent = content.Replace(replace, real_url);


            return newContent;



        }


        public static string FormatHtmlContentForQuestion(string content)
        {

            string path = HttpContext.Current.Request.Path;

            path = path.Replace(@"/Client", string.Empty);
            path = path.Replace(@"/client", string.Empty);




            string path_nofile = Path.GetDirectoryName(path);

            path_nofile = Path.Combine(path_nofile, "resources");

            path_nofile = path_nofile.Replace("\\", "/");

            string replace = path_nofile; // "/faceoftesting/img/";

            string real_url = HttpContext.Current.Server.MapPath(replace);

            real_url = real_url.Replace("\\", "/");

            string newContent = content.Replace(replace, real_url);


            return newContent;



        }


        public static void BulkInsert<T>(string connection, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BulkCopyTimeout = 0;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                    //Dirty hack to make sure we only have system data types 
                    //i.e. filter out the relationships/collections
                                           .Cast<PropertyDescriptor>()
                                           .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                           .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }




        #region Machine id / license scheme (simple sha)
        public static void CheckSerial2()
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