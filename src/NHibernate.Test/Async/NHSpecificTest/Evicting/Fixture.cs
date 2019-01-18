﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;

namespace NHibernate.Test.NHSpecificTest.Evicting
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			base.OnSetUp();
			using (var session = Sfi.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Save(new Employee
				{
                    Id = 1,
					FirstName = "a",
					LastName = "b"
				});
				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = Sfi.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Delete(session.Load<Employee>(1));

				tx.Commit();
			}
			base.OnTearDown();
		}


		[Test]
		public async Task Can_evict_entity_from_sessionAsync()
		{
			using (var session = Sfi.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var employee = await (session.LoadAsync<Employee>(1));
				Assert.IsTrue(session.Contains(employee));

				await (session.EvictAsync(employee));

				Assert.IsFalse(session.Contains(employee));

				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task Can_evict_non_persistent_objectAsync()
		{

			using (var session = Sfi.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var employee = new Employee();
				Assert.IsFalse(session.Contains(employee));

				await (session.EvictAsync(employee));

				Assert.IsFalse(session.Contains(employee));

				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task Can_evict_when_trying_to_evict_entity_from_another_sessionAsync()
		{

			using (var session1 = Sfi.OpenSession())
			using (var tx1 = session1.BeginTransaction())
			{

				using (var session2 = Sfi.OpenSession())
				using (var tx2 = session2.BeginTransaction())
				{
					var employee = await (session2.LoadAsync<Employee>(1));
					Assert.IsFalse(session1.Contains(employee));
					Assert.IsTrue(session2.Contains(employee));

					await (session1.EvictAsync(employee));

					Assert.IsFalse(session1.Contains(employee));

					Assert.IsTrue(session2.Contains(employee));

					await (tx2.CommitAsync());
				}
				
				await (tx1.CommitAsync());
			}
		}
	
	}
}
