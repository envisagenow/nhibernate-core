﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.Driver
{
	public partial class BasicResultSetsCommand: IResultSetsCommand
	{

		public virtual async Task<DbDataReader> GetReaderAsync(int? commandTimeout, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var batcher = Session.Batcher;
			SqlType[] sqlTypes = Commands.SelectMany(c => c.ParameterTypes).ToArray();
			ForEachSqlCommand((sqlLoaderCommand, offset) => sqlLoaderCommand.ResetParametersIndexesForTheCommand(offset));
			var command = batcher.PrepareQueryCommand(CommandType.Text, sqlString, sqlTypes);
			if (commandTimeout.HasValue)
			{
				command.CommandTimeout = commandTimeout.Value;
			}
			log.Info(command.CommandText);
			BindParameters(command);
			return await (BatcherDataReaderWrapper.CreateAsync(batcher, command, cancellationToken)).ConfigureAwait(false);
		}
	}

	public partial class BatcherDataReaderWrapper: DbDataReader
	{

		public static async Task<BatcherDataReaderWrapper> CreateAsync(IBatcher batcher, DbCommand command, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return new BatcherDataReaderWrapper(batcher, command)
			{
				reader = await (batcher.ExecuteReaderAsync(command, cancellationToken)).ConfigureAwait(false)
			};
		}
	}
}
