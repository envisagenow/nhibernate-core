﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1675
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect.GetType() == typeof(MsSql2005Dialect) || dialect.GetType() == typeof(MsSql2008Dialect);
		}

		protected override void OnSetUp()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				for (var i = 0; i < 5; i++)
				{
					s.Save(new Person {FirstName = "Name" + i});
				}
				tx.Commit();
			}
		}

		[Test]
		public async Task ShouldWorkUsingDistinctAndLimitsAsync()
		{
			using (var s = OpenSession())
			{
				var q = s.CreateQuery("select distinct p from Person p").SetFirstResult(0).SetMaxResults(10);
				Assert.That(await (q.ListAsync()), Has.Count.EqualTo(5));
			}
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
	}
}