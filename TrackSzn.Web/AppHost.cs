﻿using System.Web.Mvc;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using TrackSzn.Models;

namespace TrackSzn.Web
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Track Workout API", typeof(AppHost).Assembly) {}

        public override void Configure(Container container)
        {
            this.SetConfig(new HostConfig()
            {
                HandlerFactoryPath = "api"
            });

            Plugins.Add(new MiniProfilerFeature());
            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)
                {
                    ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
                });

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Athlete>();
                db.DropAndCreateTable<Event>();
                db.DropAndCreateTable<Lift>();
                db.DropAndCreateTable<Meet>();
                db.DropAndCreateTable<AthleteLift>();
                db.DropAndCreateTable<AthletePerformance>();

                db.InsertAll(SeedData.Athletes);
                db.InsertAll(SeedData.Events);
                db.InsertAll(SeedData.Lifts);
                db.InsertAll(SeedData.Meets);
                db.InsertAll(SeedData.AthleteLifts);
                db.InsertAll(SeedData.AthletePerformances);
            }

            ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
        }
    }
}