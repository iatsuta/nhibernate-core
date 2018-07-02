﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Runtime.Serialization;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Type;

namespace NHibernate.Cache.Entry
{
	using System.Threading.Tasks;
	using System.Threading;
	public sealed partial class CacheEntry
	{

		public static async Task<CacheEntry> CreateAsync(object[] state, IEntityPersister persister, bool unfetched, object version,
										ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return new CacheEntry
			{
				//disassembled state gets put in a new array (we write to cache by value!)
				DisassembledState = await (TypeHelper.DisassembleAsync(state, persister.PropertyTypes, null, session, owner, cancellationToken)).ConfigureAwait(false),
				AreLazyPropertiesUnfetched = unfetched || !persister.IsLazyPropertiesCacheable,
				Subclass = persister.EntityName,
				Version = version
			};
		}

		public Task<object[]> AssembleAsync(object instance, object id, IEntityPersister persister, IInterceptor interceptor,
		                         ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (!persister.EntityName.Equals(Subclass))
			{
				throw new AssertionFailure("Tried to assemble a different subclass instance");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object[]>(cancellationToken);
			}

			return AssembleAsync(DisassembledState, instance, id, persister, interceptor, session, cancellationToken);
		}

		private static async Task<object[]> AssembleAsync(object[] values, object result, object id, IEntityPersister persister,
		                                 IInterceptor interceptor, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			//assembled state gets put in a new array (we read from cache by value!)
			object[] assembledProps = await (TypeHelper.AssembleAsync(values, persister.PropertyTypes, session, result, cancellationToken)).ConfigureAwait(false);
	
			//from h3.2 TODO: reuse the PreLoadEvent
			PreLoadEvent preLoadEvent = new PreLoadEvent((IEventSource) session);
			preLoadEvent.Entity = result;
			preLoadEvent.State=assembledProps;
			preLoadEvent.Id = id;
			preLoadEvent.Persister=persister;

			IPreLoadEventListener[] listeners = session.Listeners.PreLoadEventListeners;
			for (int i = 0; i < listeners.Length; i++)
			{
				await (listeners[i].OnPreLoadAsync(preLoadEvent, cancellationToken)).ConfigureAwait(false);
			}

			persister.SetPropertyValues(result, assembledProps);

			return assembledProps;
		}
	}
}
