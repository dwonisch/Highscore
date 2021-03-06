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
            configuration.Configure(GetType().Assembly, "HighScore.Data.hibernate.cfg.xml");
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

        public void ClearData()
        {
            var playersWithScores = session.QueryOver<Score>().Select(s => s.Player).List<Player>().Distinct().ToList();
            var allPlayers = session.QueryOver<Player>().List();

            Delete(allPlayers.Except(playersWithScores).ToList()); //Delete all players that didn't have any scores
            Delete(session.QueryOver<Score>().List()); // Delete all scores to start with a new database
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
            Delete(scores);
        }

        private void Delete(IEnumerable<object> objects)
        {
            if (!objects.Any())
                return;
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    foreach (var o in objects)
                    {
                        session.Delete(o);
                    }

                    transaction.Commit();
                }
                catch
                {
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

                yield return new ResultScore(DateTime.Now,group.Key.Name, score1, score2);
            }
        }

        public IEnumerable<ResultScore> GetChildrenHighscores() {
            var scores = session.QueryOver<Score>().JoinQueryOver<Player>(s => s.Player).Where(p => p.Child).List();

            var groups = scores.GroupBy(s => s.Player, s => s.Values.Select(v => v.Value));

            foreach (var group in groups) {
                var values = group.SelectMany(s => s).OrderByDescending(s => s).Take(2).ToList();
                int score1 = 0;
                int score2 = 0;

                if (values.Count > 0)
                    score1 = values[0];
                if (values.Count > 1)
                    score2 = values[1];

                yield return new ResultScore(DateTime.Now, group.Key.Name, score1, score2);
            }
        }

        public IEnumerable<ResultScore> GetDayScores() {
            var scores = session.QueryOver<Score>().JoinQueryOver<Player>(s => s.Player).List();

            var groups = scores.GroupBy(s => s.Date);

            foreach (var group in groups) {
                var score = group.OrderByDescending(s => s.Values.Select(v => v.Value).Max()).First();
                yield return new ResultScore(score.Date, score.Player.Name, score.Values.Select(v => v.Value).Max(), score.Values.Select(v => v.Value).Min());
            }
        }

        public IEnumerable<ResultScore> GetPlayerShots() {
            var scores = session.QueryOver<Score>().JoinQueryOver<Player>(s => s.Player).List();

            var groups = scores.GroupBy(s => s.Player);

            foreach (var group in groups) {
                yield return new ResultScore(DateTime.Now, group.Key.Name, group.Sum(g => g.Count), group.Select(g => g.Date).Distinct().Count());
            }
        }

        public IEnumerable<ResultScore> GetDayShots() {
            var scores = session.QueryOver<Score>().List();

            var groups = scores.GroupBy(s => s.Date);

            foreach (var group in groups) {
                yield return new ResultScore(group.Key, string.Empty, group.Sum(g => g.Count), group.Select(g => g.Player).Distinct().Count());
            }

            yield return new ResultScore(DateTime.Now, "Gesamt:", groups.SelectMany(g => g.Select(p => p.Count)).Sum(), groups.SelectMany(g => g.Select(p => p.Player)).Distinct().Count());
        }

        public Settings GetSettings() {
            var setting = session.QueryOver<Settings>().Take(1).SingleOrDefault();
            return setting ?? new Settings();
        }

        public void SaveSettings(Settings settings) {
            using (var transaction = session.BeginTransaction()) {
                try {
                    session.SaveOrUpdate(settings);
                    transaction.Commit();
                } catch {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
