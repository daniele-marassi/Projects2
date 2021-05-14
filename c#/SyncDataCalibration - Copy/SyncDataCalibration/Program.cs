using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SyncDataCalibration
{
    class test
    {
        public int MyProperty { get; set; }

        public int GetInt()
        {
            return Test();
        }

        protected virtual int Test()
        {
            return ++MyProperty;
        }
    }

    class OverrideTest : test
    {
        protected override int Test()
        {
            base.Test();
            return 100;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CalibrationDB_1Entities, CalibrationDB_2Entities>();
            });


            using (var db1 = new CalibrationDB_1Entities())
            {

                //var resultQuery = db1.Project.Where(_ => _.ProductLine.Name == "DH").Select(_ => new { ProductLine=_.ProductLine.Name, ProjectName = _.Name } ).ToList();

                using (var db2 = new CalibrationDB_2Entities())
                {





                    List<dynamic> tablesList = GetTablesList(db1.GetType());
                    DataTable db1ToDataTable = null;
                    DataTable db2ToDataTable = null;

                    foreach (var table in tablesList)
                    {





                        db1ToDataTable = DataTable(db1, $"SELECT * FROM {table.Name} WHERE IsDelete = 0", table.Name);

                        //foreach (var item in db1ToDataTable.Constraints)
                        //{
                        //    var fffffff = item;

                        //    var gg = 0;
                        //}


          

                        //foreach (DataRow row in db1ToDataTable.Rows)
                        //{
                        //    db2ToDataTable = DataTable(db2, $"SELECT * FROM {table.Name} WHERE IsDelete = 0 AND GUID = '{row["GUID"]}' AND DataModify < {row["DataModify"]}", table.Name);
                            
                        //}


                    }




                    var ttt = 0;
                    //IMapper mapper = config.CreateMapper();
                    //var source = new Source();
                    //var dest = mapper.Map<Source, Dest>(source);


                    //var name = "00002530"; //Console.ReadLine();

                    //var drawing = new Drawing { Name = name };
                    ////db.Drawing.Add(drawing);
                    ////db.SaveChanges();





                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static List<dynamic> GetTablesList(Type typeofModel)
        {
            List<dynamic> props = new List<dynamic>() { };

            foreach (var prop in typeofModel.GetProperties())
            {
                props.Add(prop);
            }
            return props;
        }



        public static DataTable DataTable(DbContext context, string sqlQuery, string tableName)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.Connection);
            DataTable dt = null;
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = context.Database.Connection;
                cmd.CommandType = CommandType.Text;



                //cmd.CommandText = $@"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME={tableName}";




                //cmd.CommandText = $@"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'";

                //using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                //{
                //    adapter.SelectCommand = cmd;

                //    dt = new DataTable();
                //    adapter.Fill(dt);

                //    foreach (DataRow row in dt.Rows)
                //    {

                //        var fffffff = row;

                //        var ee = 0;

                //    }
                //}


                cmd.Connection.Open();
                //DataTable schema = cmd.Connection.GetSchema("Tables", new string[] { null, null, tableName });
                DataTable schema = cmd.Connection.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.WriteLine("TABLE:" + tableName + " COLUMN:" + item);
                    }

                    
                }
                cmd.Connection.Close();


                //cmd.Connection.Open();
                //DataTable schema = cmd.Connection.GetSchema("Columns", new string[] { null, null, tableName });
                //foreach (DataRow row in schema.Rows)
                //    Console.WriteLine("TABLE:" + row.Field<string>("TABLE_NAME") +
                //                      " COLUMN:" + row.Field<string>("COLUMN_NAME"));
                //cmd.Connection.Close();


                //cmd.CommandText = sqlQuery;

                //using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                //{
                //    adapter.SelectCommand = cmd;

                //    dt = new DataTable();
                //    adapter.Fill(dt);

                //        return dt;
                //}
                return dt;
            }



    }


    }
}
