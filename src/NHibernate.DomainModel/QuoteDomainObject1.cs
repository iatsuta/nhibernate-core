using System;

namespace NHibernate.DomainModel
{
	public class QuoteDomainObject1
	{
		private Guid id;

		private string name;

		private int order;

		public QuoteDomainObject1()
		{
		}
		public virtual Guid Id
		{
			get => this.id;
			set => this.id = value;
		}

		public virtual string Name
		{
			get => this.name;
			set => this.name = value;
		}

		public virtual int Order
		{
			get => this.order;
			set => this.order = value;
		}
	}
}
