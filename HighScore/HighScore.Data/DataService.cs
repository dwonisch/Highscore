using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
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
            return session.QueryOver<Score>().List<Score>();
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

        public void Dispose()
        {
            if (session != null)
                session.Dispose();
            if (sessionFactory != null)
                sessionFactory.Dispose();
        }

        public IList<Player> GetPlayers()
        {
            return session.QueryOver<Player>().List<Player>();
        }
    }
}
