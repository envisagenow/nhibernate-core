﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace NHibernate.Id
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class SelectGenerator : AbstractPostInsertGenerator, IConfigurable
	{

		#region Nested type: SelectGeneratorDelegate

		public partial class SelectGeneratorDelegate : AbstractSelectingDelegate
		{

			// Since 5.2
			[Obsolete("Use or override BindParameters(ISessionImplementor session, DbCommand ps, IBinder binder) instead.")]
			protected internal override async Task BindParametersAsync(ISessionImplementor session, DbCommand ps, object entity, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				var entityPersister = (IEntityPersister) persister;
				var uniqueKeyPropertyNames = uniqueKeySuppliedPropertyNames ??
					PostInsertIdentityPersisterExtension.DetermineNameOfPropertiesToUse(entityPersister);
				for (var i = 0; i < uniqueKeyPropertyNames.Length; i++)
				{
					var uniqueKeyValue = entityPersister.GetPropertyValue(entity, uniqueKeyPropertyNames[i]);
					await (uniqueKeyTypes[i].NullSafeSetAsync(ps, uniqueKeyValue, i, session, cancellationToken)).ConfigureAwait(false);
				}
			}

			protected internal override async Task BindParametersAsync(ISessionImplementor session, DbCommand ps, IBinder binder, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				// 6.0 TODO: remove the "if" block and do the other TODO of this method
				if (persister is IEntityPersister)
				{
#pragma warning disable 618
					await (BindParametersAsync(session, ps, binder.Entity, cancellationToken)).ConfigureAwait(false);
#pragma warning restore 618
					return;
				}

				if (persister is ICompositeKeyPostInsertIdentityPersister compositeKeyPersister)
				{
					await (compositeKeyPersister.BindSelectByUniqueKeyAsync(session, ps, binder, uniqueKeySuppliedPropertyNames, cancellationToken)).ConfigureAwait(false);
					return;
				}

				// 6.0 TODO: remove by merging ICompositeKeyPostInsertIdentityPersister in IPostInsertIdentityPersister
				await (binder.BindValuesAsync(ps, cancellationToken)).ConfigureAwait(false);
			}

			protected internal override async Task<object> GetResultAsync(ISessionImplementor session, DbDataReader rs, object entity, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (!await (rs.ReadAsync(cancellationToken)).ConfigureAwait(false))
				{
					var message = "The inserted row could not be located by the unique key";
					if (uniqueKeySuppliedPropertyNames != null)
						message = $"{message} (supplied unique key: {string.Join(", ", uniqueKeySuppliedPropertyNames)})";
					throw new IdentifierGenerationException(message);
				}
				return await (idType.NullSafeGetAsync(rs, persister.RootTableKeyColumnNames, session, entity, cancellationToken)).ConfigureAwait(false);
			}
		}

		#endregion
	}
}
