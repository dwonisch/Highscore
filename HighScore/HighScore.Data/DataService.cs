using NHibernate;
using NHibernate.Cfg;
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
        }

        private ISessionFactory sessionFactory;
        private ISession session;

        public void Dispose()
        {
            if (session != null)
                session.Dispose();
            if (sessionFactory != null)
                sessionFactory.Dispose();
        }
    }
}
