using NHibernate;
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
            if (!scores.Any())
                return;
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
            var result = session.QueryOver<Player>().Select(Projections.Group<Player>(p => p.Name)).List<string>();
            return result.ToList();
        }

        public Tuple<bool, Player> GetPlayer(string name) {
            var player = session.QueryOver<Player>().Where(p => p.Name == name).Take(1).SingleOrDefault();

            if (player == null) {
                return new Tuple<bool, Player>(false, new Player() { Name = name });
            }

            return new Tuple<bool, Player>(true, player);
        }

        public IEnumerable<ResultScore> GetHighscores(bool female) {
            var scores = session.QueryOver<Score>().JoinQueryOver<Player>(s => s.Player).Where(p => p.Female == female).List();

            var groups = scores.GroupBy(s => s.Player, s => s.Values.Select(v => v.Value));

            foreach (var group in groups) {
                var values = group.SelectMany(s => s).OrderByDescending(s => s).Take(2).ToList();
                int score1 = 0;
                int score2 = 0;

                if(values.Count > 0)
                    score1 = values[0];
                if(values.Count > 1)
                    score2 = values[1];

                yield return new ResultScore(group.Key.Name, score1, score2);
            }
        }
    }
}
