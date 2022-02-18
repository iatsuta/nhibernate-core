using System;
using System.Collections;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Test.GeneratedTest
{
	[TestFixture]
	public class QuotingFaultTest : TestCase
	{
		[Test]
		public void FaultTest()
		{
			Guid domainObjectId;

			using (var session = OpenSession())
			using (var tran = session.BeginTransaction())
			{
				var domainObject = new QuoteDomainObject1() { Name = Guid.NewGuid().ToString(), Order = 123 };

				session.Save(domainObject);

				domainObjectId = domainObject.Id;

				tran.Commit();
			}
			
			using (var session = OpenSession())
			{
				var data1 = session.Query<QuoteDomainObject1>().ToArray();

				var data2 = session.Query<QuoteDomainObject2>().ToArray();


				//Assert
				Assert.AreEqual(1, data1.Length);
				Assert.AreEqual(1, data2.Length);

				Assert.AreEqual(domainObjectId, data1[0].Id);
				Assert.AreEqual(domainObjectId, data2[0].Id);
			}
		}

		protected override void Configure(Configuration configuration)
		{
			var dialect = NHibernate.Dialect.Dialect.GetDialect(configuration.GetDerivedProperties());

			SchemaMetadataUpdater.QuoteTableAndColumns(configuration, dialect);
		}

		protected override string[] Mappings
		{
			get { return new string[] { "QuoteDomainObject1.hbm.xml", "QuoteDomainObject2.hbm.xml" }; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2008Dialect;
		}

		protected override string CacheConcurrencyStrategy
		{
			get { return null; }
		}
	}
}
