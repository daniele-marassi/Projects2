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
using Microsoft.EntityFrameworkCore;


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
    }

    public interface IBaseEntity
    {
        Guid ID { get; set; }
    }
    internal class Class1
    {
        private readonly IDataSource _dataSource;
        private readonly IDataDestination _dataDestination;

        public Class1(IDataSource dataSource, IDataDestination dataDestination)
        {
            _dataSource = dataSource;
            _dataDestination = dataDestination;
        }


        public void Run()
        {
            using (var context = _dataSource.GetDbContext())
            {
                HandleSync<SyncDataCalibration.ProductLine>();
                HandleSync<SyncDataCalibration.Project>();
            }
        }

        private void HandleSync<T>()
            where T : class, IBaseEntity
        {
            using (var sourceContext = _dataSource.GetDbContext())
            {
                using (var destContext = _dataDestination.GetDbContext())
                {
                    var source = sourceContext.Set<T>().First(_ => _.ID == 1);
                    var dest = destContext.Set<T>().First(_ => _.ID == 1);

                    if (source.ID > dest.ID)
                        destContext.Set<T>().Add(source);
                    else
                    {
                        AutoMapper.Mapper.Map(source, dest);
                        destContext.Set<T>().Update(dest);
                    }

                    destContext.SaveChanges();
                }
            }
        }

        public class IDataSource
        {
            public dynamic GetDbContext()
            {
                return new CalibrationDB_1Entities();
            }
        }

        public class IDataDestination
        {
            public dynamic GetDbContext()
            {
                return new CalibrationDB_2Entities();
            }
        }


    }

}
