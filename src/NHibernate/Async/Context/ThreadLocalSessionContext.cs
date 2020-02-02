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

using NHibernate;
using NHibernate.Engine;

namespace NHibernate.Context
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class ThreadLocalSessionContext : ICurrentSessionContext
	{

		private static async Task CleanupAnyOrphanedSessionAsync(ISessionFactory factory, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			ISession orphan = DoUnbind(factory, false);

			if (orphan != null)
			{
				log.Warn("Already session bound on call to bind(); make sure you clean up your sessions!");

				try
				{
					try
					{
						var transaction = orphan.GetCurrentTransaction();
						if (transaction?.IsActive == true)
							await (transaction.RollbackAsync(cancellationToken)).ConfigureAwait(false);
					}
					catch (OperationCanceledException) { throw; }
					catch (Exception ex)
					{
						log.Debug(ex, "Unable to rollback transaction for orphaned session");
					}
					orphan.Close();
				}
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					log.Debug(ex, "Unable to close orphaned session");
				}
			}
		}

		public static async Task BindAsync(ISession session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			ISessionFactory factory = session.SessionFactory;
			await (CleanupAnyOrphanedSessionAsync(factory, cancellationToken)).ConfigureAwait(false);
			DoBind(session, factory);
		}
	}
}
