﻿using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public interface IDataService : IDisposable
    {

    }

    public class DataService : IDataService
    {
        public DataService()
        {
            var configuration = new Configuration();
                configuration.Configure();
            configuration.AddAssembly(typeof(DataService).Assembly);

            sessionFactory = configuration.BuildSessionFactory();
            session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Commit;

            var schemaExport = new SchemaUpdate(configuration);
            schemaExport.Execute(true, true);
        }

        private static void BuildSchema()
        {

        }

        private ISessionFactory sessionFactory;
        private ISession session;

        public IEnumerable<Score> GetScores(DateTime date) {
            return session.QueryOver<Score>().Where(s => s.Date == date).List<Score>();
        }

        public void SaveScores(IEnumerable<Score> scores) {
            using (var transaction = session.BeginTransaction()) {
                try
                {
                    foreach (var score in scores) {
                        session.SaveOrUpdate(score);
                    }

                    transaction.Commit();
                }
                catch {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteScores(IEnumerable<Score> scores) {
            using (var transaction = session.BeginTransaction()) {
                try {
                    foreach (var score in scores) {
                        session.Delete(score);
                    }

                    transaction.Commit();
                } catch {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Dispose()
        {
            if (session != null)
                session.Dispose();
            if (sessionFactory != null)
                sessionFactory.Dispose();
        }

        public IList<string> GetPlayers()
        {
            var result = session.QueryOver<Score>().Select(Projections.Group<Score>(p => p.Player)).List<string>();
            return result.ToList();
        }
    }
}
